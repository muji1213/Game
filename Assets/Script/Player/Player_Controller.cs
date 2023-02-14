using System.Collections;
using UnityEngine;


//このスクリプトはフリスビーを投げる前のプレイヤーのスクリプトです
public class Player_Controller : MonoBehaviour
{
    //変数
    [Header("奥移動速度")] [SerializeField] float zSpeed;
    [Header("横移動速度")] [SerializeField] float xSpeed;
    [Header("ジャンプ高度")] [SerializeField] float jumpSpeed;
    [Header("重力")] [SerializeField] float gravity;
    [Header("フリスビー")] [SerializeField] GameObject frisbee;
    [Header("プレイヤーの加速度")] [SerializeField] AnimationCurve runCurve;
    [Header("接地判定チェック")] [SerializeField] GroundChecker groundChecker;
    [Header("ダメージ受けた時のSE")] [SerializeField] AudioClip damageSE;
    [Header("ダメージ受けた時のエフェクト")] ParticleSystem damageEffect;
    [Header("コイン取得時のエフェクト")] [SerializeField] ParticleSystem coinEffect;
    [Header("コイン取得時のSE")] [SerializeField] AudioClip coinSE;
    [Header("走るSE")] [SerializeField] AudioClip runSE;
    [Header("ライフアップSE")] [SerializeField] AudioClip lifeUP;
    [Header("ライフアップエフェクト")] [SerializeField] ParticleSystem lifeUPEffect;
    [Header("どれくらい走れば最大HPになるか")] [SerializeField] public int MaxHPTime;

    //走った時間を計測
    //走った時間が長いほどフリスビーのライフが増える
    //これはStageManagerで参照される
    [HideInInspector] public float runTime = 0.0f;

    //各種キー
    private bool rightKey; //右移動
    private bool leftKey; //左移動
    private bool jumpKey; //ジャンプ
    private bool shootKey; //フリスビーを投げる
    private bool poseKey; //ポーズ

    //各種判定
    private bool isGround = false; //接地判定
    private bool isRun = false; //走っているかどうか
    private bool isJump = false; //ジャンプ中かどうか
    private bool isShoot = false; //フリスビーを投げたかどうか
    private bool isLifeUP = false;

    //インスタンス
    private StageManager stageManager; // ステージマネージャー
    private Rigidbody rb;  //リジットボディ
    private Player_HurtBox player_HurtBox; //当たり判定
    private Animator anim; //アニメーター
    private AudioSource audioSource;

    void Start()
    {
        //コンポーネント取得
        audioSource = GetComponent<AudioSource>();

        //ステージマネージャー
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();

        //リジットボディ
        rb = GetComponent<Rigidbody>();

        //当たり判定取得スクリプト
        player_HurtBox = GetComponent<Player_HurtBox>();

        //アニメーター
        anim = GetComponent<Animator>();
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

        Jump();

        //Fキーでフリスビーを投げる
        if (shootKey)
        {
            Shoot();
        }

        //Pキーでポーズ
        //ステージマネージャーのポーズメソッドを呼び出し、一時停止させる
        if (poseKey)
        {
            stageManager.Pose();
        }
    }

    private void FixedUpdate()
    {
        if (!audioSource.enabled)
        {
            audioSource.enabled = true;
        }
        audioSource.pitch = zSpeed / 3;

        //接地判定をGroundCheckerのメソッドから取得
        isGround = groundChecker.CheckGround();

        //下方向に重力をかける
        Gravity();

        //アニメーションの遷移をする
        SetAnimation();

        if (!player_HurtBox.isDeadCheck())
        {
            //フリスビーを投げていなければ移動を続ける
            if (!isShoot)
            {
                Move();
            }
            else
            {
                return;
            }
        }
    }

    /// <summary>
    /// 移動メソッド
    /// 左右キーで横移動
    /// 前方へは自動で進み続ける
    /// 前方への移動速度はアニメーションカーブで設定。
    /// 走った時間もここで計測する
    /// </summary>
    private void Move()
    {
        //走った時間を計測
        runTime += Time.deltaTime;

        //走った時間に応じてアニメーションカーブの値を奥移動速度に適用する（走った時間が長いほど速度が上がる）
        zSpeed = runCurve.Evaluate(runTime);

        //接地しているときにしか横移動できない
        if (isGround)
        {
            isRun = true;

            //右キーで右へ
            if (rightKey)
            {
                rb.velocity = new Vector3(xSpeed, rb.velocity.y, zSpeed);
            }

            //左キーで左へ
            else if (leftKey)
            {
                rb.velocity = new Vector3(-xSpeed, rb.velocity.y, zSpeed);
            }
            //入力無しは左右移動しない
            else
            {
                rb.velocity = new Vector3(0, rb.velocity.y, zSpeed);
            }
        }
    }

