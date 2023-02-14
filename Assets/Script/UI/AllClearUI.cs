using UnityEngine;

//すべてクリアした際に流す演出
public class AllClearUI : MonoBehaviour
{
    [Header("表示時に流すSE")] [SerializeField] AudioClip SE;

    private float volume;
    private void Awake()
    {
        //スタート時はOFF
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        //演出中はBGMを止めるため、音量を記録
        volume = BGMManager.bgmManager.BgmVolume;

        //音量0に
        BGMManager.bgmManager.BgmVolume = 0.0f;

        //SE
        SEManager.seManager.PlaySe(SE);
    }

    public void Deactive()
    {
        //ボタンを押下時にBGMの音量を戻す
        BGMManager.bgmManager.BgmVolume = volume;

        //非アクティブに
        this.gameObject.SetActive(false);
    }
}
