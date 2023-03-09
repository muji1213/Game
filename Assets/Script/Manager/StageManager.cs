using System.Collections;
using UnityEngine;
using OnStage;

//このスクリプトはステージシーンの管理をする
public class StageManager : MonoBehaviour
{
    [Header("UIマネージャー")] [SerializeField] OnStage.UIManager uiManager;

    [Header("ステージ関連")]
    [Header("ノーダメージの時の加算スコア")] [SerializeField] int noDamageAddPoint;
    [Header("HP満タンになるまで走った時の加算スコア")] [SerializeField] int maxHPAddPoint;

    [Header("カメラ関連")]
    [Header("フィニッシュ演出用のカメラ")] [SerializeField] Camera finishCam;
    [Header("ステージ入場カメラ")] [SerializeField] StageEnterCamera stageEnterCamera;
    [Header("メインカメラに表示させるキャンバス")] [SerializeField] Canvas canvas;
    [Header("フィニッシュカメラに映すキャンバス")] [SerializeField] GameObject finishCanvas;

    [Header("オブジェクト関連")]
    [Header("Player")] [SerializeField] Player_Controller player;
    [Header("ターゲットのゲームオブジェクト")] [SerializeField] GameObject target;

    [Header("音関連")]
    [Header("BGM")] [SerializeField] AudioClip bgm;
    [Header("ターゲットとの衝突時のSE")] [SerializeField] AudioClip colSE;
    [Header("衝突SEの音量")] [SerializeField] [Range(0, 1)] float colSEVol = 1;
    [Header("ターゲットが吹っ飛ぶSE")] [SerializeField] AudioClip blowSE;
    [Header("吹っ飛びSEの音量")] [SerializeField] [Range(0, 1)] float blowSEVol = 1;
    [Header("リザルト表示時のSE")] [SerializeField] AudioClip successSE;
    [Header("リザルトSEの音量")] [SerializeField] [Range(0, 1)] float successSEVol = 1;
    [Header("ミスしたときのSE")] [SerializeField] AudioClip missedSE;
    [Header("ミスSEの音量")] [SerializeField] [Range(0, 1)] float missSEVol = 1;

    //ステージで取得したポイントを保持、ステージクリア後、ゲームマネージャに加算する
    private int temporarilyScore;

    //リザルトの加点判定
    private bool isNodamage;
    private bool isMaxHP;

    //トータルスコア
    private int totalScore = 0;

    //ステージのゴール位置
    private float goalPos;

    //ポーズ中かどうか
    private bool isPose = false;

    //UIおよびプレイヤーの操作を有効にしたか
    private bool isStart = false;

    //カメラ追従用のスクリプト
    private CameraFollower cameraFollower;

    //フィニッシュ演出用のカメラ
    private FinishCamera finishCamera;

    private void Start()
    {
        //各種コンポーネント
        cameraFollower = GameObject.Find("Main Camera").GetComponent<CameraFollower>();
        finishCamera = GameObject.Find("Finish Camera").GetComponent<FinishCamera>();

        //フィニッシュカメラに映すキャンバスは非表示に
        finishCanvas.SetActive(false);

        //ステージで取得したポイントを初期化
        temporarilyScore = 0;

        //リザルト判定の初期値を設定
        isNodamage = true;
        isMaxHP = false;

        //ゴール位置を記録
        goalPos = target.transform.position.z;

        //BGM
        BGMManager.I.PlayBgm(bgm);

        //スタート時はプレイヤーを停止
        StopPlayerControll();

        //UIを初期化
        uiManager.Inicialize();
    }

    private void Update()
    {
        //カウントダウンが終わるまで操作無効
        if (!uiManager.isCountDownEnd())
        {
            //フェードが終わったら
            if (uiManager.isFadeEnd())
            {
                //スタート演出を流す
                stageEnterCamera.StartMovie();

                //演出が終わったら、プレイヤーにカメラを追従させる
                if (stageEnterCamera.MovieFinished())
                {
                    cameraFollower.enabled = true;
                }
                else
                {
                    return;
                }

                //カウントダウンスタート
                uiManager.ActiveCountDownUI();
            }
            return;
        }
        else
        {
            //UIを配置する
            if (!isStart)
            {
                //プレイヤーの操作を有効にする
                MovePlayer();

                //UIを有効にする
                uiManager.ActiveGaugeUI();
                uiManager.ActiveScoreUI();

                isStart = true;
            }
        }
    }

    private void FixedUpdate()
    {
        //ゲージをためる
        uiManager.ChargeGaugeUI(GetRunTime());
    }

    /// <summary>
    /// UIを表示
    /// ハートの個数をセット
    /// </summary>
    public void SetUIAndLife(int frisbeeHP, bool isLifeUp)
    {
        //UIを表示する
        uiManager.ActiveEvaluateUIAndLife(frisbeeHP);

        //最大HPまで走ったフラグをTrueにする

        //ライフアップを取っていた場合、4が最大HPになる
        if (isLifeUp)
        {
            if (frisbeeHP == 4)
            {
                isMaxHP = true;
            }
            else
            {
                isMaxHP = false;
            }
        }
        //取っていない場合、3が最大HPになる
        else
        {
            if (frisbeeHP == 3)
            {
                isMaxHP = true;
            }
            else
            {
                isMaxHP = false;
            }
        }
    }

    //ステージで取得したポイントを一時的に保持する
    //スコアテキストを変更する
    //途中でリタイアした際、ポイントを加算できないようにする
    public void StorePoint(int point)
    {
        temporarilyScore += point;
        uiManager.ChangeScoreUI(temporarilyScore);
    }

    //ステージクリア後、ゲームマネージャにポイントを加算
    public void AddPoint(int score)
    {
        GameManager.I.Point += score;

        //一時ポイントを0に
        temporarilyScore = 0;
    }

