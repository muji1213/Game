using UnityEngine;

//���̃X�N���v�g�̓t���X�r�[�̃X�e�[�^�X��ݒ肵�܂�
public class FrisbeeItem
{
    private int level; //�t���X�r�[�̃��x��
    private int num; //�Ǘ��ԍ�
    private string frisname; //�t���X�r�[�̖��O
    private int price; //�l�i
    private Sprite frisSprite; //�t���X�r�[�̉摜���Z�b�g
    private GameObject Frisbee; //����ɑΉ�����t���X�r�[�̃v���n�u
    private bool isObtained;

    [TextArea] [SerializeField] private string frisInfo; //�t���X�r�[�̐�����

    public FrisbeeItem(int level, int num, string frisname, int price, Sprite frisSprite, string frisInfo, GameObject Frisbee, bool isObtained)
    {
        this.level = level;
        this.num = num;
        this.frisname = frisname;
        this.price = price;
        this.frisSprite = frisSprite;
        this.frisInfo = frisInfo;
        this.Frisbee = Frisbee;
        this.isObtained = isObtained;
    }

    //���O�擾
    public string Name
    {
        get { return frisname; }
    }

    //�l�i���擾
    public int Price
    {
        get { return price; }
    }

    //�t���X�r�[�̃t���[�o�[�e�L�X�g
    public string Info
    {
        get { return frisInfo; }
    }

    //�t���X�r�[�̉摜
    public Sprite Image
    {
        get { return frisSprite; }
    }

    //�t���X�r�[�̔ԍ�
    public int Num
    {
        get { return num; }
    }

    //�Ή�����t���X�r�[�̎擾
    public GameObject SelectedFrisbee
    {
        get { return Frisbee; }
    }

    //���x�����擾
    public int FrisbeeLevel
    {
        get { return level; }
    }

    //�������Ă��邩�ǂ���
    public bool Obtain
    {
        get
        {
            return isObtained;
        }
        set
        {
            isObtained = value;
        }
    }
}
