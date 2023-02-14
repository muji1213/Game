using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ړ��������͉�]�^��������
/// </summary>
public class Obstacle_ReturningPlanet : Collisionable_Obstacle, IMovable
{
    [Header("�ړ��o�H")] public GameObject[] movePoint;
    [Header("����")] public float speed = 1.0f;

    private int nowPoint = 0;
    private bool returnPoint = false;


    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        if (movePoint != null && movePoint.Length > 1 && rb != null)
        {
            //�ʏ�i�s
            if (!returnPoint)
            {
                int nextPoint = nowPoint + 1;

                //�ڕW�|�C���g�Ƃ̌덷���킸���ɂȂ�܂ňړ�
                if (Vector3.Distance(transform.position, movePoint[nextPoint].transform.position) > 0.1f)
                {
                    //���ݒn���玟�̃|�C���g�ւ̃x�N�g�����쐬
                    Vector3 toVector = Vector3.MoveTowards(transform.position, movePoint[nextPoint].transform.position, speed * Time.deltaTime);

                    //���̃|�C���g�ֈړ�
                    rb.MovePosition(toVector);
                }
                //���̃|�C���g���P�i�߂�
                else
                {
                    rb.MovePosition(movePoint[nextPoint].transform.position);
                    ++nowPoint;

                    //���ݒn���z��̍Ōゾ�����ꍇ
                    if (nowPoint + 1 >= movePoint.Length)
                    {
                        returnPoint = true;
                    }
                }
            }
            //�ܕԂ��i�s
            else
            {
                int nextPoint = nowPoint - 1;

                //�ڕW�|�C���g�Ƃ̌덷���킸���ɂȂ�܂ňړ�
                if (Vector3.Distance(transform.position, movePoint[nextPoint].transform.position) > 0.1f)
                {
                    //���ݒn���玟�̃|�C���g�ւ̃x�N�g�����쐬
                    Vector3 toVector = Vector3.MoveTowards(transform.position, movePoint[nextPoint].transform.position, speed * Time.deltaTime);

                    //���̃|�C���g�ֈړ�
                    rb.MovePosition(toVector);
                }
                //���̃|�C���g���P�߂�
                else
                {
                    rb.MovePosition(movePoint[nextPoint].transform.position);
                    --nowPoint;

                    //���ݒn���z��̍ŏ��������ꍇ
                    if (nowPoint <= 0)
                    {
                        returnPoint = false;
                    }
                }
            }
        }
    }
}
