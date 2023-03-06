using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunaiFrisbee : Frisbee
{
    [SerializeField] [Header("回避時間")] private int dodgeTime;
    [SerializeField] [Header("回避クールタイム")] private int dodgeCoolTime;
    [SerializeField] [Header("煙エフェクト")] private ParticleSystem smoke;

    private Color color;

    //回避しているか
    private bool isDodge = false;

    //回避可能状態か
    private bool canDodge = true;

    //回避クールタイマー
    private float dodgeCoolTimer = 0.0f;

    //向き調整
    private void OnEnable()
    {
        transform.Rotate(0, 180, 0);
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();

        if (!isDodge)
        {
            //クールタイム中なら
            if (dodgeCoolTimer < dodgeCoolTime)
            {
                //タイムを計る
                dodgeCoolTimer += Time.deltaTime;

                //回避はできない
                canDodge = false;
            }
            else
            {
                //
                canDodge = true;
            }
        }    
    }

    protected override void Dodge()
    {
        //回避中でないかつ、被弾無敵時間中でない
        if (!isDodge && !isInvincible && canDodge)
        {
            StartCoroutine(TheDodge(dodgeTime));

            //タイマーリセット
            dodgeCoolTimer = 0.0f;

            //回避中フラグ
            isDodge = true;
        }
    }

    private IEnumerator TheDodge(int time)
    {
        //エフェクト
        smoke.Play();

        //レイヤーをほかのレイヤーにすることで、障害物との衝突を回避させる
        this.gameObject.layer = 10;

        //半透明にする
        this.gameObject.GetComponent<MeshRenderer>().material.color = new Color(color.a, color.g, color.b, 0.3f);

        yield return new WaitForSeconds(time);

        //一定時間透明度を後戻す
        this.gameObject.GetComponent<MeshRenderer>().material.color = new Color(color.a, color.g, color.b, 1.0f);

        //レイヤーをほかのレイヤーにすることで、障害物との衝突を回避させる
        this.gameObject.layer = 9;

        //回避中でない
        isDodge = false;

        //エフェクト
        smoke.Play();
    }
}
