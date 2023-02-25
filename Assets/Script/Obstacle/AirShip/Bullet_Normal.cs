using UnityEngine;

public class Bullet_Normal : Bullet
{

    [SerializeField] [Header("弾の速さは")] private float bulletSpeed;

    //フリスビーの位置
    private Vector3 targetPos;

    //飛んでいく方向
    Vector3 direction;

    new void Start()
    {
        base.Start();

        //フリスビーの方向を向く
        this.transform.LookAt(targetPos);

        //発射方向
        direction = (targetPos - this.transform.position).normalized;
    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();
        transform.position += direction * bulletSpeed;
    }

    //フリスビーの場所
    public Vector3 TargetPos
    {
        set
        {
            targetPos = value;
        }
    }
}
