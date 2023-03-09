using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEndTrigger : Trigger
{
    //ボス演出用
    [SerializeField] [Header("追突する惑星")] GameObject planet;
    [SerializeField] [Header("吹っ飛ばす強さ")] private float pow;
    [SerializeField] [Header("爆発音")] private AudioClip explosion;
    [SerializeField] [Header("爆発音の音量")] [Range(0, 1)] float explosionSEVol = 1;
    [SerializeField] [Header("爆発のエフェクト")] GameObject burstEff;
    [SerializeField] [Header("メインカメラ")] CameraFollower cameraFollower;

    private void Start()
    {
        targetTag = "Enemy";
    }

    protected override void ActiveEvent()
    {
        //衝突位置に爆発のエフェクト
        GameObject burst = Instantiate(burstEff, colPos, Quaternion.identity);
        Destroy(burst, burst.GetComponent<ParticleSystem>().main.duration);

        //ボスから衝突した惑星に向けてのベクトル
        Vector3 vec = planet.transform.position - colPos;

        //惑星を吹っ飛ばす
        planet.GetComponent<Rigidbody>().AddForce(vec * pow, ForceMode.VelocityChange);
        planet.GetComponent<Rigidbody>().AddTorque(vec * pow, ForceMode.VelocityChange);
        Destroy(planet, 5.0f);

        //カメラを振動
        cameraFollower.Shake(0.5f, 1.0f);

        //爆発音
        SEManager.I.PlaySE(explosionSEVol, explosion);
    }
}
