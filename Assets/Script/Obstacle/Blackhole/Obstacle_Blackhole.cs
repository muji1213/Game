using UnityEngine;

public class Obstacle_Blackhole : UnCollisionable_Obstacle
{
    [Header("引き寄せの強さ")] [SerializeField] float force;

    private Rigidbody frisbeeRb;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume *= (SEManager.I.seVol);
    }

    private void OnTriggerStay(Collider other)
    {
        //フリスビーが引き寄せ判定に入っている場合
        if (other.CompareTag("Frisbee"))
        {
            //リジットボディを取得
            frisbeeRb = other.gameObject.GetComponent<Rigidbody>();

            //フリスビーから自身に対してのベクトル生成
            Vector3 direction = (this.transform.position - other.gameObject.transform.position).normalized;

            //引き寄せる
            frisbeeRb.AddForce(direction * force);
        }
    }
}
