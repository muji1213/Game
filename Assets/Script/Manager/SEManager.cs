using UnityEngine;

public class SEManager : MonoBehaviour
{
    public static SEManager seManager = null;

    //オーディオソース
    public AudioSource seAudioSource;

    //音量
    public float seVol;

    void Start()
    {
        seAudioSource = GetComponent<AudioSource>();

        if (seManager == null)
        {
            seManager = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(gameObject);
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
    //引数で受け取ったボリューム(0 ~ 256)にタイトルで設定したSEのボリュームの割合(0 ~ 1)をかけて再生する
    public void PlaySE(float volume, AudioClip clip)
    {
        seAudioSource.volume = volume;
        seAudioSource.volume *= seVol;
        seAudioSource.PlayOneShot(clip);
    }
}
