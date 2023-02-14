using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


//�Q�[�����|�C���g�ƃ��[�h�̊Ǘ�������
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject loadUIPrefab;

    private AsyncOperation async;

    //�J�ڐ�V�[���̖��O
    private string nextSceneName;

    public static GameManager gameManager = null;

    //�X�e�[�W�Ŏ擾�����|�C���g��ێ�����
    //�f�o�b�O�p��[hide]�ɂ��Ȃ�����
    public int point;

    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = this;

            //�V�[���Ԃō폜���s���Ȃ��悤�ɂ���
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

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
}
