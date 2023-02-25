using UnityEngine;
using System.Collections;

public abstract class Frisbee : MonoBehaviour
{
    //フリスビーの移動状態
    //例えば上キーを押した場合UPになる
    public enum State
    {
        UP,
        DOWN,
        RIGHT,
        LEFT,
        RIGHTUP,
        LEFTUP,
        RIGHTDOWN,
        LEFTDOWN
    }

    [SerializeField] float speed; //縦横移動速度
    [SerializeField] float zSpeed; //奥移動速度
    [SerializeField] float maxZspeed; //奥移動最大速度
    [SerializeField] float minZspeed; //奥移動最低速度
    [SerializeField] float rotateSpeed; //回転速度
    [SerializeField] float gravity; //重力
    [SerializeField] float invincibleTime; //無敵時間
    [SerializeField] float blinkInterval; //無敵時間中の点滅間隔
    [SerializeField] AnimationCurve speedCurve; //方向転換した際の加速度
    [SerializeField] AnimationCurve acceleteCurve; //加速ボタンを押した際のフリスビーの速度
    [Header("ダメージ受けた時のSE")] [SerializeField] AudioClip damageSE;
    [Header("ダメージSEの音量")] [SerializeField] [Range(0, 1)] float damageSEVol = 1;
    [Header("ダメージ受けた時のエフェクト")] [SerializeField] GameObject damageEffect;
    [Header("コイン取得時のエフェクト")] [SerializeField] ParticleSystem coinEffect;
    [Header("コイン取得時のSE")] [SerializeField] AudioClip coinSE;
    [Header("コイン取得SEの音量")] [SerializeField] [Range(0, 1)] float coinSEVol = 1;
    [Header("投げた時のSE")] [SerializeField] AudioClip throwSE;
    [Header("投げた時のSEの音量")][SerializeField] [Range(0, 1)] float throwSEVol = 1;
    [Header("効果音用のオーディオソース")] [SerializeField] AudioSource seAudioSource;

    private int HP; //HP
    private float moveTime; //停止、もしくは方向転換してからの経過時間
    private float moveSpeed; //移動速度

    private float invincibleTimer; //無敵経過時間
    private float blinkTimer; //無敵時間点滅のタイマー
    private float acceleteTime; //加速ボタンを押している時間
    private float throwSpeed;　//runステージからスタート地点につくまでの速さ
    private Vector3 toVector; //スタート地点までのベクトル
    private GameObject StartPos; //スタート地点
    private ParticleSystem speedEff; //集中線のエフェクト

    //キー入力
    private bool upKey; //上移動
    private bool downKey; //横移動
    private bool leftKey; //左移動
    private bool rightKey; //右移動
    private bool rotateRKey; //右回転
    private bool rotateLKey; //左回転
    private bool poseKey; //ポーズ
    private bool acceleteKey; //加速
    private bool dodgeKey; //回避

    //判定
    private bool isStart = false; //フリスビーが開始位置についたかどうか(StartPosについたらtrue)
    private bool isDead = false; //死亡判定
    protected bool isInvincible = false; //無敵かどうか(障害物に衝突後無敵時間を付ける)
    private bool isRetryed = false; //リザルトUIが複数回呼び出されるのを防ぐ

    private State prevState = State.RIGHT; //前の状態
    private State currentState; //現在状態

    //状態異常
    //風の影響を受けるか
    public bool isBlowed;

    //インスタンス
    protected Rigidbody rb; //リジットボディ
    private CameraFollower cameraFollower; //カメラを追従させるスクリプト
    private Frisbee_HurtBox frisbee_Hurt; //当たり判定
    private Renderer render; //レンダラー
    private StageManager stageManager; //ステージマネージャー(ポーズの呼び出し、リトライUIの呼びだしに必要)


