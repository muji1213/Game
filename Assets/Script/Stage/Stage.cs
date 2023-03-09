using UnityEngine;

//�X�e�[�W�̃X�e�[�^�X��ݒ肵�܂�
public class Stage
{
    private int stageNum; //�X�e�[�W�ԍ�
    private string stageInfo;�@//�X�e�[�W�̏��
    private string stageName; //�X�e�[�W��
    private Sprite stageImage; //�X�e�[�W�̉摜
    private bool isEntered; //�������t���O
    private bool isCompleted; //�N���A�t���O

    public Stage(int stageNum, string stageName, string stageInfo, Sprite stageImage, bool isEntered, bool isCompleted)
    {
        this.stageNum = stageNum;
        this.stageInfo = stageInfo;
        this.stageName = stageName;
        this.stageImage = stageImage;
        this.isEntered = isEntered;
        this.isCompleted = isCompleted;
    }

    //�X�e�[�W�ԍ����擾
    public int StageNum
    {
        get { return stageNum; }
    }

    //�X�e�[�W�����擾
    public string StageInfo
    {
        get { return stageInfo; }
    }

    //�X�e�[�W�����擾
    public string StageName
    {
        get { return stageName; }
    }

    //�X�e�[�W�̉摜���擾
    public Sprite StageImage
    {
        get { return stageImage; }
    }

    //�X�e�[�W�ɓ���ς݂��ǂ���
    public bool Entered
    {
        get
        {
            return isEntered;
        }
        set
        {
            isEntered = value;
        }
    }

    //�X�e�[�W���U���ς݂��ǂ���
    public bool Completed
    {
        get
        {
            return isCompleted;
        }
        set
        {
            isCompleted = value;
        }
    }
}
