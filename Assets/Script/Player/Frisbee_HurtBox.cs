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

    //���S����
    private bool isDead = false;

    private Frisbee frisbee; //�t���X�r�[�̃X�N���v�g
    private CameraFollower cameraFollower; //�J�����Ǐ]�p�̃X�N���v�g
    private MeshCollider meshCollider;

    private void Start()
    {
        //�e��R���|�[�l���g�擾

        //�t���X�r�[
        frisbee = GetComponent<Frisbee>();

        //�J�����Ǐ]�p�̃X�N���v�g
        cameraFollower = GameObject.Find("Main Camera").GetComponent<CameraFollower>();

        //���b�V���R���C�_�[
        meshCollider = GetComponent<MeshCollider>();
    }

    /// <summary>
    /// ���S����擾���\�b�h
    /// �v���C���[�������̓t���X�r�[�����S�����ۂ�True��Ԃ�
    /// </summary>
    /// <returns></returns>
    public bool isDeadCheck()
    {
        if (isDead)
        {
            meshCollider.enabled = false;
            return true;
        }
        else
        {
            return false;
        }
    }


    //�Փ˔���

    //�t���X�r�[�͏�Q���Ƃ̏Փ˔���A����уS�[�������蔲���Ă��܂����ۂ̔�������
    //�v���C���[�͏�Q���Ƃ̏Փ˔���A����ь��ɗ������ۂ̔�������
    private void OnCollisionEnter(Collision collision)
    {
        //��Q���Ƃ̏Փ˔���
        if (collision.gameObject.tag == obstacleTag)
        {
            //�Փˎ��A�J������U��������
            cameraFollower.Shake(0.3f, 1f);


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
                if (obstacle.level >= StageSelectManager.selectedFrisbee.FrisbeeLevel)
                {
                    //���C�t�����炷
                    frisbee.ReduceLife();
                    frisbee.PlayDamageEffect(collision.contacts[0].point);

                    //���C�t��0�ȉ��ɂȂ�����
                    if (frisbee.GetHP <= 0)
                    {

                        //���S�����True��
                        frisbee.TheDie(0);
                        isDead = true;

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
            isDead = false;
        }

        //�t���X�r�[���^�[�Q�b�g�i�S�[���j�ƏՓ˂����ꍇ
        if (collision.gameObject.tag == targetTag)
        {
            //�t���X�r�[�̑���𖳌��ɂ���
            frisbee.enabled = false;

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
                isDead = true;
            }
        }


        //�{�X�̍U���ɂ��������ꍇ
        if (other.CompareTag("Attack"))
        {
            frisbee.ReduceLife();
            frisbee.PlayDamageEffect(other.ClosestPointOnBounds(this.gameObject.transform.position));

            if (frisbee.GetHP <= 0)
            {
                isDead = true;
                
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
                frisbee.PlayCoinEffect();
            }

        }
    }
}
