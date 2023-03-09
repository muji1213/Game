using UnityEngine;

//すべてクリアした際に流す演出
public class AllClearUI : MonoBehaviour
{
    [Header("表示時に流すSE")] [SerializeField] AudioClip SE;
    [Header("演出音量")] [SerializeField] [Range(0, 1)] float SEVol = 1;
     
    private float volume;
    private void Awake()
    {
        //スタート時はOFF
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        //演出中はBGMを止めるため、音量を記録
        volume = BGMManager.I.BgmVolume;

        //音量0に
        BGMManager.I.BgmVolume = 0.0f;

        //SE
        SEManager.I.PlaySE(SEVol, SE);
    }

    public void Deactive()
    {
        //ボタンを押下時にBGMの音量を戻す
        BGMManager.I.BgmVolume = volume;

        //非アクティブに
        this.gameObject.SetActive(false);
    }
}