    void Start()
    {
        //コンポーネント取得

        //ステージマネージャー
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();

        //リジットボディ
        rb = GetComponent<Rigidbody>();

        //レンダー
        render = GetComponent<Renderer>();

        //当たり判定
        frisbee_Hurt = GetComponent<Frisbee_HurtBox>();

        //フリスビーが移動中に画面端に集中線を表示させ、スピード感を出す
        speedEff = GameObject.Find("Canvas/SpeedEffect").GetComponent<ParticleSystem>();
        speedEff.Play();

        //開始位置を取得する
        //フリスビーがこの位置についたら操作が可能になる
        StartPos = GameObject.Find("StartPos");

        //最小z移動速度
        minZspeed = zSpeed;

        //カメラ追従用のスクリプト
        cameraFollower = GameObject.Find("Main Camera").GetComponent<CameraFollower>();

        //カメラの追従対象をUnityちゃんからフリスビーに変える
        cameraFollower.ChangeTarget(this.gameObject);

        //カメラの位置調整
        cameraFollower.ZoomToTarget(-5, 0, new Vector3(0, 0, 0));

        //スタート地点につくまでの速さを計算
        throwSpeed = (StartPos.transform.position.z - this.transform.position.z) / 1.5f;

        //SE
        SEManager.seManager.PlaySE(throwSEVol,throwSE);
    }


    void Update()
    {
        //キー入力
        upKey = Input.GetKey(KeyCode.UpArrow);
        downKey = Input.GetKey(KeyCode.DownArrow);
        leftKey = Input.GetKey(KeyCode.LeftArrow);
        rightKey = Input.GetKey(KeyCode.RightArrow);
        rotateRKey = Input.GetKey(KeyCode.R);
        rotateLKey = Input.GetKey(KeyCode.W);
        poseKey = Input.GetKeyDown(KeyCode.P);
        acceleteKey = Input.GetKey(KeyCode.F);
        dodgeKey = Input.GetKeyDown(KeyCode.A);

        if (dodgeKey)
        {
            Dodge();
        }


        //Pキー入力でポーズ
        //ステージマネージャーのメソッドでUIと一時停止をする
        if (poseKey)
        {
            stageManager.Pose();
        }
    }

    protected void FixedUpdate()
    {

        //死亡時
        if (isDead)
        {
            Gravity();
            return;
        }

        //Runステージからスタート地点までフリスビーを飛ばす
        if (Vector3.Distance(this.gameObject.transform.position, StartPos.transform.position) > 0.1f && !isStart)
        {
            //スタート地点までのベクトルを生成
            toVector = Vector3.MoveTowards(transform.position, StartPos.transform.position, throwSpeed * Time.deltaTime);

            //生成ベクトルをもとに飛ばす
            rb.MovePosition(toVector);
        }
        else
        {
            //開始位置に一定距離まで近づいたらスタートフラグをtrue
            isStart = true;

            //カメラをフリスビーの真後ろに固定する
            //固定することでカメラの描画範囲外に移動できないようにする
            cameraFollower.FixCamera(StartPos);
        }

        if (isStart)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, zSpeed);

            //死亡判定を取得
            isDead = frisbee_Hurt.isDeadCheck();

            //各種メソッド
            Move();
            Rotate();
            Clamp();
            Accelerate();


