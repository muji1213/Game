using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    // eventSystem���擾���邽�߂̕ϐ��錾
    [Header("�C�x���g�V�X�e��")] [SerializeField] EventSystem eventSystem;

    [Header("------�X�e�[�W�Z���N�g��ʂɊւ���ݒ�---------")]
    [Header("�t���X�r�[�I����ʂ�\������{�^��")] [SerializeField] private Button displayFrisbeeSelectButton;
    [Header("�t���X�r�[�I���̃{�^���v���n�u")] [SerializeField] GameObject selectButtonPrefab;
    [Header("�X�e�[�W���e�L�X�g")] [SerializeField] Text stageNameText;
    [Header("�X�e�[�W�t���[�o�[�e�L�X�g")] [SerializeField] Text stageInfoText;
    [Header("�I�𒆂̃X�e�[�W�摜�\��")] [SerializeField] Image stageSprite;
    [Header("�X�e�[�W�I���{�^���̃v���n�u")] [SerializeField] GameObject stageSelectButtonPrefab;
    [Header("�X�e�[�W�{�^���p�l��")] [SerializeField] GameObject stagePanel;
    [Header("�S�ẴX�e�[�W���N���A�����ۂɕ\������UI")] [SerializeField] GameObject allClearUI;

    [Header("------�t���X�r�[�I����ʂɊւ���ݒ�--------")]
    [Header("�t���X�r�[�p�l��")] [SerializeField] GameObject frisbeeSelectPanel;
    [Header("�t���X�r�[�I�����")] [SerializeField] GameObject startSelectUI;
    [Header("�I�𒆂̃t���X�r�[�̏�ɕ\��������")] [SerializeField] GameObject arrow;
    [Header("�t���X�r�[�̃t���[�o�[�e�L�X�g�\���p�e�L�X�g")] [SerializeField] Text frisInfoText;
    [Header("�X�e�[�W�V�[���ւ������߂̃{�^��")] [SerializeField] Button goStageButton;

    [Header("---------���Ɋւ���ݒ�----------")]
    [Header("BGM")] [SerializeField] AudioClip bgm;
    [Header("�{�^����������SE")] [SerializeField] AudioClip inputSE;
    [Header("�{�^��SE�̉���")] [SerializeField] [Range(0, 1)] float inputSEVol = 1;
    [Header("�L�����Z������SE")] [SerializeField] AudioClip cancelSE;
    [Header("�L�����Z��SE�̉���")] [SerializeField] [Range(0, 1)] float cancelSEVol = 1;

    private void Start()
    {
        //�X�e�[�W�I�����̊m�F�_�C�A���O��OFF
        startSelectUI.SetActive(false);

        //�X�e�[�W�I���{�^���̓X�e�[�W�����߂đI�������܂�OFF
        displayFrisbeeSelectButton.interactable = false;

        //�t���X�r�[�����e�L�X�g��OFF
        frisInfoText.text = null;

        //�o�^���Ă���X�e�[�W�������{�^���𐶐�
        //���������X�e�[�W���N���A���Ă��Ȃ��Ȃ�A�_�ł�����
        for (int i = 0; i < StageSelectManager.I.StageCount(); i++)
        {
            //0�Ԗڂ͕K����������
            if (i == 0)
            {
                CreateStageSelectButton(i, StageSelectManager.I.IsStageCleared(0));
            }
            //1�Ԗڈȍ~
            else if (i >= 1)
            {
                //�ЂƂO�̃X�e�[�W���N���A���Ă��Ȃ��Ȃ琶�����Ȃ�
                if (!StageSelectManager.I.IsPreStageCleared(i))
                {
                    
                }
                //�N���A���Ă���Ȃ琶������
                else
                {
                    CreateStageSelectButton(i, StageSelectManager.I.IsStageCleared(i));
                }
            }
        }

        //�S�ẴX�e�[�W���N���A�������ǂ����`�F�b�N

        //�S�N�����Ă�����
        if (StageSelectManager.I.CheckAllStageCleared())
        {
            //���o���o��
            allClearUI.SetActive(true);
        }

        BGMManager.I.PlayBgm(bgm);
    }

    //�X�e�[�W�ɒ��킷�邩�m�F����UI��\��
    public void ActiveStartUI()
    {
        //�������̃t���X�r�[��S�Ē��ׂ�
        foreach (FrisbeeItem frisbeeItem in ItemManager.items.Values)
        {
            //���������ǂ�������
            if (ItemManager.I.GetItemFlag(frisbeeItem.Num))
            {
                //���łɃp�l��������Ă���Ȃ�A�폜���č�蒼��
                CheckChild(frisbeeItem.Name);
                CreateFrisbeeSelectButton(frisbeeItem);
            }
        }

        SEManager.I.PlaySE(inputSEVol, inputSE);

        //UI��������悤�ɂ���
        startSelectUI.SetActive(true);
    }

    //�p�l���̎q�I�u�W�F�N�g�𒲂ׂ�B(�������̃t���X�r�[���ׂ�)
    //�������O�̃t���X�r�[�̃{�^��������Ȃ�폜����
    private void CheckChild(string name)
    {
        foreach (Transform child in frisbeeSelectPanel.transform)
        {
            if (name == child.name)
            {
                Destroy(child.gameObject);
                return;
            }
        }
        return;
    }

    //�X�e�[�W�Z���N�g�̃{�^���𐶐�
    //����isCleared��false�Ȃ�_�ł�����
    private void CreateStageSelectButton(int stageNum, bool isCleared)
    {
        //�{�^���𐶐�
        GameObject selectStageButton = Instantiate(stageSelectButtonPrefab);

        //�p�l���ɕt����i�O���b�h���C�A�E�g�����Ă���j
        selectStageButton.transform.SetParent(stagePanel.transform);

        //�傫����ʒu����
        selectStageButton.transform.localPosition = new Vector3(0, 0, 0);
        selectStageButton.transform.localScale = new Vector3(1, 1, 1);

        //�{�^�����擾
        Button button = selectStageButton.transform.GetChild(0).GetComponent<Button>();

        //������ǉ�
        button.onClick.AddListener(() => DisplayStageInfo(stageNum));

        if (!isCleared)
        {
            selectStageButton.GetComponent<Animator>().Play("Flash");
        }
    }

    //�{�^�����N���b�N�����ہA�X�e�[�W����UI�ɕ\������
    public void DisplayStageInfo(int stageNum)
    {
        SEManager.I.PlaySE(inputSEVol, inputSE);

        //�I�����ꂽ�X�e�[�W
        Stage selectedStage = StageSelectManager.I.GetStage(stageNum);

        //�����擾
        stageNameText.text = selectedStage.StageName;
        stageInfoText.text = selectedStage.StageInfo;
        stageSprite.sprite = selectedStage.StageImage;

        //�X�^�[�g�{�^����������悤��
        displayFrisbeeSelectButton.interactable = true;

        //�X�e�[�W�ԍ���������
        GameManager.I.SelectedStageInfo = selectedStage;
    }

    //�t���X�r�[�̑I���{�^���𐶐�
    private void CreateFrisbeeSelectButton(FrisbeeItem frisbeeItem)
    {
        //�{�^���𐶐�
        GameObject selectButton = Instantiate(selectButtonPrefab);

        //Grid layout�̎q���ɂ���(ItemPanel��Grid)
        selectButton.transform.SetParent(frisbeeSelectPanel.transform);

        //�傫����ʒu�𒲐�
        selectButton.name = frisbeeItem.Name;
        selectButton.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        selectButton.transform.localPosition = new Vector3(0f, 0f, 0f);

        //�摜��ݒ�
        selectButton.GetComponent<Image>().sprite = frisbeeItem.Image;

        //�������폜
        selectButton.transform.GetChild(0).GetComponent<Text>().text = null;

        //�{�^���ɏ�����ǉ�
        selectButton.GetComponent<Button>().onClick.AddListener(() => GetSelectedFrisbeeInfo(frisbeeItem));

        //�X�e�[�W�V�[���֍s���{�^���͔�A�N�e�B�u��
        goStageButton.interactable = false;

        //�t���X�r�[��񕶂�������
        frisInfoText.text = "�t���X�r�[��I�����Ă�������";
    }

    //�I�����ꂽ�t���X�r�[�̏���\������
    public void GetSelectedFrisbeeInfo(FrisbeeItem frisbeeItem)
    {
        if (!arrow.activeSelf)
        {
            arrow.SetActive(true);
        }

        //�X�e�[�W�V�[���֍s���{�^�����A�N�e�B�u��
        goStageButton.interactable = true;

        //����\������
        arrow.transform.localPosition = eventSystem.currentSelectedGameObject.transform.localPosition;

        //SE
        SEManager.I.PlaySE(inputSEVol, inputSE);

        //�t���X�r�[�̓�����\��
        frisInfoText.text = frisbeeItem.Info;

        //�I�΂�Ă���t���X�r�[����
        GameManager.I.SelectedFrisbeeInfo = frisbeeItem;
    }

    //�X�e�[�W�ɒ��킷�邩�m�F����UI������
    public void DeactiveStartUI()
    {
        //��������
        arrow.SetActive(false);

        //SE��炷
        SEManager.I.PlaySE(cancelSEVol, cancelSE);

        //�t���X�r�[�I����ʂ�����
        startSelectUI.SetActive(false);

        //�I�𒆂̃t���X�r�[����������
        GameManager.I.SelectedFrisbeeInfo = null;

        //�I�𒆂̃X�e�[�W����������
        GameManager.I.SelectedStageInfo = null;
    }

    //�X�e�[�W�ɂ�������
    //�t���X�r�[��I�����Ă��Ȃ��ꍇ�A�s���Ȃ�
    public void ToStageScene()
    {
        if (GameManager.I.SelectedFrisbeeInfo == null)
        {
            return;
        }

        SceneChanger.I.ToStageScene(GameManager.I.SelectedStageInfo.StageNum);
    }

    //�V���b�v�̃V�[���֔��
    public void ToShopScene()
    {
        SEManager.I.PlaySE(inputSEVol, inputSE);
        SceneChanger.I.ToShopScene();
    }

    //�^�C�g����
    public void ToTitleScene()
    {
        SEManager.I.PlaySE(cancelSEVol, cancelSE);
        SceneChanger.I.ToTitleScene();
    }
}
