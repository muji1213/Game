using UnityEngine;
using UnityEngine.SceneManagement;

//�V�[���J��
//�}�l�[�W���[�n�͂�����p������
public class SceneChanger : MonoBehaviour
{
   
    //�X�e�[�W�Z���N�g�ɑJ�ڂ���
    public virtual void ToStageSelect()
    {
        //SE��炷
        SEManager.seManager.PlaySe(Resources.Load<AudioClip>("Sound/SE/SE_Input"));

        //�|�[�Y�Ȃǂ���J�ڂ��邱�Ƃ����邽�߁A�Q�[���X�s�[�h�͖߂�
        Time.timeScale = 1.0f;

        //�J��
        GameManager.gameManager.NextScene("StageSelect");
        SceneManager.LoadScene("StageSelect");
    }

    //�^�C�g����ʂɑJ�ڂ���
    public void ReturnTitle()
    {
        //SE��炷
        SEManager.seManager.PlaySe(Resources.Load<AudioClip>("Sound/SE/SE_InputCancel"));

        //�|�[�Y�Ȃǂ���J�ڂ��邽�߁A�Q�[���X�s�[�h�͖߂�
        Time.timeScale = 1.0f;

        //�J��
        GameManager.gameManager.NextScene("Title");
        SceneManager.LoadScene("Title");
    }
}