    /// <summary>
    /// フリスビーが障害物に衝突時、ライフUIを減らし、ノーダメージフラグはfalseに
    /// </summary>
    public void TakeDamage()
    {
        //UIのハートを一個減らす
        uiManager.ReduceHPUI();

        //振動させる
        cameraFollower.Shake(0.3f, 1.0f);

        //ノーダメージフラグはOFFに
        if (isNodamage)
        {
            isNodamage = false;
        }
    }

    //フリスビーが的に当たった時の演出
    public IEnumerator Finish()
    {
        //SE
        SEManager.I.PlaySE(colSEVol, colSE);

        //フィニッシュキャンバスをONに
        ActiveFinishCanvas();

        //カメラ切り替え
        cameraFollower.ChangeFinishCamera();

        //振動させる
        finishCamera.Shake(0.5f, 1.0f);

        //一定時間止める（画面振動はする）
        Time.timeScale = 0.0f;

        yield return new WaitForSecondsRealtime(0.7f);

        Time.timeScale = 1.0f;

        //SE
        SEManager.I.PlaySE(blowSEVol, blowSE);

        //フィニッシュキャンバスをOFFに
        DeactiveFinishCanvas();

        //リザルト呼び出し
        Invoke("ActiveResult", 2.0f);
    }

    /// <summary>
    /// ポーズメソッド
    /// Pキーで呼び出しされる
    /// </summary>
    public void Pose()
    {
        //ポーズ中でなければ
        if (!isPose)
        {
            //ポーズフラグをおろす
            isPose = true;

            //ポーズUI表示
            uiManager.ActivePoseUI();

            //ゲーム時間を停止
            Time.timeScale = 0.0f;
        }
        else
        {
            //ポーズ中なら
            isPose = false;

            //ポーズUIを非表示
            uiManager.DeactivePoseUI();

            //ゲーム時間を戻す
            Time.timeScale = 1.0f;
        }
    }

    /// <summary>
    /// プレイヤーもしくはフリスビーが死亡した際に呼び出す
    /// </summary>
    public void Retry(float deadPos)
    {
        StopPlayerControll();

        //SE
        SEManager.I.PlaySE(missSEVol, missedSE);

        //どれだけの割合進んだかの割合を出す
        var value = Mathf.Clamp((float)deadPos / (float)goalPos, 0, 1);

        uiManager.ActiveRetryUI(value);
    }

    /// <summary>
    /// プレイヤーの走った時間が最大HPになるまでに必要な時間のどれくらいの割合かを返す
    /// これはゲージに使用する
    /// </summary>
    /// <returns></returns>
    public float GetRunTime()
    {
        return Mathf.Clamp((float)player.runTime / (float)player.MaxHPTime, 0, 1);
    }

    /// <summary>
    /// プレイヤーの操作を有効にする
    /// </summary>
    public void MovePlayer()
    {
        player.enabled = true;
    }

    /// <summary>
    /// プレイヤーの操作を無効にする
    /// </summary>
    public void StopPlayerControll()
    {
        player.enabled = false;
    }

    /// <summary>
    /// フィニッシュ演出用のキャンバスを有効にする
    /// </summary>
    public void ActiveFinishCanvas()
    {
        finishCanvas.SetActive(true);
    }

    /// <summary>
    /// フィニッシュ演出用のキャンバスを無効にする
    /// </summary>
    public void DeactiveFinishCanvas()
    {
        finishCanvas.SetActive(false);
    }

    /// <summary>
    /// リザルト画面を有効にする
    /// </summary>
    public void ActiveResult()
    {
        //SE
        SEManager.I.PlaySE(successSEVol, successSE);

        //カメラを変更する
        canvas.worldCamera = finishCam;

        var noDamageAddPoint = 0;
        var maxHPAddPoint = 0;

        //ノーダメージかどうか
        switch (isNodamage)
        {
            //ノーダメージなら設定した分加算する
            case true:
                noDamageAddPoint = this.noDamageAddPoint;
                break;

            case false:
                noDamageAddPoint = 0;
                break;
        }

        //最大HPまで走ったか
        switch (isMaxHP)
        {
            //走ったなら設定した分加算する
            case true:
                maxHPAddPoint = this.maxHPAddPoint;
                break;

            case false:
                maxHPAddPoint = 0;
                break;
        }

        //リザルトUIを表示
        uiManager.ActiveResultUI(temporarilyScore, noDamageAddPoint, maxHPAddPoint);

        //トータルスコアを算出する
        totalScore = temporarilyScore + maxHPAddPoint + noDamageAddPoint;

        //ゲームマネージャーにスコア加算
        AddPoint(totalScore);

        //ステージをクリア扱いにする
        StageSelectManager.I.AvtiveStageClearFlag(GameManager.I.SelectedStageInfo.StageNum);
    }

    //再度このシーンをやり直す（リトライ）
    public void ReTryThisScene()
    {
        //ゲーム時間を戻し、リトライ
        Time.timeScale = 1.0f;
        SceneChanger.I.Retry();
    }

    //タイトルへ
    public void ToTitleScene()
    {
        //ゲーム時間を戻し、タイトルへ
        Time.timeScale = 1.0f;
        SceneChanger.I.ToTitleScene();
    }

    //ポーズ画面でつづけるを押したとき、ゲームを続行させる
    public void ContinueGame()
    {
        Time.timeScale = 1.0f;
        uiManager.DeactivePoseUI();
    }

    //ステージセレクトへ
    public void ToStageSelectScene()
    {
        //ゲーム時間を戻し、ステージセレクトへ
        Time.timeScale = 1.0f;
        SceneChanger.I.ToStageSelectScene();
    }
}
