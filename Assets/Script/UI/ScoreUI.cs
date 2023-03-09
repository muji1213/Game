using UnityEngine;
using UnityEngine.UI;

//ステージ内の取得ポイントを表示する
public class ScoreUI : MonoBehaviour
{
    //スコアを表示するテキスト
    [Header("スコア表示テキスト")][SerializeField]private Text pointText;

    public void ChangeNum(int num)
    {
        pointText.text = num.ToString();
    }
}
