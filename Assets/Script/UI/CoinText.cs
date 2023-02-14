using UnityEngine;
using UnityEngine.UI;

//�X�e�[�W���̎擾�|�C���g��\������
public class CoinText : MonoBehaviour
{
    //�X�R�A��\������e�L�X�g
    [Header("�X�R�A�\���e�L�X�g")][SerializeField]private Text pointText;

    //�X�e�[�W�}�l�[�W���[
    private StageManager stageManager;
    
    void Start()
    {
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        this.gameObject.SetActive(false);
    }

    //�X�e�[�W���Ŏ擾�����|�C���g��\������
    //�|�C���g�̓X�e�[�W�}�l�[�W���[���ێ����Ă���
    void Update()
    {
        pointText.text =�@stageManager.temporarilypoint.ToString();
    }
}
