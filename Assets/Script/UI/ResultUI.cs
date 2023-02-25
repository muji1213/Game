using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultUI : SceneChanger
{
    [Header("ステージマネージャー")] [SerializeField] StageManager stageManager;
    [Header("あつめたポイントテキスト")] [SerializeField] Text getPointText;
    [Header("ノーダメージボーナステキスト")] [SerializeField] Text nodamageText;
    [Header("最大HPボーナステキスト")] [SerializeField] Text maxHPText;
    [Header("トータルポイント")] [SerializeField] Text totalPointText;

    [Header("ノーダメージの時の加点")] [SerializeField] int nodamagePoint;
    [Header("最大HPになるまで走行した時の加点")] [SerializeField] int maxHPPoint;
    
    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    //ポイントを設定
    public void SetPoint()
    {
        //ステージ中であつめたポイントを代入
        getPointText.text = stageManager.temporarilypoint.ToString();

        //ノーダメージなら
        if (stageManager.isNodamage)
        {
            //ノーダメージテキストに数字を代入
            nodamageText.text = nodamagePoint.ToString();
        }
        else
        {
            //ダメージを受けていたら0ポイント
            nodamageText.text = 0.ToString();
            nodamagePoint = 0;
        }

        //最大HPになるまで走ったら
        if (stageManager.isMaxHP)
        {
            //マックスHPテキストに数字を代入
            maxHPText.text = maxHPPoint.ToString();
        }
        else
        {
            //そうでないなら0ポイント
            maxHPText.text = 0.ToString();
            maxHPPoint = 0;
        }

        //トータルを表示
        totalPointText.text = (nodamagePoint + maxHPPoint + stageManager.temporarilypoint).ToString() + "ポイント";
    }

     public override void ToStageSelectScene()
    {
        //ステージマネージャーに加算
        stageManager.StorePoint(nodamagePoint + maxHPPoint);

        //ゲームマネージャーに加算
        stageManager.AddPoint();

        //ゲーム時間を戻す
        Time.timeScale = 1.0f;

        //ステージセレクトに
        SceneManager.LoadScene("StageSelect");
    }
}
