using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

//シーン遷移
//マネージャー系はこれを継承する
public class SceneChanger : SingleTon<SceneChanger>
{
    private AsyncOperation async;

    //遷移先シーンの名前
    private string nextSceneName;

    [SerializeField]
    private GameObject loadUIPrefab;

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

        //移動
        GoNextScene(sceneName);
    }

    //シーンへ遷移する
    public void GoNextScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
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

    //ステージセレクトに遷移する
    public void ToStageSelectScene()
    {
        //ポーズなどから遷移することがあるため、ゲームスピードは戻す
        Time.timeScale = 1.0f;
        NextScene("StageSelect");
    }

    //タイトル画面に遷移する
    public void ToTitleScene()
    {
        //ポーズなどから遷移するため、ゲームスピードは戻す
        Time.timeScale = 1.0f;

        //遷移
        NextScene("Title");
    }

    //説明シーンへ飛ぶ
    public void ToInstructionScene()
    {
        NextScene("Instruction");
    }

    //ステージシーンへ遷移する
    public void ToStageScene(int stageNum)
    {
        NextScene("Stage" + stageNum);
    }

    //ショップシーンへ遷移する
    public void ToShopScene()
    {
        NextScene("Shop");
    }

    //リトライ
    public void Retry()
    {
        NextScene(SceneManager.GetActiveScene().name);
    }
}
