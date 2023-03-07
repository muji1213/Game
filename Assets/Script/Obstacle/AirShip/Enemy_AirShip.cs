using UnityEngine;
using Cinemachine;

public class Enemy_AirShip : MonoBehaviour, IMovable
{
    [HideInInspector]
    public enum State
    {
        NormalAttack,
        ChargeAttack,
        BombAttack,
        Dead
    }

    [SerializeField] [Header("フリスビーの侵入判定")] BossTrigger checkFrisbee;
    [SerializeField] [Header("doll cart")] CinemachineDollyCart dollyCart;
    [SerializeField] [Header("登場時パス")] CinemachineSmoothPath startPath;
    [SerializeField] [Header("攻撃開始から終了まで使う移動パス")] CinemachineSmoothPath movePath;
    [SerializeField] [Header("照準UI")] GameObject rockOnUI;
    [SerializeField] [Header("通常攻撃の玉")] GameObject bullet_Normal;
    [SerializeField] [Header("チャージ攻撃の玉")] GameObject bullet_Charge;
    [SerializeField] [Header("飛行SE再生用AudioSource")] AudioSource flySource;
    [SerializeField] [Header("通常攻撃SE")] AudioClip normalAttackSE;
    [SerializeField] [Header("通常攻撃のSEの音量")] [Range(0, 1)] float normalAttackSEVol = 1;
    [SerializeField] [Header("ボム設置SE")] AudioClip bombInstalledSE;
    [SerializeField] [Header("ボム設置SEの音量")] [Range(0, 1)] float bombInstalledSEVol = 1;
    [SerializeField] [Header("チャージSE")] AudioClip chargeSE;
    [SerializeField] [Header("チャージSEの音量")] [Range(0, 1)] float chargeSEVol = 1;
    [SerializeField] [Header("チャージ攻撃SE")] AudioClip leserSE;
    [SerializeField] [Header("チャージ攻撃SEの音量")] [Range(0, 1)] float leserSEVol = 1;
    [SerializeField] [Header("死亡時の煙")] ParticleSystem smoke;

    //フリスビーのリジットボディ
    Rigidbody frisbeeRb;

    //ステート
    public State state;

    //アニメーター
    [SerializeField] [Header("アニメーター")] private Animator anim;

    //登場時のスピード
    [SerializeField] [Header("ボストリガーを踏んでから規定位置に到着するまでのスピード")] private float startSpeed;

    //開始位置についたか
    private bool isStart = false;

    //中間位置についたか
    private bool isMiddle = false;

    //終了位置についたか
    private bool isEnd = false;

    //フリスビーのゲームオブジェクト
    private GameObject frisbee;

    //通常攻撃------------------------------------

    //ロックオンの時間
    [SerializeField] [Header("照準を定めてから撃つまでの時間")] private float rockOnTime;

    //ロックオンタイマー
    private float rockOnTimer = 0.0f;

    //攻撃までのディレイ
    [SerializeField] [Header("ロックオン時間が終了してから実際に弾を発射するまでのディレイ")] private float attackDuration;

    //攻撃時のエフェクト
    [SerializeField] ParticleSystem effect1;
    [SerializeField] ParticleSystem effect2;

    //通常攻撃の発射位置
    [SerializeField] [Header("通常攻撃発射位置")] GameObject weapon_Normal;

    //通常攻撃の残存時間
    [SerializeField] [Header("通常攻撃の残存時間")] float normalAttackDestroyTime;


    //-----------------------------------------

    //チャージ攻撃---------------------------------------------

    //チャージ攻撃の必要攻撃回数
    [SerializeField] [Header("チャージ攻撃に必要な攻撃回数は")] private int chargeAttackNum;

    //攻撃回数
    private int attackNum = 1;

    //チャージ攻撃の継続時間
    [SerializeField] [Header("チャージ攻撃の継続時間は")] private int chargeAttackTime;

    [SerializeField] [Header("チャージ攻撃判定が出るまでの時間")] private float chargeAttackDelay;

    //チャージ攻撃の経過時間
    private float chargeAttackTimer = 0.0f;


    //地雷------------------------------------------------------------
    [SerializeField] [Header("時限爆弾のオブジェクト")] GameObject limitBomb;

    //地雷の必要攻撃回数
    [SerializeField] [Header("時限爆弾攻撃に必要な攻撃回数")] private int bombAttackNum;

    //地雷攻撃後一定時間攻撃を行わない時間
    [SerializeField] [Header("時限爆弾攻撃後攻撃を行わない時間")] private float bombAttackCoolTime;

    //攻撃クールタイムのタイマー
    private float bombAttackCoolTimer = 0.0f;

    //ボム攻撃の残存時間
    [SerializeField] [Header("爆弾の残存時間")] float bombDestoryTime;

    //------------------------------------------------------------

    //武器１の位置
    [SerializeField] GameObject weapon1;

    //武器2の位置
    [SerializeField] GameObject weapon2;

    //武器３
    [SerializeField] GameObject weapon3;

    //武器４
    [SerializeField] GameObject weapon4;

    //時限爆弾生成位置
    [SerializeField] GameObject bombWeapon;

