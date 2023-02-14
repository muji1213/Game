using System.Collections;
using UnityEngine;

//このスクリプトはステージシーンの管理をする
public class StageManager : SceneChanger
{
    [Header("メインカメラに表示させるキャンバス")] [SerializeField] Canvas canvas;
    [Header("フィニッシュカメラに映すキャンバス")] [SerializeField] GameObject finishCanvas;

    [Header("Player")] [SerializeField] Player_Controller player;

    [Header("フィニッシュ演出用のカメラ")] [SerializeField] Camera finishCam;
    [Header("ステージクリア後に表示するUI")] [SerializeField] GameObject resultUI;
    [Header("ポーズUI")] [SerializeField] GameObject poseUI;
    [Header("リトライUI")] [SerializeField] GameObject retryUI;
    [Header("ポイントUI")] [SerializeField] GameObject scoreUI;
    [Header("ライフUI")] [SerializeField] GameObject lifeUI;
    [Header("ゲージUI")] [SerializeField] GameObject gaugeUI;
    [Header("評価UI")] [SerializeField] GameObject evaluateUI;

    [Header("リザルトのスクリプト")] [SerializeField] ResultUI result;
    [Header("カウントダウンスクリプト")] [SerializeField] CountDown countDown;
    [Header("フェードインのスクリプト")] [SerializeField] Fade fadeIn;

    [Header("ターゲットのゲームオブジェクト")] [SerializeField] GameObject target;

    [Header("BGM")] [SerializeField] AudioClip bgm;
    [Header("ターゲットとの衝突時のSE")] [SerializeField] AudioClip colSE;
    [Header("ターゲットが吹っ飛ぶSE")] [SerializeField] AudioClip blowSE;
    [Header("リザルト表示時のSE")] [SerializeField] AudioClip successSE;
    [Header("ミスしたときのSE")] [SerializeField] AudioClip missedSE;
    [Header("ステージ入場カメラ")] [SerializeField] StageEnterCamera stageEnterCamera;

    [HideInInspector] public int temporarilypoint; //ステージで取得したポイントを保持、ステージクリア後、ゲームマネージャに加算する

    //リザルトの加点判定
    [HideInInspector] public bool isNodamage;
    [HideInInspector] public bool isMaxHP;

    //ステージのゴール位置
    [HideInInspector] public float goalPos;

    //ステージの死亡位置
    [HideInInspector] public float deadPos;

    //ポーズ中かどうか
    private bool isPose = false;

    //UIおよびプレイヤーの操作を有効にしたか
    private bool isStart = false;

    //クリアしたかどうか（これはゴールのゲームオブジェクト側からTrueにされる）
    [HideInInspector] public bool isCleared = false;

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
        temporarilypoint = 0;

        //リザルト判定の初期値を設定
        isNodamage = true;
        isMaxHP = false;

        //ゴール位置を記録
        goalPos = target.transform.position.z;

        //BGM
        BGMManager.bgmManager.PlayBgm(bgm);

        StopPlayerControll();
    }

    private void Update()
    {
        //カウントダウンが終わるまで操作無効
        if (!countDown.isCountdownEnd)
        {
            //フェードが終わったら
            if (fadeIn.isAnimEnd)
            {
                stageEnterCamera.StartMovie();

                if (stageEnterCamera.MovieFinished())
                {
                    cameraFollower.enabled = true;             
                }
                else
                {
                    return;
                }

                //カウントダウンスタート
                countDown.StartCountDown();
            }
            return;
        }
        else
        {
            //UIを配置する
            if (!isStart)
            {
                MovePlayer();
                scoreUI.SetActive(true);
                gaugeUI.SetActive(true);
                isStart = true;
            }
            //ゴールのゲームオブジェクトに衝突後、リザルトを呼び出す
            if (isCleared)
            {
                //リザルトUI呼び出し
                Invoke("ActiveResult", 2.0f);

                //クリアフラグをfalse
                isCleared = false;

                result.SetPoint();
            }
        }
    }

    /// <summary>
    /// UIを表示
    /// ハートの個数をセット
    /// </summary>
    public void SetUI(int frisbeeHP)
    {
        //
        evaluateUI.SetActive(true);
        evaluateUI.GetComponent<EvaluateUI>().Evaluate(frisbeeHP);

        //
        lifeUI.SetActive(true);
        lifeUI.GetComponent<Life>().SetInitialLife(frisbeeHP);
    }

    //ステージで取得したポイントを一時的に保持する
    //途中でリタイアした際、ポイントを加算できないようにする
    public void StorePoint(int point)
    {
        temporarilypoint += point;
    }

    //ステージクリア後、ゲームマネージャにポイントを加算
    public void AddPoint()
    {
        GameManager.gameManager.point += temporarilypoint;

        //一時ポイントを0に
        temporarilypoint = 0;
    }

    /// <summary>
    /// フリスビーが障害物に衝突時、ライフを減らす
    /// </summary>
    public void ReduceHPUI()
    {
        lifeUI.GetComponent<Life>().DestroyLife();

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
        SEManager.seManager.PlaySe(colSE);

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
        SEManager.seManager.PlaySe(blowSE);

        //フィニッシュキャンバスをOFFに
        DeactiveFinishCanvas();
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
            poseUI.SetActive(true);

            //ゲーム時間を停止
            Time.timeScale = 0.0f;
        }
        else
        {
            //ポーズ中なら
            isPose = false;

            //ポーズUIを非表示
            poseUI.SetActive(false);

            //ゲーム時間を戻す
            Time.timeScale = 1.0f;
        }
    }

    /// <summary>
    /// プレイヤーもしくはフリスビーが死亡した際に呼び出す
    /// </summary>
    public void Retry()
    {
        SEManager.seManager.PlaySe(missedSE);
        var value = Mathf.Clamp((float)deadPos / (float)goalPos, 0, 1);
        retryUI.SetActive(true);
        retryUI.GetComponent<RetryUI>().SetClearPer(value);
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
        Debug.Log("操作を有効か");
        player.enabled = true;
    }

    /// <summary>
    /// プレイヤーの操作を無効にする
    /// </summary>
    public void StopPlayerControll()
    {
        Debug.Log("操作を無効か");
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
        SEManager.seManager.PlaySe(successSE);

        //カメラを変更する
        canvas.worldCamera = finishCam;

        //リザルトUIを表示
        resultUI.SetActive(true);

        //ステージをクリア扱いにする
        StageSelectManager.stageSelectManager.AvtiveStageClearFlag(StageSelectManager.selectedStageNum);
    }
}
