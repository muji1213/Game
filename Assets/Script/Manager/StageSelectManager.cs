using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

//�X�e�[�W�I����ʂɊւ���X�N���v�g�ł�
public class StageSelectManager : SceneChanger
{
    //�X�^�e�B�b�N�̓V�[���Ԃł���񂪎����Ȃ�
    public static StageSelectManager stageSelectManager = null;

    // eventSystem���擾���邽�߂̕ϐ��錾
    [Header("�C�x���g�V�X�e��")] [SerializeField] EventSystem eventSystem;

    [Header("�X�e�[�W�֍s���ۂ̃_�C�A���O")] [SerializeField] GameObject startSelectUI;
    [Header("�X�^�[�g�{�^��")] [SerializeField] private Button startButton;
    [Header("�t���X�r�[�I���̃{�^���v���n�u")] [SerializeField] GameObject selectButtonPrefab;
    [Header("�A�C�e���p�l��")] [SerializeField] GameObject itemPanel;
    [Header("�t���X�r�[�̃t���[�o�[�e�L�X�g�\���p�e�L�X�g")] [SerializeField] Text frisInfoText;
    [Header("�X�e�[�W���e�L�X�g")] [SerializeField] Text stageNameText;
    [Header("�X�e�[�W�����e�L�X�g")] [SerializeField] Text stageInfoText;
    [Header("�I�𒆂̃X�e�[�W�摜�\��")] [SerializeField] Image stageSprite;
    [Header("�X�e�[�W�I���{�^���̂ЂȌ`")] [SerializeField] GameObject stageSelectButtonPrefab;
    [Header("�p�l��")] [SerializeField] GameObject stagePanel;
    [Header("�S�ẴX�e�[�W���N���A�����ۂɕ\������UI")] [SerializeField] GameObject allClearUI;
    [Header("�I�𒆂̃t���X�r�[�̏�ɕ\��������")] [SerializeField] GameObject arrow;
    [Header("�X�e�[�W�V�[���ւ������߂̃{�^��")] [SerializeField] Button goStageButton;

    [Header("BGM")] [SerializeField] AudioClip bgm;
    [Header("�{�^����������SE")] [SerializeField] AudioClip inputSE;
    [Header("�{�^��SE�̉���")] [SerializeField] [Range(0, 1)] float inputSEVol = 1;
    [Header("�L�����Z������SE")] [SerializeField] AudioClip cancelSE;
    [Header("�L�����Z��SE�̉���")] [SerializeField] [Range(0, 1)] float canselSEVol = 1;

    //�������������ǂ����A�V�[���Ԃŕێ������
    private static bool isInicialized = false;

    //�X�e�[�W�̃f�B�N�V���i��
    public static Dictionary<int, Stage> stages;

    //�I�����ꂽ�X�e�[�W�ԍ����i�[����
    public static int selectedStageNum;

    //�X�e�[�W�Z���N�g�őI�΂ꂽ�t���X�r�[
    public static FrisbeeItem selectedFrisbee;

    //�S�ẴX�e�[�W���N���A�����t���O
    private static bool isAllStageCleared = false;

    private void Awake()
    {
        //����������Ă��邩�ǂ���
        if (!isInicialized)
        {
            stageSelectManager = this;

            stages = new Dictionary<int, Stage>();

            //�X�e�[�W0
            string stage0 = "�R�ɃJ�R�܂�Ă���N�������W���E�B�J�P�_�V�̃t���X�r�X�g�̓R�R�Ń����V���E���I";
            stages.Add(0, new Stage(0, "�N�������W���E", stage0, Resources.Load<Sprite>("Stage0_Image"), false, true));

            //�X�e�[�W1
            string stage1 = "�J�[�����ꂭ�邤�L���E�R�N�B�C�����ނ������ŁA�A���������ł����������ς�";
            stages.Add(1, new Stage(1, "�{�E�t�E�̃L���E�R�N", stage1, Resources.Load<Sprite>("Stage1_Image"), false, true));

            //�X�e�[�W2
            string stage2 = "�E�`���E��������B�������J���_���J�����I�u���b�N�z�[���Ƀ`���E�C���I";
            stages.Add(2, new Stage(2, "�E�`���E", stage2, Resources.Load<Sprite>("Stage2_Image"), false, false));

            isInicialized = true;
        }
    }

    private void Start()
    {
        //�X�e�[�W�I�����̊m�F�_�C�A���O��OFF
        startSelectUI.SetActive(false);

        //�X�e�[�W�I���{�^���̓X�e�[�W�����߂đI�������܂�OFF
        startButton.interactable = false;

        //�t���X�r�[�����e�L�X�g��OFF
        frisInfoText.text = null;

        //�o�^���Ă���X�e�[�W�������{�^���𐶐�
        for (int i = 0; i < stages.Count; i++)
        {
            CreateStageSelectButton(i);
        }

        //�S�ẴX�e�[�W���N���A�������ǂ����`�F�b�N
        if (!isAllStageCleared)
        {
            //�S�N�����Ă�����
            if (CheckAllStageCleared())
            {
                //���o���o��
                allClearUI.SetActive(true);

                //static�ȃt���O������
                isAllStageCleared = true;
            }
        }

        BGMManager.bgmManager.PlayBgm(bgm);
    }

