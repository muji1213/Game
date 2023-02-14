using UnityEngine;


//���̃X�N���v�g�̓R�C�����擾�����ۂ̃X�N���v�g��
public class Coin : MonoBehaviour
{
    private int point = 200;//�擾�|�C���g

    // �X�e�[�W�}�l�[�W���[
    private StageManager stageManager;

    //�t���X�r�[�̃^�O
    private string frisbeeTag = "Frisbee";

    //�v���C���[�^�O
    private string playerTag = "Player";

    void Start()
    {
        //�X�e�[�W�}�l�[�W���[
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
    }

    //�v���C���[�������̓t���X�r�[���G�ꂽ�ꍇ
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(frisbeeTag) || other.CompareTag(playerTag))
        {
            //�X�e�[�W�}�l�[�W���[�̃|�C���g�𑝂₷
            stageManager.StorePoint(point);

            //���̌���ł�����
            Destroy(this.gameObject);
        }
    }
}
