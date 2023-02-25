using UnityEngine;

public class Bullet_Normal : Bullet
{

    [SerializeField] [Header("�e�̑�����")] private float bulletSpeed;

    //�t���X�r�[�̈ʒu
    private Vector3 targetPos;

    //���ł�������
    Vector3 direction;

    new void Start()
    {
        base.Start();

        //�t���X�r�[�̕���������
        this.transform.LookAt(targetPos);

        //���˕���
        direction = (targetPos - this.transform.position).normalized;
    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();
        transform.position += direction * bulletSpeed;
    }

    //�t���X�r�[�̏ꏊ
    public Vector3 TargetPos
    {
        set
        {
            targetPos = value;
        }
    }
}