    //�S�ẴX�e�[�W���N���A�������ǂ���
    //���ׂăN���A���Ă�����true
    private bool CheckAllStageCleared()
    {
        int i = 0;
        while (true)
        {
            //�N���A���Ă��Ȃ������烋�[�v�𔲂���
            if (!GetStageClearFlag(i))
            {
                //�N���A���Ă��Ȃ��X�e�[�W��_�ł�����
                stagePanel.transform.GetChild(i).GetComponent<Animator>().Play("Flash");
                break;
            }

            //�S�ĉ��؂�����true��Ԃ�
            if (i == stages.Count - 1)
            {
                return true;
            }
            i++;
        }

        return false;
    }


    //�V���b�v�̃V�[���֔��
    public void ToShopScene()
    {
        SEManager.seManager.PlaySE(inputSEVol, inputSE);
        GameManager.gameManager.NextScene("Shop");
        SceneManager.LoadScene("Shop");
    }

    //�X�e�[�W�ɒ��킷�邩�m�F����UI��\��
    public void ActiveStartUI()
    {
        //�������̃t���X�r�[��S�Ē��ׂ�
        foreach (FrisbeeItem frisbeeItem in ItemManager.items.Values)
        {
            //���������ǂ�������
            if (ItemManager.itemManager.GetItemFlag(frisbeeItem.Num))
            {
                //���łɃp�l��������Ă���Ȃ�A�폜���č�蒼��
                CheckChild(frisbeeItem.Name);
                CreateFrisbeeSelectButton(frisbeeItem);
            }
        }

        SEManager.seManager.PlaySE(inputSEVol, inputSE);
        //UI��������悤�ɂ���
        startSelectUI.SetActive(true);
    }

    //�p�l���̎q�I�u�W�F�N�g�𒲂ׂ�B(�������̃t���X�r�[���ׂ�)
    //�������O�̃t���X�r�[�̃{�^��������Ȃ�폜����
    private void CheckChild(string name)
    {
        foreach (Transform child in itemPanel.transform)
        {
            if (name == child.name)
            {
                Destroy(child.gameObject);
                return;
            }
        }
        return;
    }

    //�X�e�[�W��1��ł����ꂵ����
    public bool GetStageEnteredFlag(int num)
    {
        return GetStage(num).Entered;
    }

    //�X�e�[�W����t���O�𗧂Ă�
    public void ActiveStageEnteredFlag(int num)
    {
        GetStage(num).Entered = true;
    }

    //�X�e�[�W���N���A���Ă��邩
    //num�͓o�^���̃X�e�[�W�ԍ��ɑΉ�����
    public bool GetStageClearFlag(int num)
    {
        return GetStage(num).Completed;
    }

    //�X�e�[�W�N���A�̃t���O�𗧂Ă�
    //num�͓o�^���̃X�e�[�W�ԍ��ɑΉ�����
    public void AvtiveStageClearFlag(int num)
    {
        GetStage(num).Completed = true;
    }

    //�f�B�N�V���i���̃X�e�[�W���擾����
    //num�͓o�^���̃X�e�[�W�ԍ��ɑΉ�����
    private Stage GetStage(int stageNum)
    {
        return stages[stageNum];
    }


    //�X�e�[�W�Z���N�g�̃{�^���𐶐�
    private void CreateStageSelectButton(int stageNum)
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

        //�ЂƂO�̃X�e�[�W�����N���A�Ȃ疳���ɂ���
        if (stageNum != 0)
        {
            if (!GetStageClearFlag(stageNum - 1))
            {
                button.interactable = false;
            }
        }

        //������ǉ�
        button.onClick.AddListener(() => DisplayStageInfo(stageNum));
    }

    //�{�^�����N���b�N�����ہA�X�e�[�W����UI�ɕ\������
    public void DisplayStageInfo(int stageNum)
    {
        SEManager.seManager.PlaySE(inputSEVol, inputSE);
        //�I�����ꂽ�X�e�[�W
        Stage selectedStage = GetStage(stageNum);

        //�����擾
        stageNameText.text = selectedStage.StageName;
        stageInfoText.text = selectedStage.StageInfo;
        stageSprite.sprite = selectedStage.StageImage;

        //�X�^�[�g�{�^����������悤��
        startButton.interactable = true;

        //�X�e�[�W�ԍ���������
        selectedStageNum = selectedStage.StageNum;
    }

    //�t���X�r�[�̑I���{�^���𐶐�
    private void CreateFrisbeeSelectButton(FrisbeeItem frisbeeItem)
    {
        //�{�^���𐶐�
        GameObject selectButton = Instantiate(selectButtonPrefab);

        //Grid layout�̎q���ɂ���(ItemPanel��Grid)
        selectButton.transform.SetParent(itemPanel.transform);

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
        SEManager.seManager.PlaySE(inputSEVol, inputSE);

        //�t���X�r�[�̓�����\��
        frisInfoText.text = frisbeeItem.Info;

        //�I�΂�Ă���t���X�r�[����
        selectedFrisbee = frisbeeItem;
    }


    //�X�e�[�W�ɒ��킷�邩�m�F����UI������
    public void DeactiveStartUI()
    {
        arrow.SetActive(false);
        SEManager.seManager.PlaySE(canselSEVol, cancelSE);
        startSelectUI.SetActive(false);
    }

    //�X�e�[�W�ɂ�������
    //�t���X�r�[��I�����Ă��Ȃ��ꍇ�A�s���Ȃ�
    public void ToStageScene()
    {
        if (selectedFrisbee == null)
        {
            return;
        }
        GameManager.gameManager.NextScene("Stage" + selectedStageNum);
        SceneManager.LoadScene("Stage" + selectedStageNum);
    }
}
