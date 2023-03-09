using UnityEngine;

//��Q���i�؁j�̃X�N���v�g
public class Obstacle_Tree : Collisionable_Obstacle, IRoatatable
{
    [Header("�|�󂷂鑬��")] [SerializeField] private float rotateSpeed;
    [Header("�|�󂷂����")] [SerializeField] private Vector3 rotateVec;
    [Header("�v���C���[���m�̃X�N���v�g")] [SerializeField] private CheckFrisbeeEnter checkFrisbeeEnter;

    [Header("�|�󎞂�SE")] [SerializeField] AudioClip rotateSE;
    [Header("�|�󎞂�SE�̉���")] [SerializeField] [Range(0, 1)] float rotateSEVol = 1;

    //���ł�����t���O
    private bool isDestroy = false;

    //SE�̃t���O
    private bool isSounded = false;

    private void FixedUpdate()
    {
        if (isDestroy)
        {
            //���ł����Ȃ��Ɖ�]�������邽�ߏ���
            Destroy(this.gameObject, 7.0f);
        }
        Rotate();
    }

    //�t���X�r�[������̈ʒu�ɐڐG������
    public void Rotate()
    {
        if (checkFrisbeeEnter.CheckEnterFrisbee())
        {
            //��]������
            this.transform.Rotate(rotateVec * rotateSpeed);

            //���ł�����t���O�𗧂Ă�
            isDestroy = true;

            if (!isSounded)
            {
                SEManager.I.PlaySE(rotateSEVol, rotateSE);
                isSounded = true;
            }
            else
            {
                return;
            }
        }
    }
}
