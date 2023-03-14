using System.Collections;
using UnityEngine;


//このスクリプトはフリスビーを投げる前のプレイヤーのスクリプトです
public class Player_Controller : MonoBehaviour, IMovable, IDienable<Player_HurtBox.DeadType>, PlayerUnit
{
    public enum State
    {
        Idle,
        Run,
        Jump,
        Shoot,
        Dead
    }

    //変数
    [Header("パラメータ")]
    [Header("奥移動速度")] [SerializeField] float zMoveSpeed;
    [Header("横移動速度")] [SerializeField] float xMoveSpeed;
    [Header("ジャンプ高度")] [SerializeField] float jumpSpeed;
    [Header("重力")] [SerializeField] float gravity;
    [Header("死亡時の吹っ飛びの強さ")] [SerializeField] float dieBlowPow;
    [Header("フリスビー")] [SerializeField] GameObject frisbee;
    [Header("プレイヤーの加速度")] [SerializeField] AnimationCurve runCurve;
    [Header("どれくらい走れば最大HPになるか")] [SerializeField] public int MaxHPTime;
    [Header("接地判定チェック")] [SerializeField] GroundChecker groundChecker;

    [Header("エフェクト")]
    [Header("ジャンプのSE")] [SerializeField] AudioClip jumpSE;
    [Header("ジャンプSEの音量")] [SerializeField] [Range(0, 1)] float jumpSEVol = 1; 
    [Header("ダメージ受けた時のSE")] [SerializeField] AudioClip damageSE;
    [Header("ダメージSEの音量")] [SerializeField] [Range(0, 1)] float damageSEVol = 1;
    [Header("コイン取得時のエフェクト")] [SerializeField] ParticleSystem coinEffect;
    [Header("コイン取得時のSE")] [SerializeField] AudioClip coinSE;
    [Header("コインSEの音量")] [SerializeField] [Range(0, 1)] float coinSEVol = 1;
    [Header("ライフアップSE")] [SerializeField] AudioClip lifeUP;
    [Header("ライフアップSEの音量")] [SerializeField] [Range(0, 1)] float lifeUPSEVol = 1;
    [Header("ライフアップエフェクト")] [SerializeField] ParticleSystem lifeUPEffect;
   
    //X速度
    private float xSpeed;

    //走った時間を計測
    //走った時間が長いほどフリスビーのライフが増える
    //これはStageManagerで参照される
    [HideInInspector] public float runTime = 0.0f;

    //障害物に衝突時吹っ飛ぶ方向
    Vector3 dieBlowDirection;

    //ステート
    State currentState;

    //各種キー
    private bool rightKey; //右移動
    private bool leftKey; //左移動
    private bool jumpKey; //ジャンプ
    private bool shootKey; //フリスビーを投げる
    private bool poseKey; //ポーズ

    //各種判定
    private bool isGround = false; //接地判定
    private bool isLifeUP = false;

    //インスタンス
    private StageManager stageManager; // ステージマネージャー
    private CameraFollower cameraFollower; //カメラフォロワー
    private Rigidbody rb;  //リジットボディ
    private Animator anim; //アニメーター

    void Start()
    {
        //コンポーネント取得
        //カメラ追従用のスクリプト
        cameraFollower = GameObject.Find("Main Camera").GetComponent<CameraFollower>();

        //ステージマネージャー
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();

        //リジットボディ
        rb = GetComponent<Rigidbody>();

        //アニメーター
        anim = GetComponent<Animator>();

        currentState = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        //キー入力
        rightKey = Input.GetKey(KeyCode.RightArrow);
        leftKey = Input.GetKey(KeyCode.LeftArrow);
        jumpKey = Input.GetKeyDown(KeyCode.Space);
        shootKey = Input.GetKeyDown(KeyCode.F);
        poseKey = Input.GetKeyDown(KeyCode.P);

        switch (currentState)
        {
            case State.Dead:
                break;

            //初期状態のとき、Runに遷移する
            case State.Idle:
                currentState = State.Run;
                break;

            //Run状態の時
            case State.Run:
                Move();

                //ジャンプキーが押されたらジャンプ
                if (jumpKey)
                {
                    Jump();
                }
                //シュートキーが押されたらシュート
                else if (shootKey)
                {
                    Shoot();
                }
                break;

            //ジャンプ中は落下メソッド
            case State.Jump:
                JumpDown();
                break;

            case State.Shoot:
                break;
        }

        //Pキーでポーズ
        //ステージマネージャーのポーズメソッドを呼び出し、一時停止させる
        if (poseKey)
        {
            stageManager.Pose();
        }

        //走った時間を計測
        runTime += Time.deltaTime;

        //走った時間に応じてアニメーションカーブの値を奥移動速度に適用する（走った時間が長いほど速度が上がる）
        zMoveSpeed = runCurve.Evaluate(runTime);

        rb.velocity = new Vector3(xSpeed, rb.velocity.y, zMoveSpeed);
    }

