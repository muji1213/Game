using UnityEngine;

public class SEManager : SingleTon<SEManager>
{
    //オーディオソース
    public AudioSource seAudioSource;

    //音量
    public float seVol;

    protected override void Init()
    {
        seAudioSource = GetComponent<AudioSource>();
    }

    //SEのボリューム設定
    public float SEVolume
    {
        get
        {
            return seVol;
        }
        set
        {
            seVol = value;
        }
    }


    //SEの再生
    //引数で受け取ったボリューム(0 ~ 1)にタイトルで設定したSEのボリュームの割合(0 ~ 1)をかけて再生する
    public void PlaySE(float volume, AudioClip clip)
    {
        seAudioSource.volume = volume;
        seAudioSource.volume *= seVol;
        seAudioSource.PlayOneShot(clip);
    }
}
