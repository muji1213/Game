using UnityEngine;
using UnityEngine.UI;

//���̃X�N���v�g�̓V���b�v�̍w���Ɋւ���X�N���v�g�ł�
public class ShopManager : SceneChanger
{
    [Header("�|�C���g��\������e�L�X�g")] [SerializeField] Text pointText;
    [Header("�O���b�h�p�l��")] [SerializeField] GameObject itemPanel;
    [Header("��������q�v�f")] [SerializeField] GameObject itemPrefab;

    [Header("BGM")] [SerializeField] AudioClip bgm;
    [Header("�w�����̉�")] [SerializeField] AudioClip buySE;
    [Header("�|�C���g������Ȃ��Ƃ��̉�")] [SerializeField]AudioClip blockSE;

    //�O�̃|�C���g
    private int prePoint;

    private void Awake()
    {
        //0�Ԃ͂��Ƃ��珊�����Ă���̂œ���Ȃ�
        for (int i = 1; i < ItemManager.items.Count; i++)
        {
            //�p�l���𐶐�
            CreateItemPanel(i);
        }
    }

    private void Start()
    {
        //BGM
        BGMManager.bgmManager.PlayBgm(bgm);
    }

    private void CreateItemPanel(int num)
    {
        //�p�l���̂ЂȌ`�𐶐�
        GameObject item = Instantiate(itemPrefab);

        //�q�I�u�W�F�N�g�̃{�^�����擾
        Button buyButton = item.transform.Find("BuyButton").transform.GetChild(0).GetComponent<Button>();

        //�q�I�u�W�F�N�g�̃{�^���̃e�L�X�g���擾
        Text buyButtonText = buyButton.transform.GetChild(1).GetComponent<Text>();

        //�q�I�u�W�F�N�g�̔���؂�\�����擾
        GameObject soldOut = item.transform.Find("SoldOut").gameObject;

        //�p�l����e�Ɂi�O���b�h���C�A�E�g�����Ă���j
        item.transform.SetParent(itemPanel.transform);

        //�ʒu��傫���𒲐�
        item.transform.localScale = new Vector3(1, 1, 1);
        item.transform.localPosition = new Vector3(0, 0, 0);

        //�摜�A���O�A�t���[�o�[�e�L�X�g�A�l�i��ݒ�
        item.transform.Find("Image/FrisbeeImage").GetComponent<Image>().sprite =
                                           ItemManager.itemManager.GetItem(num).Image;
        item.transform.Find("Text/FrisName").GetComponent<Text>().text =
                                            ItemManager.itemManager.GetItem(num).Name;
        item.transform.Find("Text/FrisInfo").GetComponent<Text>().text =
                                           ItemManager.itemManager.GetItem(num).Info;
        item.transform.Find("Text/Price").GetComponent<Text>().text =
                                           "�l�_���F" + ItemManager.itemManager.GetItem(num).Price.ToString() + "�|�C���g";


        //�{�^���ɏ�����ǉ�
        buyButton.onClick.AddListener(() => BuyItem(num, soldOut));

        //�{�^���̃e�L�X�g��ݒ�
        buyButtonText.text = "����";

        //���������łɏ������Ă���Ȃ甄��؂�\����
        if (ItemManager.itemManager.GetItemFlag(num))
        {
            soldOut.SetActive(true);
        }
        //�����łȂ��Ȃ��\����
        else
        {
            soldOut.SetActive(false);
        }
    }

    //�A�C�e���}�l�[�W���[�ɓo�^
    //ItemManager��Dictionary�ŃA�C�e�����Ǘ�
    //�w������ƁA�A�C�e���}�l�[�W���[�̏����t���O��ON�ɂȂ�
    public void BuyItem(int num, GameObject soldOut)
    {
        //�{�^���̐e�̃I�u�W�F�N�g���擾�i�A�C�e���̃X�N���v�g�����Ă���j

        //�A�ő΍�
        if (ItemManager.itemManager.GetItemFlag(num))
        {
            Debug.Log("���łɏ������Ă��܂�");
        }
        else if (GameManager.gameManager.point < ItemManager.itemManager.GetItem(num).Price)
        {
            SEManager.seManager.PlaySe(blockSE);
            Debug.Log("�����|�C���g������܂���");
        }
        else
        {
            //SE
            SEManager.seManager.PlaySe(buySE);

            //����؂�\��������
            soldOut.SetActive(true);

            //�����|�C���g������
            GameManager.gameManager.point -= ItemManager.itemManager.GetItem(num).Price;

            //�����ĂȂ��Ȃ�A�t���O��ON��
            ItemManager.itemManager.ActiveItemFlag(num);
        }
    }

    void Update()
    {
        //�O��ƃ|�C���g���ς�����������e�L�X�g��ύX����
        if (prePoint != GameManager.gameManager.point)
        {
            pointText.text = GameManager.gameManager.point.ToString();
            prePoint = GameManager.gameManager.point;
        }
    }
}

