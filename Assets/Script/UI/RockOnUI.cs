using UnityEngine;

public class RockOnUI : MonoBehaviour
{
    // フリスビー
    private Transform target;

    // オブジェクトを映すカメラ
    [SerializeField] Camera mainCamera;

    //UIcam
    [SerializeField] Camera uiCamera;

    [SerializeField] AudioClip rockOnSE;

    private AudioSource audioSource;

    RectTransform rectTransform;

    private void Start()
    {
        //音量が合わないため、SEマネージャの割合分をかけ、適性音量にする
        audioSource = GetComponent<AudioSource>();
        audioSource.volume *= (SEManager.seManager.SeVolume / 1);
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
        audioSource.PlayOneShot(rockOnSE);
    }
}
