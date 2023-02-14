using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEndTrigger : MonoBehaviour
{
    //ボス演出用
    [SerializeField] [Header("追突する惑星")] GameObject planet;
    [SerializeField] [Header("吹っ飛ばす強さ")] private float pow;
    [SerializeField] [Header("爆発音")] private AudioClip explosion;
    [SerializeField] [Header("爆発のエフェクト")] GameObject burstEff;
    [SerializeField] [Header("メインカメラ")] CameraFollower cameraFollower;

    private AudioSource audioSource;

    private void Start()
    {
        //音量
        audioSource = GetComponent<AudioSource>();
        audioSource.volume *= (SEManager.seManager.SeVolume / 1.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        //ボスだったら
        if (other.CompareTag("Enemy"))
        {
            //衝突位置を検知
            Vector3 pos = other.ClosestPointOnBounds(this.transform.position);

            //衝突位置に爆発のエフェクト
            GameObject burst = Instantiate(burstEff, pos, Quaternion.identity);
            Destroy(burst, burst.GetComponent<ParticleSystem>().main.duration);

            //ボスから衝突した惑星に向けてのベクトル
            Vector3 vec = planet.transform.position - pos;

            //惑星を吹っ飛ばす
            planet.GetComponent<Rigidbody>().AddForce(vec * pow, ForceMode.VelocityChange);
            planet.GetComponent<Rigidbody>().AddTorque(vec * pow, ForceMode.VelocityChange);
            Destroy(planet, 5.0f);

            //カメラを振動
            cameraFollower.Shake(0.5f, 1.0f);

            //爆発音
            audioSource.PlayOneShot(explosion);
        }
    }
}