    //UIのアニメーター
    [SerializeField] Animator UIanimator;

    private void Start()
    {
        flySource.volume *= (SEManager.seManager.seVol);

        //ステートは通常攻撃にしておく
        state = State.NormalAttack;

        //初回に溜め攻撃をしないよう、1から始める
        attackNum = 1;

        //位置とパスを初期化
        dollyCart.m_Path = startPath;
        dollyCart.m_Position = 0;
    }

    private void Update()
    {
        //アニメーション
        SetAnim();
        SetUIAnim();

        Move();


        if (isStart && !isEnd)
        {
            //フリスビーが墜落した場合、攻撃停止する
            if (frisbee.GetComponent<Frisbee>().GetHP == 0)
            {
                isEnd = true;
            }

            //スピードを常にフリスビーを同じにする
            dollyCart.m_Speed = -frisbeeRb.velocity.z;

            //チャージ攻撃回数と、ボム攻撃回数が同時に起きた場合
            if (attackNum % chargeAttackNum == 0 && attackNum % bombAttackNum == 0)
            {
                //ボム優先               
                state = State.BombAttack;
            }

            //攻撃回数がボム攻撃回数になったら
            else if (attackNum % bombAttackNum == 0)
            {
                //ボム攻撃状態に
                state = State.BombAttack;
            }
            else if (attackNum % chargeAttackNum == 0)
            {
                //チャージ攻撃状態にする
                state = State.ChargeAttack;
            }
            else
            {
                //それ以外は通常攻撃
                state = State.NormalAttack;
            }

            switch (state)
            {
                //ボム攻撃
                case State.BombAttack:

                    //ボム後の攻撃を行わない時間を過ぎたら
                    if (bombAttackCoolTimer >= bombAttackCoolTime)
                    {
                        //攻撃回数を更新
                        attackNum += 1;

                        //タイマーリセット
                        bombAttackCoolTimer = 0;
                        return;
                    }
                    else if (bombAttackCoolTimer == 0)
                    {
                        BombAttack();
                    }

                    bombAttackCoolTimer += Time.deltaTime;
                    break;

                //チャージ攻撃
                case State.ChargeAttack:

                    //チャージ攻撃時間を過ぎたら
                    if (chargeAttackTimer >= chargeAttackTime)
                    {
                        //攻撃回数を更新
                        attackNum += 1;

                        //タイマーリセット
                        chargeAttackTimer = 0;
                        return;
                    }
                    else if (chargeAttackTimer == 0)
                    {
                        ChargeAttack();
                    }

                    chargeAttackTimer += Time.deltaTime;
                    break;

                //通常攻撃
                case State.NormalAttack:

                    //ロックオンタイマーを進める
                    rockOnTimer += Time.deltaTime;

                    //中間を到達していたら射撃速度が上昇する
                    if (isMiddle)
                    {
                        rockOnTimer += Time.deltaTime;
                    }

                    //ロックオンタイマーが規定値を超えるまでロックオンする
                    if (rockOnTimer >= rockOnTime)
                    {
                        Invoke("Attack", attackDuration);
                    }
                    else if (rockOnTimer >= rockOnTime * 0.4f)
                    {
                        //一定値以上ロックオンすると照準が表示される
                        NormalAttackRockOn();
                    }
                    break;
            }
        }
    }

    public void Move()
    {
        //フリスビーを感知したら移動する
        if (checkFrisbee.CheckEnterFrisbee() && isStart == false)
        {
            flySource.Play();
            dollyCart.m_Speed = startSpeed;
        }

        //スタート位置についたらパスを変更
        if (dollyCart.m_Position == startPath.PathLength && Mathf.Abs(this.transform.position.z - GameObject.FindWithTag("Frisbee").transform.position.z) < 20f)
        {
            //フリスビーのリジットボディを取得
            frisbeeRb = GameObject.FindWithTag("Frisbee").GetComponent<Rigidbody>();

            //フラグをおろす
            isStart = true;

            //パスを変更
            dollyCart.m_Path = movePath;

            //パスの初期位置に移動する
            dollyCart.m_Position = movePath.PathLength;

            frisbee = GameObject.FindWithTag("Frisbee");
        }

        //終了位置についたら
        if (dollyCart.m_Path == movePath && isEnd)
        {
            rockOnUI.SetActive(false);
            dollyCart.m_Speed = -frisbeeRb.velocity.z;
            state = State.Dead;
        }

        //パスの最終地点まで行ったら非アクティブに
        if (dollyCart.m_Path == movePath && dollyCart.m_Position == 0)
        {
            this.gameObject.SetActive(false);
        }

    }

    //ロックオン中、照準を表示する
    private void NormalAttackRockOn()
    {
        //照準が表示されてないので表示
        if (!rockOnUI.activeSelf)
        {
            rockOnUI.SetActive(true);
        }

        UIanimator.SetTrigger("RockOn");
    }

