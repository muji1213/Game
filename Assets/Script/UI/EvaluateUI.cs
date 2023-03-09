using UnityEngine;
using UnityEngine.UI;

//フリスビーを投げた時の評価です
//走行距離に応じて評価が上がります
public class EvaluateUI : MonoBehaviour
{
    //スプライト
    [Header("エクセレント")] [SerializeField] Sprite excellent;
    [Header("グレート")] [SerializeField] Sprite great;
    [Header("グッド")] [SerializeField] Sprite good;

    [Header("SE")] [SerializeField] AudioClip SE;
    [Header("SEの音量")] [SerializeField] [Range(0, 1)] float SEVol = 1;

    [Header("画像のインスタンス")] [SerializeField] private Image sprite;

    private void Awake()
    {
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        //ステージマネージャ側からactiveにされるので、Enableで一度だけ呼ぶ
        SEManager.I.PlaySE(SEVol, SE);
    }

    //typeはFrisbeeのHP
    //最大３
    public void Evaluate(int type)
    {
        //フリスビーを投げた時のHPで評価が変わる
        switch (type)
        {
            case 1:
                sprite.sprite = good;
                break;

            case 2:
                sprite.sprite = great;
                break;

            case 3:
                sprite.sprite = excellent;
                break;

            case 4:
                sprite.sprite = excellent;
                break;
        }
    }
}
