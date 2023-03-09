using UnityEngine;


//���̃X�N���v�g�̓v���C���[����уt���X�r�[�̏Փ˔�����擾����X�N���v�g
public class Player_HurtBox : MonoBehaviour
{
    public enum DeadType
    {
        Fall = 0,
        Collide = 1
    }

    //�e��^�O

    //�v���C���[�^�O
    private string playerTag = "Player";

    //��Q���^�O
    private string obstacleTag = "Obstacle";

    //�R�C���^�O
    private string coinTag = "Coin";

    //���S����i���A�������̓S�[�����O�ꂽ�ꍇ�j
    private string deadColTag = "DeadCol";

    //���S���
    private DeadType type;

    private Player_Controller player; //�v���C���[�R���g���[���[�i������j
   
    private void Start()
    {
        //�e��R���|�[�l���g�擾

        //�v���C���[�i������j
        player = GetComponent<Player_Controller>();
    }

    //�Փ˔���

    //�t���X�r�[�͏�Q���Ƃ̏Փ˔���A����уS�[�������蔲���Ă��܂����ۂ̔�������
    //�v���C���[�͏�Q���Ƃ̏Փ˔���A����ь��ɗ������ۂ̔�������
    private void OnCollisionEnter(Collision collision)
    {
        //��Q���Ƃ̏Փ˔���
        if (collision.gameObject.tag == obstacleTag)
        {

            //��Q���Ƃ̏Փ˂��v���C���[�i������j�������ꍇ�A���S�����True�ɂ��A���S�A�j���[�V�������Đ�
            if (this.gameObject.tag == playerTag)
            {
                Vector3 direction = (this.gameObject.transform.position - collision.gameObject.transform.position).normalized;

                type = DeadType.Collide;

                player.TheDie(type, direction);
            }
        }
        else
        {
            
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
                type = DeadType.Fall;

                //���S���\�b�h�Ăяo��
                player.TheDie(type, new Vector3(0, 0, 0));
            }
        }

        //�R�C���ƏՓ˂����ꍇ
        if (other.CompareTag(coinTag))
        {
            int coinScore = other.GetComponent<Coin>().Score;

            Debug.Log("�R�C���擾");

            //�R�C���擾���̃G�t�F�N�g���o��
            player.GetCoin(coinScore);
        }

        //���C�t�A�b�v���������HP����ǉ�����
        if (other.CompareTag("Heart"))
        {
            player.LifeUP();
        }
    }
}