    private void FixedUpdate()
    {
        //接地判定をGroundCheckerのメソッドから取得
        isGround = groundChecker.CheckGround();

        //下方向に重力をかける
        Gravity();
    }

    /// <summary>
    /// 移動メソッド
    /// 左右キーで横移動
    /// 前方へは自動で進み続ける
    /// 前方への移動速度はアニメーションカーブで設定。
    /// 走った時間もここで計測する
    /// </summary>
    public void Move()
    {
        //ステートを変更
        currentState = State.Run;

        //アニメーターのブールを変更
        SetAnimation(true);

        if (isGround)
        {
            //右キーで右へ
            if (rightKey)
            {
                xSpeed = xMoveSpeed;
            }

            //左キーで左へ
            else if (leftKey)
            {
                xSpeed = -xMoveSpeed;
            }
            //入力無しは左右移動しない
            else
            {
                xSpeed = 0.0f;
            }
        }
    }

    /// <summary>
    /// ジャンプメソッド
    /// Spaceキーでジャンプ
    /// ジャンプ中はフリスビーが投げられない
    /// </summary>
    public void Jump()
    {
        //接地しているかつジャンプキーが押された
        if (isGround)
        {
            //SE
            SEManager.I.PlaySE(jumpSEVol, jumpSE);

            //ステートを変更
            currentState = State.Jump;

            //アニメーターのブールを変更
            SetAnimation(true);

            //上方向へ力を加える
            rb.AddForce(transform.up * jumpSpeed);
        }
    }

    public void JumpDown()
    {
        if (!isGround)
        {
            //ジャンプフラグがTrueかつ速度が一定以下になったらジャンプをfalseに
            if (rb.velocity.y < -1.0f && currentState == State.Jump)
            {
                Debug.Log("ジャンプ解除");

                //アニメーターのブールを変更
                SetAnimation(false);

                //ステートを変更
                currentState = State.Run;
            }
        }
    }

    /// <summary>
    /// フリスビーを投げるメソッド
    /// ジャンプ中は投げられない
    /// </summary>
    public void Shoot()
    {
        //接地中でないとき投げられない
        if (!isGround)
        {
            return;
        }

        //ステートを変更
        currentState = State.Shoot;

        int frisbeeHP = CaluculateFrisbeeHP((int)runTime);

        //UIを表示
        stageManager.SetUIAndLife(frisbeeHP, isLifeUP);

        //フリスビーを生成
        GameObject threwfrisbee = Instantiate(GameManager.I.SelectedFrisbeeInfo.SelectedFrisbee, new Vector3(transform.position.x, this.transform.position.y, this.transform.position.z + 2), Quaternion.Euler(new Vector3(90, 90, 0)));
        threwfrisbee.GetComponent<Frisbee>().SetHP(frisbeeHP);

        //投げた時点でプレイヤーを停止させる
        rb.velocity = Vector3.zero;

        //投げた後はステージマネージャーのメソッドでプレイヤーの操作を無効にし、フリスビーだけ操作できるようにする
        stageManager.StopPlayerControll();

        //投げた瞬間Unityちゃんは非表示に
        this.gameObject.SetActive(false);
    }


