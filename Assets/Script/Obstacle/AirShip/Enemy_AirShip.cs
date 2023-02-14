using UnityEngine;
using Cinemachine;

public class Enemy_AirShip : MonoBehaviour
{
    private enum State
    {
        NormalAttack,
        ChargeAttack,
        BombAttack
    }

    [SerializeField] [Header("フリスビーの侵入判定")] BossTrigger checkFrisbee;
    [SerializeField] [Header("doll cart")] CinemachineDollyCart dollyCart;
    [SerializeField] [Header("登場時パス")] CinemachineSmoothPath startPath;
    [SerializeField] [Header("攻撃開始から終了まで使う移動パス")] CinemachineSmoothPath movePath;
    [SerializeField] [Header("照準UI")] GameObject rockOnUI;
    [SerializeField] [Header("通常攻撃の玉")] GameObject bullet_Normal;
    [SerializeField] [Header("チャージ攻撃の玉")] GameObject bullet_Charge;
    [SerializeField] [Header("飛行SE再生用AudioSource")] AudioSource flySource;
    [SerializeField] [Header("攻撃SE再生用AudioSource")] AudioSource attackSource;
    [SerializeField] [Header("通常攻撃SE")] AudioClip normalAttackSE;
    [SerializeField] [Header("ボム接地SE")] AudioClip bombInstalledSE;
    [SerializeField] [Header("チャージSE")] AudioClip chargeSE;
    [SerializeField] [Header("チャージ攻撃SE")] AudioClip leserSE;
    [SerializeField] [Header("死亡時の煙")] ParticleSystem smoke;

    //フリスビーのリジットボディ
    Rigidbody frisbeeRb;

    //ステート
    State state;

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
    [SerializeField][Header("ロックオン時間が終了してから実際に弾を発射するまでのディレイ")] private float attackDuration;

    //攻撃時のエフェクト
    [SerializeField] ParticleSystem effect1;
    [SerializeField] ParticleSystem effect2;

    //通常攻撃の発射位置
    [SerializeField] [Header("通常攻撃発射位置")] GameObject weapon_Normal;


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
        flySource.volume *= (SEManager.seManager.SeVolume / 1.0f);
        attackSource.volume *= (SEManager.seManager.SeVolume / 1.0f);

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

            //フリスビーの方を向きながら後退させたいので、パスを逆走させる
            dollyCart.m_Position = movePath.PathLength;

            frisbee = GameObject.FindWithTag("Frisbee");
        }

        //終了位置についたらフラグをおろす
        if (dollyCart.m_Path == movePath && isEnd)
        {
            rockOnUI.SetActive(false);
            dollyCart.m_Speed = -frisbeeRb.velocity.z;
        }

        //パスの最終地点まで行ったら非アクティブに
        if (dollyCart.m_Path == movePath && dollyCart.m_Position == 0)
        {
            this.gameObject.SetActive(false);
        }

        //スピードを常にフリスビーを同じにする
        if (isStart && !isEnd)
        {
            //フリスビーが墜落した場合、攻撃停止する
            if (frisbee.GetComponent<Frisbee>().GetHP == 0)
            {
                isEnd = true;
            }

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
        attackSource.PlayOneShot(normalAttackSE);

        //攻撃と同時にタイマーリセット
        rockOnTimer = 0.0f;

        //攻撃と同時に照準を消す
        rockOnUI.SetActive(false);

        //攻撃回数更新
        attackNum += 1;

        //アニメーション
        anim.SetTrigger("NormalAttack");

        //左右の発射口から弾を放出
        GameObject bullet1 = Instantiate(bullet_Normal, weapon_Normal.transform.position, Quaternion.identity);
        bullet1.GetComponent<Bullet_Normal>().TargetPos = frisbee.transform.position;
        effect1.Play();
    }

    //チャージ攻撃
    private void ChargeAttack()
    {
        attackSource.PlayOneShot(chargeSE);

        anim.SetTrigger("ChargeAttack");

        //チャージ攻撃オブジェクトを、武器1から生成
        GameObject leser1 = Instantiate(bullet_Charge, weapon1.transform.position, Quaternion.identity);
        leser1.transform.SetParent(weapon1.transform);

        //チャージ攻撃の継続時間、および溜め開始から実際に攻撃判定が出るまでの時間を渡す（設定はインスペクタで）
        leser1.GetComponent<Bullet_Charge>().DestroyTime = chargeAttackTime;
        leser1.GetComponent<Bullet_Charge>().AttackDelay = chargeAttackDelay;


        //チャージ攻撃オブジェクトを、武器2から生成
        GameObject leser2 = Instantiate(bullet_Charge, weapon2.transform.position, Quaternion.identity);
        leser2.transform.SetParent(weapon2.transform);

        //チャージ攻撃の継続時間、および溜め開始から実際に攻撃判定が出るまでの時間を渡す（設定はインスペクタで）
        leser2.GetComponent<Bullet_Charge>().DestroyTime = chargeAttackTime;
        leser2.GetComponent<Bullet_Charge>().AttackDelay = chargeAttackDelay;


        //中間地点を超えた場合、射出場所が増える
        if (isMiddle)
        {
            //チャージ攻撃オブジェクトを、武器3から生成
            GameObject leser3 = Instantiate(bullet_Charge, weapon3.transform.position, Quaternion.identity);
            leser3.transform.SetParent(weapon3.transform);

            //チャージ攻撃の継続時間、および溜め開始から実際に攻撃判定が出るまでの時間を渡す（設定はインスペクタで）
            leser3.GetComponent<Bullet_Charge>().DestroyTime = chargeAttackTime;
            leser3.GetComponent<Bullet_Charge>().AttackDelay = chargeAttackDelay;


            //チャージ攻撃オブジェクトを、武器3から生成
            GameObject leser4 = Instantiate(bullet_Charge, weapon4.transform.position, Quaternion.identity);
            leser4.transform.SetParent(weapon4.transform);

            //チャージ攻撃の継続時間、および溜め開始から実際に攻撃判定が出るまでの時間を渡す（設定はインスペクタで）
            leser4.GetComponent<Bullet_Charge>().DestroyTime = chargeAttackTime;
            leser4.GetComponent<Bullet_Charge>().AttackDelay = chargeAttackDelay;
        }

        Invoke("PlayLeserSE", chargeAttackDelay);
    }

    //レーザSE
    private void PlayLeserSE()
    {
        attackSource.PlayOneShot(leserSE);
    }

    //ボム攻撃
    private void BombAttack()
    {
        attackSource.PlayOneShot(bombInstalledSE);

        GameObject bomb = Instantiate(limitBomb, bombWeapon.transform.position, Quaternion.identity);
        bomb.GetComponent<Bullet_Mine>().Frisbee = frisbee;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("End"))
        {
            isEnd = true;
            smoke.Play();
        }

        if (other.CompareTag("Middle"))
        {
            isMiddle = true;
        }
    }
}
