using UnityEngine;


//���̃X�N���v�g�̓R�C�����擾�����ۂ̃X�N���v�g��
public class Coin : MonoBehaviour
{
    private int score = 200;//�擾�|�C���g

    //�t���X�r�[�̃^�O
    private string frisbeeTag = "Frisbee";

    //�v���C���[�^�O
    private string playerTag = "Player";

    //�v���C���[�������̓t���X�r�[���G�ꂽ�ꍇ
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(frisbeeTag) || other.CompareTag(playerTag))
        {
            //���ł�����
            Destroy(this.gameObject);
        }
    }

    //�R�C���擾���̃X�R�A��Ԃ�
    public int Score
    {
        get
        {
            return score;
        }
    }
}
