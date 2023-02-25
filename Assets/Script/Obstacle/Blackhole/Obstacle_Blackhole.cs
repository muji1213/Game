using UnityEngine;

public class Obstacle_Blackhole : UnCollisionable_Obstacle, IMovable
{
    [Header("引き寄せの強さ")] [SerializeField] float force;
    [Header("動くかどうか")] [SerializeField] bool isMove;
    [Header("動く方向")] [SerializeField] Vector3 moveDirection;
    [Header("動く速さ")] [SerializeField] float moveSpeed;

    private Rigidbody frisbeeRb;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume *= (SEManager.seManager.seVol);
    }

    private void FixedUpdate()
    {
        //動く設定をしてるなら
        if (isMove)
        {
            Move();
        }
    }

    public void Move()
    {
        this.transform.position += moveDirection * moveSpeed;
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