            //無敵中
            if (isInvincible)
            {
                if (invincibleTimer < invincibleTime)
                {
                    //点滅時間タイマー
                    blinkTimer += Time.deltaTime;

                    //無敵中点滅させる
                    //インターバルを超えたら
                    if (blinkTimer > blinkInterval)
                    {
                        //表示非表示を反転
                        render.enabled = !render.enabled;

                        //タイマーリセット
                        blinkTimer = 0.0f;
                    }

                    //レイヤーをほかのレイヤーにすることで、障害物との衝突を回避させる
                    this.gameObject.layer = 10;

                    //無敵時間計測
                    invincibleTimer += Time.deltaTime;
                }
                else
                {
                    //無敵終了後、レイヤーを戻す
                    this.gameObject.layer = 9;

                    //タイマーリセット
                    invincibleTimer = 0.0f;

                    //色を戻す
                    render.enabled = true;

                    //フラグを戻す
                    isInvincible = false;
                }
            }
        }
    }

    /// <summary>
    /// 移動用メソッド
    /// </summary>
    public void Move()
    {
        moveSpeed = speed;
        if (upKey && rightKey)
        {
            currentState = State.RIGHTUP;
        }
        else if (upKey && leftKey)
        {
            currentState = State.LEFTUP;
        }
        else if (downKey && rightKey)
        {
            currentState = State.RIGHTDOWN;
        }
        else if (downKey & leftKey)
        {
            currentState = State.LEFTDOWN;
        }
        else if (upKey)
        {
            currentState = State.UP;
        }
        else if (downKey)
        {
            currentState = State.DOWN;
        }
        else if (leftKey)
        {
            currentState = State.LEFT;
        }
        else if (rightKey)
        {
            currentState = State.RIGHT;
        }
        else
        {
            //何も入力していない場合、0
            moveSpeed = 0.0f;
            moveTime = 0.0f;
        }

        //前回の状態と違ったら速度をリセットする
        if (prevState != currentState)
        {
            moveTime = 0f;
        }
        else
        {
            //同じボタンを押し続けていれば、moveTimeが加算される
            moveTime += Time.deltaTime;
        }

        //アニメーションカーブに基づいて加速させる
        //moveTimeが多くなるほど、速度が上がるようにアニメーションカーブを設定
        moveSpeed *= speedCurve.Evaluate(moveTime);

        //各方向移動
        switch (currentState)
        {
            case State.UP:
                rb.velocity = new Vector3(0, moveSpeed, rb.velocity.z);
                break;
            case State.DOWN:
                rb.velocity = new Vector3(0, -moveSpeed, rb.velocity.z);
                break;
            case State.LEFT:
                rb.velocity = new Vector3(-moveSpeed, 0, rb.velocity.z);
                break;
            case State.RIGHT:
                rb.velocity = new Vector3(moveSpeed, 0, rb.velocity.z);
                break;
            case State.RIGHTUP:
                rb.velocity = new Vector3(moveSpeed, moveSpeed, rb.velocity.z);
                break;
            case State.LEFTUP:
                rb.velocity = new Vector3(-moveSpeed, moveSpeed, rb.velocity.z);
                break;
            case State.RIGHTDOWN:
                rb.velocity = new Vector3(moveSpeed, -moveSpeed, rb.velocity.z);
                break;
            case State.LEFTDOWN:
                rb.velocity = new Vector3(-moveSpeed, -moveSpeed, rb.velocity.z);
                break;
        }

        //前回の状態を記録
        prevState = currentState;
    }

    /// <summary>
    /// 回転させる
    /// </summary>
    public void Rotate()
    {
        //Rキーで右に回転
        if (rotateRKey)
        {
            this.transform.Rotate(rotateSpeed * Time.deltaTime, 0, 0);
        }

        //Lキーで左回転
        if (rotateLKey)
        {
            this.transform.Rotate(-rotateSpeed * Time.deltaTime, 0, 0);
        }
    }

    /// <summary>
    /// 加速メソッド
    /// Fキー押下中速度が上昇（ただし一定速度以上にならない）
    /// </summary>
    public void Accelerate()
    {
        //加速キーを押す
        if (acceleteKey)
        {
            //加速時間に加算していく
            acceleteTime += Time.deltaTime;

            //アニメーションカーブは0から始まり右肩上がりにし、acceleteTimeが増加するほど、zSpeedが上がるようにする
            //アニメーションカーブを適用するのではなく、加算すること
            zSpeed += acceleteCurve.Evaluate(acceleteTime);
        }
        else
        {
            //キーを離した場合、加速時間が徐々に減衰していく
            acceleteTime -= Time.deltaTime;

            //ただし0以下にならない
            if (acceleteTime < 0)
            {
                acceleteTime = 0.0f;
            }

            //zspeedから減算する
            //価の高い方から減算していくため、急速に速度が落ちるようにしている
            zSpeed -= acceleteCurve.Evaluate(acceleteTime);
        }

        //zSpeedがmaxZspeedを上回らないようにする
        if (zSpeed > maxZspeed)
        {
            zSpeed = maxZspeed;
        }

        //zSpeedがminZspeedを下回らないようにする
        else if (zSpeed < minZspeed)
        {
            zSpeed = minZspeed;
        }
    }

    public void TheDie(int type)
    {
        StartCoroutine(Die(type));
    }

    /// <summary>
    /// 死亡時に呼び出す
    /// </summary>
    private IEnumerator Die(int type)
    {
        if (isRetryed)
        {
            yield break;
        }


        //死亡位置を記録
        stageManager.deadPos = this.transform.position.z;

        //集中線を消す
        speedEff.Stop();

        //カメラを止める
        cameraFollower.StopCamera();

        switch (type)
        {
            case 0:
                Time.timeScale = 0.0f;

                yield return new WaitForSecondsRealtime(0.5f);

                Time.timeScale = 1.0f;

                break;

            case 1:
                break;
        }

        //下方向に落とし、フェードアウトさせる
        rb.AddForce(new Vector3(0, -0.5f, -0.5f), ForceMode.Impulse);

        //リトライUIを表示
        stageManager.Retry();

        isRetryed = true;

        yield return new WaitForSeconds(2);

        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// 移動範囲をカメラ範囲内に収める
    /// </summary>
    public void Clamp()
    {
        Vector3 pos = transform.position;

        float distance = pos.z - Camera.main.transform.position.z;
        // 画面左下のワールド座標をビューポートから取得
        Vector3 min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));

        // 画面右上のワールド座標をビューポートから取得
        Vector3 max = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, distance));

        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        transform.position = pos;
    }

    /// <summary>
    // フリスビーがライフ０になったときに呼び出す
    //  重力を下に強くかけることで、画面からフェードアウトさせる
    //　それ以外の時は重力をかけない
    /// </summary>
    public void Gravity()
    {
        if (isDead)
        {
            gravity = 20;
        }
        else
        {
            gravity = 0;
        }
        rb.AddForce(new Vector3(0, -gravity, 0), ForceMode.Acceleration);
    }

    /// <summary>
    /// //ライフを減らす
    /// </summary>
    public void ReduceLife()
    {
        SEManager.seManager.PlaySE(damageSEVol, damageSE);
        HP -= 1;
        stageManager.ReduceHPUI();
    }

    /// <summary>
    /// 無敵メソッド
    /// </summary>
    /// <param name="time">無敵時間</param>
    public void Invincible(int time)
    {
        //無敵時間を引数の時間にする
        invincibleTime = time;

        //無敵フラグをtrueに
        isInvincible = true;
    }

    //コイン取得時のエフェクト
    //エフェクトはインスペクタから設定
    public void PlayCoinEffect()
    {
        SEManager.seManager.PlaySE(coinSEVol, coinSE);
        coinEffect.Play();
    }

    public void PlayDamageEffect(Vector3 pos)
    {
        GameObject damageEffect = Instantiate(this.damageEffect, pos, Quaternion.identity);
        Destroy(damageEffect, damageEffect.GetComponent<ParticleSystem>().main.duration);
    }

    /// <summary>
    ///    //HPをセットする
    /// </summary>
    /// <param name="HP"></param>
    public void SetHP(int HP)
    {
        this.HP = HP;
    }

    /// <summary>
    /// HPを返す
    /// </summary>
    public int GetHP
    {
        get { return HP; }
    }

    protected abstract void Dodge();
}
