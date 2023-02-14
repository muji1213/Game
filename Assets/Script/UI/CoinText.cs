using UnityEngine;
using UnityEngine.UI;

//ステージ内の取得ポイントを表示する
public class CoinText : MonoBehaviour
{
    //スコアを表示するテキスト
    [Header("スコア表示テキスト")][SerializeField]private Text pointText;

    //ステージマネージャー
    private StageManager stageManager;
    
    void Start()
    {
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        this.gameObject.SetActive(false);
    }

    //ステージ内で取得したポイントを表示する
    //ポイントはステージマネージャーが保持している
    void Update()
    {
        pointText.text =　stageManager.temporarilypoint.ToString();
    }
}
