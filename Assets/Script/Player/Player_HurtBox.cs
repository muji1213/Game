using UnityEngine;


//���̃X�N���v�g�̓v���C���[����уt���X�r�[�̏Փ˔�����擾����X�N���v�g
public class Player_HurtBox : MonoBehaviour
{
    //�e��^�O

    //�v���C���[�^�O
    private string playerTag = "Player";

    //��Q���^�O
    private string obstacleTag = "Obstacle";

    //�R�C���^�O
    private string coinTag = "Coin";

    //���S����i���A�������̓S�[�����O�ꂽ�ꍇ�j
    private string deadColTag = "DeadCol";

    //���S����
    private bool isDead = false;

    private Player_Controller player; //�v���C���[�R���g���[���[�i������j
    private CameraFollower cameraFollower; //�J�����Ǐ]�p�̃X�N���v�g

    private void Start()
    {
        //�e��R���|�[�l���g�擾

        //�v���C���[�i������j
        player = GetComponent<Player_Controller>();

        //�J�����Ǐ]�p�̃X�N���v�g
        cameraFollower = GameObject.Find("Main Camera").GetComponent<CameraFollower>();
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

            //��Q���Ƃ̏Փ˂��v���C���[�i������j�������ꍇ�A���S�����True�ɂ��A���S�A�j���[�V�������Đ�
            if (this.gameObject.tag == playerTag)
            {
                Vector3 direction = (this.gameObject.transform.position - collision.gameObject.transform.position).normalized;

                player.TheDie(1, direction);

                isDead = true;
            }
        }
        else
        {
            isDead = false;
        }
    }

    //���S����ɏՓ˂����ꍇ�i�����j
    private void OnTriggerEnter(Collider other)
    {
        //���S����ƏՓ˂����ꍇ
        if (other.CompareTag(deadColTag))
        {
            //�v���C���[�̏ꍇ���S����͌��ɂȂ�
            if (this.gameObject.tag == playerTag)
            {
                //�J������^�ォ�牺�����ɓ��˂��A���ɗ����Ă���Ƃ��낪������悤�ɂ���
                cameraFollower.ZoomToTarget(-5, 0, new Vector3(0, -10, 0));

                //���S���\�b�h�Ăяo��
                player.TheDie(0, new Vector3(0, 0, 0));

                //���S�����True��
                isDead = true;
            }
        }

        //�R�C���ƏՓ˂����ꍇ
        if (other.CompareTag(coinTag))
        {
            //�R�C���擾���̃G�t�F�N�g���o��
            if (this.gameObject.tag == playerTag)
            {
                player.PlayCoinEffect();
            }
        }

        if (other.CompareTag("Heart"))
        {
            player.LifeUP();
            Debug.Log("lifeup");
        }
    }
}
