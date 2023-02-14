using UnityEngine;
using UnityEngine.UI;

//���̃X�N���v�g�͑��s���Ԃɉ����ăQ�[�W�����߂�
public class FrisbeeGauge : MonoBehaviour
{
    //�X�e�[�W�}�l�[�W���[
    private StageManager stageManager;

    //�Q�[�W�̃X�v���C�g
    [Header("�Q�[�W�̃X�v���C�g")] [SerializeField] private Image image;
    // Start is called before the first frame update
    void Start()
    {
        //�e��R���|�[�l���g
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        this.gameObject.SetActive(false);
    }

    //���s�����ɉ����ăQ�[�W�����߂�
    private void FixedUpdate()
    {
        //fillAmount�ŃQ�[�W��ω�������
        //�����̓X�e�[�W�}�l�[�W���[��GetRunTime����擾
        image.fillAmount = stageManager.GetRunTime();
    }
}
