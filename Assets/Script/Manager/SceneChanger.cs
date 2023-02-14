using UnityEngine;
using UnityEngine.SceneManagement;

//シーン遷移
//マネージャー系はこれを継承する
public class SceneChanger : MonoBehaviour
{
   
    //ステージセレクトに遷移する
    public virtual void ToStageSelect()
    {
        //SEを鳴らす
        SEManager.seManager.PlaySe(Resources.Load<AudioClip>("Sound/SE/SE_Input"));

        //ポーズなどから遷移することがあるため、ゲームスピードは戻す
        Time.timeScale = 1.0f;

        //遷移
        GameManager.gameManager.NextScene("StageSelect");
        SceneManager.LoadScene("StageSelect");
    }

    //タイトル画面に遷移する
    public void ReturnTitle()
    {
        //SEを鳴らす
        SEManager.seManager.PlaySe(Resources.Load<AudioClip>("Sound/SE/SE_InputCancel"));

        //ポーズなどから遷移するため、ゲームスピードは戻す
        Time.timeScale = 1.0f;

        //遷移
        GameManager.gameManager.NextScene("Title");
        SceneManager.LoadScene("Title");
    }
}
