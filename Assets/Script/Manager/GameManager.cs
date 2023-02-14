using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


//ゲーム内ポイントとロードの管理をする
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject loadUIPrefab;

    private AsyncOperation async;

    //遷移先シーンの名前
    private string nextSceneName;

    public static GameManager gameManager = null;

    //ステージで取得したポイントを保持する
    //デバッグ用に[hide]にしないこと
    public int point;

    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = this;

            //シーン間で削除が行われないようにする
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void NextScene(string sceneName)
    {
        //読み込み中はロードUIを出す
        GameObject loadUI = Instantiate(loadUIPrefab);

        loadUI.transform.SetParent(GameObject.Find("Canvas").transform);

        loadUI.transform.localPosition = new Vector3(0, 0, 0);
        loadUI.transform.localScale = new Vector3(1, 1, 1);

        nextSceneName = sceneName;
        
        //　コルーチンを開始
        StartCoroutine("LoadData");
    }

    private IEnumerator LoadData()
    {
        // シーンの読み込みをする
        async = SceneManager.LoadSceneAsync(nextSceneName);

        //読み込み終わりまで待つ
        while (!async.isDone)
        {
            yield return null;
        }

        async.allowSceneActivation = true;
    }
}
