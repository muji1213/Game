using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : SceneChanger
{
    [SerializeField] [Header("BGM")] AudioClip bgm;

    [Header("BGMスライダー")] [SerializeField] Slider bgmSlider;
    [Header("SEスライダー")] [SerializeField] Slider seSlider;

    [Header("SEスライダーを動かしたときに鳴るSE")] [SerializeField] AudioClip seSliderSE;

    //各ボリューム
    private static float bgmVol;
    private static float seVol;

    //初期化
    private static bool isInitialized = false;

    private void Start()
    {
        //初期化
        if (!isInitialized)
        {
            //それぞれ音量は1
            bgmVol = 0.6f;
            seVol = 0.6f;
            isInitialized = true;
        }

        //BGM
        BGMManager.bgmManager.PlayBgm(bgm);

        //2回目以降は、staticで保持していた音量を適用する
        bgmSlider.value = bgmVol;
        seSlider.value = seVol;
    }

    private void FixedUpdate()
    {
        //スライダーの値を保持
        bgmVol = bgmSlider.value;
        seVol = seSlider.value;

        //音量マネージャのボリュームの値に適用する
        BGMManager.bgmManager.BgmVolume = bgmVol;
        SEManager.seManager.SEVolume = seVol;
    }


    //SEスライダーからマウスを離した時にSEを鳴らす
    public void SEVolChanged()
    {
       
        SEManager.seManager.PlaySE(128, seSliderSE);
    }
}
