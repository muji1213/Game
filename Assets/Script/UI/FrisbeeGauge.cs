using UnityEngine;
using UnityEngine.UI;

//このスクリプトは走行時間に応じてゲージをためる
public class FrisbeeGauge : MonoBehaviour
{
    //ステージマネージャー
    private StageManager stageManager;

    //ゲージのスプライト
    [Header("ゲージのスプライト")] [SerializeField] private Image image;
    // Start is called before the first frame update
    void Start()
    {
        //各種コンポーネント
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        this.gameObject.SetActive(false);
    }

    //走行距離に応じてゲージをためる
    private void FixedUpdate()
    {
        //fillAmountでゲージを変化させる
        //割合はステージマネージャーのGetRunTimeから取得
        image.fillAmount = stageManager.GetRunTime();
    }
}
