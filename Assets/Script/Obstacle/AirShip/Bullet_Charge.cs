using UnityEngine;

public class Bullet_Charge : Bullet
{
    //溜め開始から攻撃判定が出るまでの時間
    private float attackDelay;
    [SerializeField] [Header("攻撃判定のオブジェクト")] private BoxCollider hitBox;
    [SerializeField] [Header("攻撃エフェクト")] private ParticleSystem leser;

    private float timer = 0.0f;

    new void Start()
    {
        base.Start();
    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();
        //タイマーが消滅時間を超えたら消す
        if (timer >= destroyTime)
        {
            timer = 0.0f;

            //当たり判定を無効に
            hitBox.enabled = false;

            //エフェクトを無効に
            leser.Stop();

            return;
        }

        //タイマーが攻撃判定が出るまでの時間を超えた場合
        if (timer >= attackDelay)
        {
            //当たり判定を出現させる
            hitBox.enabled = true;

            //エフェクトを出す
            leser.Play();
        }
        else
        {
            //ため中は当たり判定は無し
            hitBox.enabled = false;
            leser.Stop();
        }

        timer += Time.deltaTime;
    }

    //予測線の表示から攻撃判定が出るまでの時間
    public float AttackDelay
    {
        set
        {
            attackDelay = value;
        }
    }
}