    public void TheDie(Player_HurtBox.DeadType type, Vector3 direction)
    {
        //吹っ飛ばす方向
        dieBlowDirection = direction;

        //ステート変更
        currentState = State.Dead;

        //コルーチン呼び出し
        StartCoroutine(Die(type));
    }

    /// <summary>
    /// プレイヤーが障害物、もしくは穴に落ちた際に呼ぶメソッド
    /// </summary>
    /// <returns></returns>
    public IEnumerator Die(Player_HurtBox.DeadType type)
    {
        //死亡位置を記録
        var deadPos = this.transform.position.z;

        switch (type)
        {
            //穴に落下した場合
            case Player_HurtBox.DeadType.Fall:

                //カメラを真上から下方向に投射し、穴に落ちているところが見えるようにする
                cameraFollower.ZoomToTarget(-5, 0, new Vector3(0, -10, 0));

                anim.Play("Unity_Chan_Die1");
                break;

            //障害物に衝突した場合
            case Player_HurtBox.DeadType.Collide:
                //振動させる
                cameraFollower.Shake(0.3f, 1.0f);

                anim.Play("Unity_Chan_Die2");

                //速度をリセット
                rb.velocity = Vector3.zero;

                //重力を有効にする
                rb.useGravity = true;

                //障害物から自分にかけてのベクトル(direction)に力を加える
                rb.AddForce(dieBlowDirection * dieBlowPow, ForceMode.Impulse);

                //SE
                SEManager.I.PlaySE(damageSEVol, damageSE);
                break;
        }


        //ステージマネージャーのメソッドでリトライUIを呼び出す
        stageManager.Retry(deadPos);

        //ミスした後は一定時間後にゲーム時間を止める（リトライUIの背景が騒がしくなるのを防ぐ）
        yield return new WaitForSeconds(1.0f);

        Time.timeScale = 0.0f;
    }

    /// <summary>
    /// 重力メソッド
    /// 常に下方向に重量をかける
    /// ステージによって変更できるようにする
    /// </summary>
    public void Gravity()
    {
        rb.AddForce(new Vector3(0, -gravity, 0), ForceMode.Acceleration);
    }

    /// <summary>
    /// アニメーション遷移用のメソッド
    /// currentStateがRunの状態で呼び出しをSetAnimation(true)にすると、アニメーターのパラメータRunが
    /// trueになる
    /// </summary>
    public void SetAnimation(bool judge)
    {
        switch (currentState)
        {
            case State.Run:
                anim.SetBool("Run", judge);
                break;

            case State.Jump:
                anim.SetBool("Jump", judge);
                break;
        }

        //接地しているとき
        anim.SetBool("Ground", isGround);

        //走った時間が長いほど、アニメーションが加速する
        anim.SetFloat("Speed", zMoveSpeed / 6);
    }

    //コイン取得時のエフェクト
    //ステージマネージャーのスコアを加算する
    //エフェクトはインスペクタから設定
    public void GetCoin(int score)
    {
        //SE
        SEManager.I.PlaySE(coinSEVol, coinSE);

        //ポイント加算
        stageManager.StorePoint(score);

        //エフェクト
        coinEffect.Play();
    }

    /// <summary>
    /// ライフアップ取得
    /// フリスビーのHPが1個増える
    /// </summary>

    public void LifeUP()
    {
        //SE
        SEManager.I.PlaySE(lifeUPSEVol, lifeUP);
        isLifeUP = true;

        //エフェクト
        lifeUPEffect.Play();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="runTime"></param>
    /// <returns></returns>

    public int CaluculateFrisbeeHP(int runTime)
    {
        //ライフアップを取得したなら１追加
        var extraHP = 0;

        if (isLifeUP)
        {
            extraHP = 1;
        }

        //走った時間がMaxHPTimeの一定値以下だった場合で条件分け
        if (runTime >= MaxHPTime)
        {
            return 3 + extraHP;
        }
        else if (runTime >= MaxHPTime * 2 / 3)
        {
            return 2 + extraHP;
        }
        else
        {
            return 1 + extraHP;
        }
    }
}
