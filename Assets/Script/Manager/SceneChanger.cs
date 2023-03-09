using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

//�V�[���J��
//�}�l�[�W���[�n�͂�����p������
public class SceneChanger : SingleTon<SceneChanger>
{
    private AsyncOperation async;

    //�J�ڐ�V�[���̖��O
    private string nextSceneName;

    [SerializeField]
    private GameObject loadUIPrefab;

    public void NextScene(string sceneName)
    {
        //�ǂݍ��ݒ��̓��[�hUI���o��
        GameObject loadUI = Instantiate(loadUIPrefab);

        loadUI.transform.SetParent(GameObject.Find("Canvas").transform);

        loadUI.transform.localPosition = new Vector3(0, 0, 0);
        loadUI.transform.localScale = new Vector3(1, 1, 1);

        nextSceneName = sceneName;

        //�@�R���[�`�����J�n
        StartCoroutine("LoadData");

        //�ړ�
        GoNextScene(sceneName);
    }

    //�V�[���֑J�ڂ���
    public void GoNextScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator LoadData()
    {
        // �V�[���̓ǂݍ��݂�����
        async = SceneManager.LoadSceneAsync(nextSceneName);

        //�ǂݍ��ݏI���܂ő҂�
        while (!async.isDone)
        {
            yield return null;
        }

        async.allowSceneActivation = true;
    }

    //�X�e�[�W�Z���N�g�ɑJ�ڂ���
    public void ToStageSelectScene()
    {
        //�|�[�Y�Ȃǂ���J�ڂ��邱�Ƃ����邽�߁A�Q�[���X�s�[�h�͖߂�
        Time.timeScale = 1.0f;
        NextScene("StageSelect");
    }

    //�^�C�g����ʂɑJ�ڂ���
    public void ToTitleScene()
    {
        //�|�[�Y�Ȃǂ���J�ڂ��邽�߁A�Q�[���X�s�[�h�͖߂�
        Time.timeScale = 1.0f;

        //�J��
        NextScene("Title");
    }

    //�����V�[���֔��
    public void ToInstructionScene()
    {
        NextScene("Instruction");
    }

    //�X�e�[�W�V�[���֑J�ڂ���
    public void ToStageScene(int stageNum)
    {
        NextScene("Stage" + stageNum);
    }

    //�V���b�v�V�[���֑J�ڂ���
    public void ToShopScene()
    {
        NextScene("Shop");
    }

    //���g���C
    public void Retry()
    {
        NextScene(SceneManager.GetActiveScene().name);
    }
}
