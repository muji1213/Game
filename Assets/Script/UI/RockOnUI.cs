using UnityEngine;

public class RockOnUI : MonoBehaviour
{
    // フリスビー
    private Transform target;

    // オブジェクトを映すカメラ
    [Header("メインカメラ")][SerializeField] Camera mainCamera;

    //UIcam
    [Header("UIカメラ")][SerializeField] Camera uiCamera;

    [Header("ロックオンSE")][SerializeField] AudioClip rockOnSE;
    [Header("ロックオンSEの音量")][SerializeField] [Range(0, 1)] float rockOnSEVol = 1;

    RectTransform rectTransform;

    private void Start()
    {
        rectTransform = this.GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        target = GameObject.FindWithTag("Frisbee").transform;
    }

    private void Update()
    {
        // オブジェクトのワールド座標
        var targetWorldPos = target.position;

        // ワールド座標をスクリーン座標に変換する
        var targetScreenPos = mainCamera.WorldToScreenPoint(targetWorldPos);


        RectTransform parentUI = rectTransform.parent.GetComponent<RectTransform>();

        // スクリーン座標→UIローカル座標変換
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentUI,
            targetScreenPos,
            uiCamera, // オーバーレイモードの場合はnull
            out var uiLocalPos
        );

        //座標を反映
        rectTransform.localPosition = uiLocalPos;
    }

    //アニメーションイベントで呼ぶ
    public void PlayRockOnSE()
    {
        SEManager.seManager.PlaySE(rockOnSEVol,rockOnSE);
    }
}
