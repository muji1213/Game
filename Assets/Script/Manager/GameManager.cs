using UnityEngine;

public class GameManager : SingleTon<GameManager>
{
    //�X�e�[�W�Ŏ擾�����|�C���g��ێ�����
    //�f�o�b�O�p��[hide]�ɂ��Ȃ�����
    private int point;

    //�X�e�[�W�Z���N�g�őI�񂾃t���X�r�[
    private FrisbeeItem selectedFrisbee;

    //�I�������X�e�[�W
    private Stage selectedStage;


    public int Point
    {
        set
        {
            point = value;
        }
        get
        {
            return point;
        }
    }

    public FrisbeeItem SelectedFrisbeeInfo
    {
        set
        {
            selectedFrisbee = value;
        }
        get
        {
            return selectedFrisbee;
        }
    }

    public Stage SelectedStageInfo
    {
        set
        {
            selectedStage = value;
        }
        get
        {
            return selectedStage;
        }
    }
}
