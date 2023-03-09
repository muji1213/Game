using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    [Header("あつめたポイントテキスト")] [SerializeField] Text getPointText;
    [Header("ノーダメージボーナステキスト")] [SerializeField] Text nodamageText;
    [Header("最大HPボーナステキスト")] [SerializeField] Text maxHPText;
    [Header("トータルポイント")] [SerializeField] Text totalPointText;

    //ポイントを設定
    public void SetPoint(int score, int nodamage, int maxHP)
    {
        //ステージ中であつめたポイントを代入
        getPointText.text = score.ToString();

        //ノーダメージ加算ポイント表示
        nodamageText.text = nodamage.ToString();

        //最大HP加算ポイント
        maxHPText.text = maxHP.ToString();

        //トータルを表示
        totalPointText.text = (nodamage + maxHP + score).ToString() + "ポイント";
    }
}
