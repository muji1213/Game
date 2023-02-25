using UnityEngine;
using UnityEngine.SceneManagement;

//�V�[���J��
//�}�l�[�W���[�n�͂�����p������
public class SceneChanger : MonoBehaviour
{
    [SerializeField] [Header("���艹�̉���")] [Range(0, 1)] float inputSEVol = 1;
    [SerializeField] [Header("�L�����Z�����̉���")] [Range(0, 1)] float cancelSEVol = 1;

    //�X�e�[�W�Z���N�g�ɑJ�ڂ���
    public virtual void ToStageSelectScene()
    {
        //SE��炷
        SEManager.seManager.PlaySE(inputSEVol, Resources.Load<AudioClip>("Sound/SE/SE_Input"));

        //�|�[�Y�Ȃǂ���J�ڂ��邱�Ƃ����邽�߁A�Q�[���X�s�[�h�͖߂�
        Time.timeScale = 1.0f;

        //�J��
        GameManager.gameManager.NextScene("StageSelect");
        SceneManager.LoadScene("StageSelect");
    }

    //�^�C�g����ʂɑJ�ڂ���
    public void ToTitleScene()
    {
        //SE��炷
        SEManager.seManager.PlaySE(cancelSEVol, Resources.Load<AudioClip>("Sound/SE/SE_InputCancel"));
       
        //�|�[�Y�Ȃǂ���J�ڂ��邽�߁A�Q�[���X�s�[�h�͖߂�
        Time.timeScale = 1.0f;

        //�J��
        GameManager.gameManager.NextScene("Title");
        SceneManager.LoadScene("Title");
    }


    //�����V�[���֔��
    public void ToInstructionScene()
    {
        //SE��炷
        SEManager.seManager.PlaySE(inputSEVol, Resources.Load<AudioClip>("Sound/SE/SE_Input"));
        GameManager.gameManager.NextScene("Instruction");
        SceneManager.LoadScene("Instruction");
    }
}
