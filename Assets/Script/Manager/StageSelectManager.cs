using UnityEngine;
using System.Collections.Generic;

//�X�e�[�W�I����ʂɊւ���X�N���v�g�ł�
public class StageSelectManager : SingleTon<StageSelectManager>
{
    
    //�������������ǂ����A�V�[���Ԃŕێ������
    private static bool isInicialized = false;

    //�X�e�[�W�̃f�B�N�V���i��
    private static Dictionary<int, Stage> stages;

    //�S�ẴX�e�[�W���N���A�����t���O
    private static bool isAllStageCleared = false;

    protected override void Init()
    {
        //����������Ă��邩�ǂ���
        if (!isInicialized)
        {
            I = this;

            stages = new Dictionary<int, Stage>();

            //�X�e�[�W0
            string stage0 = "�R�ɃJ�R�܂�Ă���N�������W���E�B�J�P�_�V�̃t���X�r�X�g�̓R�R�Ń����V���E���I";
            stages.Add(0, new Stage(0, "�N�������W���E", stage0, Resources.Load<Sprite>("Stage0_Image"), false, false));

            //�X�e�[�W1
            string stage1 = "�J�[�����ꂭ�邤�L���E�R�N�B�C�����ނ������ŁA�A���������ł����������ς�";
            stages.Add(1, new Stage(1, "�{�E�t�E�̃L���E�R�N", stage1, Resources.Load<Sprite>("Stage1_Image"), false, false));

            //�X�e�[�W2
            string stage2 = "�E�`���E��������B�������J���_���J�����I�u���b�N�z�[���Ƀ`���E�C���I";
            stages.Add(2, new Stage(2, "�E�`���E", stage2, Resources.Load<Sprite>("Stage2_Image"), false, false));

            isInicialized = true;
        }
    }


    //�S�ẴX�e�[�W���N���A�������ǂ���
    //���ׂăN���A���Ă�����true
    //�N���A���Ă��Ȃ��X�e�[�W������A�������͑S�N���A�̃t���O�����낳��Ă���ꍇ�Afalse�ɂȂ�
    public bool CheckAllStageCleared()
    {
        if (isAllStageCleared)
        {
            return false;
        }

        int i = 0;
        while (true)
        {
            //�N���A���Ă��Ȃ������烋�[�v�𔲂���
            if (!isStageCleared(i))
            {
                //�N���A���Ă��Ȃ��X�e�[�W������Ȃ�false
                break;
            }

            //�S�ĉ��؂�����true��Ԃ�
            if (i == stages.Count - 1)
            {
                isAllStageCleared = true;
                return true;
            }
            i++;
        }

        return false;
    }

    //�X�e�[�W��1��ł����ꂵ����
    public bool isStageEntered(int num)
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
    public bool isStageCleared(int num)
    {
        return GetStage(num).Completed;
    }

    //�X�e�[�W�N���A�̃t���O�𗧂Ă�
    //num�͓o�^���̃X�e�[�W�ԍ��ɑΉ�����
    public void AvtiveStageClearFlag(int num)
    {
        GetStage(num).Completed = true;
    }

    public bool isPreStageCleared(int num)
    {
        if (isStageCleared(num - 1))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //�f�B�N�V���i���̃X�e�[�W���擾����
    //num�͓o�^���̃X�e�[�W�ԍ��ɑΉ�����
    public Stage GetStage(int stageNum)
    {
        return stages[stageNum];
    }

    public int StageCount()
    {
        return stages.Count;
    }
}