    /// <summary>
    /// ジャンプメソッド
    /// Spaceキーでジャンプ
    /// ジャンプ中はフリスビーが投げられない
    /// </summary>
    private void Jump()
    {
        //接地しているかつジャンプキーが押された
        if (jumpKey && isGround)
        {
            //ジャンプフラグをTrueに
            isJump = true;

            //上方向へ力を加える
            rb.AddForce(transform.up * jumpSpeed);
        }

        //ジャンプフラグがTrueかつ速度が一定以下になったらジャンプをfalseに
        if (rb.velocity.y < -5.0f && isJump)
        {
            isJump = false;
        }
    }

    /// <summary>
    /// フリスビーを投げるメソッド
    /// ジャンプ中は投げられない
    /// </summary>
    private void Shoot()
    {
        //接地中でないとき投げられない
        if (!isGround)
        {
            return;
        }

        int frisbeeHP = CaluculateFrisbeeHP((int)runTime);

        //UIを表示
        stageManager.SetUI(frisbeeHP);

        Debug.Log(frisbeeHP);

        //フリスビーを生成
        GameObject threwfrisbee = Instantiate(StageSelectManager.selectedFrisbee.SelectedFrisbee, new Vector3(transform.position.x, this.transform.position.y, this.transform.position.z + 2), Quaternion.Euler(new Vector3(90, 90, 0)));
        threwfrisbee.GetComponent<Frisbee>().SetHP(frisbeeHP);

        //フリスビーの投げたフラグをTrueに
        isShoot = true;

        //投げた時点でプレイヤーを停止させる
        rb.velocity = Vector3.zero;

        //投げた後はステージマネージャーのメソッドでプレイヤーの操作を無効にし、フリスビーだけ操作できるようにする
        stageManager.StopPlayerControll();

        //
        this.gameObject.SetActive(false);
    }


    public void TheDie(int type, Vector3 direction)
    {
        StartCoroutine(Die(type, direction));
    }

    /// <summary>
    /// プレイヤーが障害物、もしくは穴に落ちた際に呼ぶメソッド
    /// </summary>
    /// <returns></returns>
    private IEnumerator Die(int type, Vector3 direciton)
    {
        //死亡位置を記録
        stageManager.deadPos = this.transform.position.z;

        //アニメーション再生
        PlayDieAnim(type);

        switch (type)
        {
            //穴に落下した場合
            case 0:

                break;

            //障害物に衝突した場合
            case 1:
                rb.velocity = Vector3.zero;
                rb.useGravity = true;
                rb.AddForce(direciton, ForceMode.Impulse);
                SEManager.seManager.PlaySe(damageSE);
                break;
        }


        //ステージマネージャーのメソッドでリトライUIを呼び出す
        stageManager.Retry();

        //ステージマネージャーのメソッドでプレイヤーの操作を無効にする
        stageManager.StopPlayerControll();

        //ミスした後は一定時間後にゲーム時間を止める（リトライUIの背景が騒がしくなるのを防ぐ）
        yield return new WaitForSeconds(1.0f);

        Time.timeScale = 0.0f;
    }

    /// <summary>
    /// 重力メソッド
    /// 常に下方向に重量をかける
    /// ステージによって変更できるようにする
    /// </summary>
    private void Gravity()
    {
        rb.AddForce(new Vector3(0, -gravity, 0), ForceMode.Acceleration);
    }

    /// <summary>
    /// アニメーション遷移用のメソッド
    /// 各種boolで遷移させている
    /// </summary>
    private void SetAnimation()
    {
        //走っているとき
        anim.SetBool("Run", isRun);

        //接地しているとき
        anim.SetBool("Ground", isGround);

        //ジャンプしているとき
        anim.SetBool("Jump", isJump);

        //走った時間が長いほど、アニメーションが加速する
        anim.SetFloat("Speed", zSpeed / 6);
    }

    /// <summary>
    /// ミスした際のアニメーション
    /// 穴に落ちた際、障害物に衝突した際の２つ
    /// これはプレイヤーの当たり判定を取得するスクリプトから呼び出す
    /// </summary>
    /// <param name="type"></param>
    public void PlayDieAnim(int type)
    {
        switch (type)
        {
            //落下した際
            case 0:
                anim.Play("Unity_Chan_Die1");
                break;

            //障害物にあたった際
            case 1:
                anim.Play("Unity_Chan_Die2");
                break;
        }
    }

    //コイン取得時のエフェクト
    //エフェクトはインスペクタから設定
    public void PlayCoinEffect()
    {
        SEManager.seManager.PlaySe(coinSE);
        coinEffect.Play();
    }

    /// <summary>
    /// ライフアップ取得
    /// フリスビーのHPが1個増える
    /// </summary>

    public void LifeUP()
    {
        //SE
        SEManager.seManager.PlaySe(lifeUP);
        isLifeUP = true;

        //エフェクト
        lifeUPEffect.Play();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="runTime"></param>
    /// <returns></returns>

    private int CaluculateFrisbeeHP(int runTime)
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
