using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultUI : SceneChanger
{
    [Header("�X�e�[�W�}�l�[�W���[")] [SerializeField] StageManager stageManager;
    [Header("���߂��|�C���g�e�L�X�g")] [SerializeField] Text getPointText;
    [Header("�m�[�_���[�W�{�[�i�X�e�L�X�g")] [SerializeField] Text nodamageText;
    [Header("�ő�HP�{�[�i�X�e�L�X�g")] [SerializeField] Text maxHPText;
    [Header("�g�[�^���|�C���g")] [SerializeField] Text totalPointText;

    [Header("�m�[�_���[�W�̎��̉��_")] [SerializeField] int nodamagePoint;
    [Header("�ő�HP�ɂȂ�܂ő��s�������̉��_")] [SerializeField] int maxHPPoint;
    
    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    //�|�C���g��ݒ�
    public void SetPoint()
    {
        //�X�e�[�W���ł��߂��|�C���g����
        getPointText.text = stageManager.temporarilypoint.ToString();

        //�m�[�_���[�W�Ȃ�
        if (stageManager.isNodamage)
        {
            //�m�[�_���[�W�e�L�X�g�ɐ�������
            nodamageText.text = nodamagePoint.ToString();
        }
        else
        {
            //�_���[�W���󂯂Ă�����0�|�C���g
            nodamageText.text = 0.ToString();
            nodamagePoint = 0;
        }

        //�ő�HP�ɂȂ�܂ő�������
        if (stageManager.isMaxHP)
        {
            //�}�b�N�XHP�e�L�X�g�ɐ�������
            maxHPText.text = maxHPPoint.ToString();
        }
        else
        {
            //�����łȂ��Ȃ�0�|�C���g
            maxHPText.text = 0.ToString();
            maxHPPoint = 0;
        }

        //�g�[�^����\��
        totalPointText.text = (nodamagePoint + maxHPPoint + stageManager.temporarilypoint).ToString() + "�|�C���g";
    }

     public override void ToStageSelectScene()
    {
        //�X�e�[�W�}�l�[�W���[�ɉ��Z
        stageManager.StorePoint(nodamagePoint + maxHPPoint);

        //�Q�[���}�l�[�W���[�ɉ��Z
        stageManager.AddPoint();

        //�Q�[�����Ԃ�߂�
        Time.timeScale = 1.0f;

        //�X�e�[�W�Z���N�g��
        SceneManager.LoadScene("StageSelect");
    }
}
