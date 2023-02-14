using UnityEngine;

public class Obstacle_Planet : Collisionable_Obstacle, IMovable
{
    [Header("��������")] [SerializeField] float speed;
    [Header("��������")] [SerializeField] Vector3 direction;
    [Header("��]����")] [SerializeField] Vector3 rotateDirection;
    [Header("��]�X�s�[�h")] [SerializeField] float rotateSpeed;

    [Header("�t���X�r�[�g���K�[")] [SerializeField] CheckFrisbeeEnter checkFrisbeeEnter;

    //�g���K�[�œ����^�C�v�̃t���O
    private bool isMove = false;

    //�����Ȃ��^�C�v
    private bool isStop = false;

    private void Update()
    {
        if (isMove)
        {
            Destroy(this.gameObject, 8);
        }
    }

    public void Move()
    {
        this.transform.position += direction * speed;
        this.transform.Rotate(rotateDirection * rotateSpeed);
    }

    private void FixedUpdate()
    {
        //�C���X�y�N�^�Ńg���K�[���ݒ肳��Ă���Ȃ�
        if (checkFrisbeeEnter != null)
        {
            //�g���K�[���t���X�r�[���ʉ߂����瓮��
            isMove = checkFrisbeeEnter.CheckEnterFrisbee();
        }
        else
        {
            isStop = true;
        }


        if (isMove || isStop)
        {
            Move();
        }
    }
}