    //通常攻撃
    private void Attack()
    {
        SEManager.seManager.PlaySE(normalAttackSEVol, normalAttackSE);

        //攻撃と同時にタイマーリセット
        rockOnTimer = 0.0f;

        //攻撃と同時に照準を消す
        rockOnUI.SetActive(false);

        //攻撃回数更新
        attackNum += 1;

        //アニメーション
        anim.SetTrigger("NormalAttack");

        //発射口から弾を放出
        GameObject bullet1 = Instantiate(bullet_Normal, weapon_Normal.transform.position, Quaternion.identity);
        Bullet_Normal bullet_normal1 = bullet1.GetComponent<Bullet_Normal>();
        bullet_normal1.TargetPos = frisbee.transform.position;
        bullet_normal1.DestroyTime = normalAttackDestroyTime;
        bullet_normal1.AirShip = this;
        effect1.Play();
    }

    //チャージ攻撃
    private void ChargeAttack()
    {
        SEManager.seManager.PlaySE(chargeSEVol, chargeSE);

        anim.SetTrigger("ChargeAttack");

        //チャージ攻撃オブジェクトを、武器1から生成
        GameObject leser1 = Instantiate(bullet_Charge, weapon1.transform.position, Quaternion.identity);
        leser1.transform.SetParent(weapon1.transform);
        Bullet_Charge bullet_Charge1 = leser1.GetComponent<Bullet_Charge>();

        //チャージ攻撃の継続時間、および溜め開始から実際に攻撃判定が出るまでの時間を渡す（設定はインスペクタで）
        bullet_Charge1.DestroyTime = chargeAttackTime;
        bullet_Charge1.AttackDelay = chargeAttackDelay;
        bullet_Charge1.AirShip = this;

        //チャージ攻撃オブジェクトを、武器2から生成
        GameObject leser2 = Instantiate(bullet_Charge, weapon2.transform.position, Quaternion.identity);
        leser2.transform.SetParent(weapon2.transform);
        Bullet_Charge bullet_Charge2 = leser2.GetComponent<Bullet_Charge>();

        //チャージ攻撃の継続時間、および溜め開始から実際に攻撃判定が出るまでの時間を渡す（設定はインスペクタで）
        bullet_Charge2.DestroyTime = chargeAttackTime;
        bullet_Charge2.AttackDelay = chargeAttackDelay;
        bullet_Charge2.AirShip = this;


        //中間地点を超えた場合、射出場所が増える
        if (isMiddle)
        {
            //チャージ攻撃オブジェクトを、武器3から生成
            GameObject leser3 = Instantiate(bullet_Charge, weapon3.transform.position, Quaternion.identity);
            leser3.transform.SetParent(weapon3.transform);
            Bullet_Charge bullet_Charge3 = leser3.GetComponent<Bullet_Charge>();

            //チャージ攻撃の継続時間、および溜め開始から実際に攻撃判定が出るまでの時間を渡す（設定はインスペクタで）
            bullet_Charge3.DestroyTime = chargeAttackTime;
            bullet_Charge3.AttackDelay = chargeAttackDelay;
            bullet_Charge3.AirShip = this;

            //チャージ攻撃オブジェクトを、武器3から生成
            GameObject leser4 = Instantiate(bullet_Charge, weapon4.transform.position, Quaternion.identity);
            leser4.transform.SetParent(weapon4.transform);
            Bullet_Charge bullet_Charge4 = leser4.GetComponent<Bullet_Charge>();

            //チャージ攻撃の継続時間、および溜め開始から実際に攻撃判定が出るまでの時間を渡す（設定はインスペクタで）
            bullet_Charge4.DestroyTime = chargeAttackTime;
            bullet_Charge4.AttackDelay = chargeAttackDelay;
            bullet_Charge4.AirShip = this;
        }

        Invoke("PlayLeserSE", chargeAttackDelay);
    }

    //レーザSE
    private void PlayLeserSE()
    {
        SEManager.seManager.PlaySE(leserSEVol, leserSE);
    }

    //ボム攻撃
    private void BombAttack()
    {
        SEManager.seManager.PlaySE(bombInstalledSEVol, bombInstalledSE);

        //ボムを生成、フリスビーの位置と残存時間を渡す
        GameObject bomb = Instantiate(limitBomb, bombWeapon.transform.position, Quaternion.identity);
        Bullet_Mine bullet_mine1 = bomb.GetComponent<Bullet_Mine>();
        bullet_mine1.Frisbee = frisbee;
        bullet_mine1.DestroyTime = bombDestoryTime;
        bullet_mine1.AirShip = this;
    }


    //アニメーション設定
    private void SetAnim()
    {
        //宇宙船のアニメーション
        anim.SetBool("isStart", isStart);
    }

    //照準のアニメーション
    private void SetUIAnim()
    {
        //発射時間が迫るほどアニメーションが加速する
        UIanimator.SetFloat("Speed", 1 + rockOnTimer);
    }

    //トリガー
    private void OnTriggerEnter(Collider other)
    {
        //終了判定、以降攻撃を行わない
        if (other.CompareTag("End"))
        {
            isEnd = true;
            smoke.Play();
        }

        //中間地点、攻撃速度が上昇する
        if (other.CompareTag("Middle"))
        {
            isMiddle = true;
        }
    }

    //ステートを返す
    public State GetState()
    {
        return state;
    }
}
