using UnityEngine;
using UnityEngine.UI;


public class InstructionManager : SceneChanger
{
    [Header("����p�p�l��")] [SerializeField] GameObject panel;
    [Header("�p�l���̑傫��")] [SerializeField] float panelWidth;
    [Header("���Ɉړ�����{�^��")] [SerializeField] GameObject goPreButton;
    [Header("�E�Ɉړ�����{�^��")] [SerializeField] GameObject goNextButton;

    [Header("BGM")] [SerializeField] AudioClip bgm;
    [Header("���E�{�^������SE")] [SerializeField] AudioClip buttonSE;

    //�p�l���̐�
    private int panelNum;

    //�O���b�h�̃Z���T�C�Y
    private float celWidth;

    //���݌��Ă���p�l��
    private int index;

    //�p�l���̌��ݍ��W
    private float currentPanelPosX;

    //�p�l���̎��̍��W
    private float nextPanelPosX;


    void Start()
    {
        //BGM
        BGMManager.bgmManager.PlayBgm(bgm);

        //�p�l�����𐔂���
        panelNum = panel.transform.childCount;

        //�C���f�b�N�X��0��
        index = 0;

        //��ԍŏ��̃p�l������ʂɉf��
        currentPanelPosX = 0;

        //�Z���T�C�Y���擾
        celWidth = panel.GetComponent<GridLayoutGroup>().cellSize.x;
    }

    private void FixedUpdate()
    {
        //�ڕW�̍��W�܂ŏ��X�ɋ߂Â���
        currentPanelPosX = Mathf.Lerp(currentPanelPosX, nextPanelPosX, 0.1f);

        //�ڕW�n�_�܂Ńp�l�����ړ�
        panel.transform.localPosition = new Vector3(currentPanelPosX, 0, 0);

        //�ڕW���W�܂Ńp�l�����߂Â����猻�ݍ��W��ڕW���W�Ɠ����ɂ���(Lerp�͊��S�ꏏ�ɂȂ�Ȃ�)
        if (Mathf.Abs(currentPanelPosX - nextPanelPosX) < 0.01f)
        {
            currentPanelPosX = nextPanelPosX;
        }

        //�Ō�̃p�l���Ȃ玟�փ{�^���͉f���Ȃ�
        if(index == panelNum - 1)
        {
            goNextButton.SetActive(false);
        }
        //�ŏ��̃p�l���Ȃ�߂�{�^���͉f���Ȃ�
        else if(index == 0)
        {
            goPreButton.SetActive(false);
        }
        //����ȊO�Ȃ�f��
        else
        {
            goNextButton.SetActive(true);
            goPreButton.SetActive(true);
        }
    }

    public void MoveNextPanel()
    {
        //���ݍ��W�ƖڕW���W�������łȂ��Ȃ牟���Ȃ��悤�ɂ���(�A�ł����ƍ��W�������)
        if (currentPanelPosX != nextPanelPosX)
        {
            return;
        }

        index++;

        //�C���f�b�N�X���p�l�����𒴂��Ă����ꍇ�A�����Ȃ�
        if (index > panelNum - 1)
        {
            index = panelNum - 1;
            return;
        }
        else
        {
            //SE
            SEManager.seManager.PlaySe(buttonSE);

            //�ڕW���W��ݒ肷��
            nextPanelPosX = currentPanelPosX - (panelWidth + celWidth);
        }
    }

    public void MovePrePanel()
    {
        //���ݍ��W�ƖڕW���W�������łȂ��Ȃ牟���Ȃ��悤�ɂ���(�A�ł����ƍ��W�������)
        if (currentPanelPosX != nextPanelPosX)
        {
            return;
        }

        index--;

        //�C���f�b�N�X��0�ȉ��Ȃ�߂�
        if (index < 0)
        {
            index = 0;
            return;
        }
        else
        {
            //SE
            SEManager.seManager.PlaySe(buttonSE);

            //�ڕW���W��ݒ�
            nextPanelPosX = currentPanelPosX + (panelWidth + celWidth);
        }
    }

}
