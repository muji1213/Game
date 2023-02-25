using UnityEngine;
using UnityEngine.SceneManagement;

//シーン遷移
//マネージャー系はこれを継承する
public class SceneChanger : MonoBehaviour
{
    [SerializeField] [Header("決定音の音量")] [Range(0, 1)] float inputSEVol = 1;
    [SerializeField] [Header("キャンセル音の音量")] [Range(0, 1)] float cancelSEVol = 1;

    //ステージセレクトに遷移する
    public virtual void ToStageSelectScene()
    {
        //SEを鳴らす
        SEManager.seManager.PlaySE(inputSEVol, Resources.Load<AudioClip>("Sound/SE/SE_Input"));

        //ポーズなどから遷移することがあるため、ゲームスピードは戻す
        Time.timeScale = 1.0f;

        //遷移
        GameManager.gameManager.NextScene("StageSelect");
        SceneManager.LoadScene("StageSelect");
    }

    //タイトル画面に遷移する
    public void ToTitleScene()
    {
        //SEを鳴らす
        SEManager.seManager.PlaySE(cancelSEVol, Resources.Load<AudioClip>("Sound/SE/SE_InputCancel"));
       
        //ポーズなどから遷移するため、ゲームスピードは戻す
        Time.timeScale = 1.0f;

        //遷移
        GameManager.gameManager.NextScene("Title");
        SceneManager.LoadScene("Title");
    }


    //説明シーンへ飛ぶ
    public void ToInstructionScene()
    {
        //SEを鳴らす
        SEManager.seManager.PlaySE(inputSEVol, Resources.Load<AudioClip>("Sound/SE/SE_Input"));
        GameManager.gameManager.NextScene("Instruction");
        SceneManager.LoadScene("Instruction");
    }
}
