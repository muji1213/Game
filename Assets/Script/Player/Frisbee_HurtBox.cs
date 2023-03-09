using UnityEngine;

public class Frisbee_HurtBox : MonoBehaviour
{
    //�e��^�O

    //�t���X�r�[�^�O
    private string frisbeeTag = "Frisbee";

    //��Q���^�O
    private string obstacleTag = "Obstacle";

    //�^�[�Q�b�g�^�O�i�S�[���j
    private string targetTag = "Target";

    //�R�C���^�O
    private string coinTag = "Coin";

    //���S����i���A�������̓S�[�����O�ꂽ�ꍇ�j
    private string deadColTag = "DeadCol";

    private Frisbee frisbee; //�t���X�r�[�̃X�N���v�g
    private MeshCollider meshCollider;

    private void Start()
    {
        //�e��R���|�[�l���g�擾

        //�t���X�r�[
        frisbee = GetComponent<Frisbee>();

        //���b�V���R���C�_�[
        meshCollider = GetComponent<MeshCollider>();
    }

    //�Փ˔���

    //�t���X�r�[�͏�Q���Ƃ̏Փ˔���A����уS�[�������蔲���Ă��܂����ۂ̔�������
    //�v���C���[�͏�Q���Ƃ̏Փ˔���A����ь��ɗ������ۂ̔�������
    private void OnCollisionEnter(Collision collision)
    {
        //��Q���Ƃ̏Փ˔���
        if (collision.gameObject.tag == obstacleTag)
        {
            //��Q���Ƃ̏Փ˂��t���X�r�[�������ꍇ
            //
            //����
            //��Q���̃��x�����t���X�r�[���Ⴂ�ꍇ�������Ȃ�
            //
            //��Q���̃��x�����t���X�r�[��荂�������ꍇ
            //�@���C�t��0���傫����΁A���G���Ԃɂ��A�t���X�r�[�̃��C�t�����炷
            //�@���C�t��0�ȉ��Ȃ�΁A���S�����True��
            if (this.gameObject.tag == frisbeeTag)
            {
                //��Q���̃X�N���v�g���擾
                Collisionable_Obstacle obstacle = collision.gameObject.GetComponent<Collisionable_Obstacle>();

                //�t���X�r�[�Ə�Q���̃��x�����r
                //�t���X�r�[�̕����Ⴏ���
                if (obstacle.level >= GameManager.I.SelectedFrisbeeInfo.FrisbeeLevel)
                {
                    //���C�t�����炷
                    frisbee.ReduceLife();
                    frisbee.PlayDamageEffect(collision.contacts[0].point);

                    //���C�t��0�ȉ��ɂȂ�����
                    if (frisbee.GetHP <= 0)
                    {

                        //���S�����True��
                        frisbee.TheDie(0);
                    }
                    //0���傫�����
                    else
                    {
                        //���G���Ԃ�
                        frisbee.Invincible(3);
                    }
                }
                //�t���X�r�[�̃��x���̂ق������������ꍇ
                else
                {
                    //�������Ȃ�
                    return;
                }
            }
        }
        else
        {
           
        }

        //�t���X�r�[���^�[�Q�b�g�i�S�[���j�ƏՓ˂����ꍇ
        if (collision.gameObject.tag == targetTag)
        {
            frisbee.Clear();

            meshCollider.enabled = false;

            //SE���~�߂�
            this.GetComponent<AudioSource>().enabled = false;
        }
    }

    //���S����ɏՓ˂����ꍇ�i�����j
    private void OnTriggerEnter(Collider other)
    {
        //���S����ƏՓ˂����ꍇ
        if (other.CompareTag(deadColTag))
        {
            //�t���X�r�[�̏ꍇ
            if (this.gameObject.tag == frisbeeTag)
            {
                //SE���~�߂�
                this.GetComponent<AudioSource>().enabled = false;

                //���S�����True��
                frisbee.TheDie(1);
            }
        }


        //�{�X�̍U���ɂ��������ꍇ
        if (other.CompareTag("Attack"))
        {
            frisbee.ReduceLife();
            frisbee.PlayDamageEffect(other.ClosestPointOnBounds(this.gameObject.transform.position));

            if (frisbee.GetHP <= 0)
            {
                frisbee.TheDie(0);
            }
            else
            {
                //���G���Ԃ�
                frisbee.Invincible(3);
            }
        }

        //�R�C���ƏՓ˂����ꍇ
        if (other.CompareTag(coinTag))
        {
            if (this.gameObject.tag == frisbeeTag)
            {
                int score = other.GetComponent<Coin>().Score;

                frisbee.GetCoin(score);
            }

        }
    }
}
