using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


//このスクリプトはプレイヤー、もしくはフリスビーが死亡した際に呼び出されるUIのスクリプトです
//ステージのどのくらいまで進んだかをゲージで表示します
public class RetryUI : MonoBehaviour
{
    [Header("表側のゲージ")] [SerializeField] Image forwardGauge;

    //targetScaleXを記録したかどうか
    private bool isRecorded = false;

    //ゲージの初期の大きさ
    private float scaleX = 0;

    //ゲージの目標の大きさ
    private float targetScaleX;

    //ゲージのアニメーション速度
    private float easing = 0.05f;

    void Awake()
    {
        forwardGauge.transform.localScale = new Vector2(0, 1);
        //初期は非表示
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //ラープで減衰しながら近づく
        scaleX = Mathf.Lerp(scaleX, targetScaleX, easing);
        forwardGauge.transform.localScale = new Vector2(scaleX, 1.0f);
    }

    public void SetClearPer(float per)
    {
        //一回呼び出されたら移行処理しない
        if (isRecorded)
        {
            return;
        }

        //目標の大きさを記録
        targetScaleX = per;

        //フラグをおろす
        isRecorded = true;
    }
}
