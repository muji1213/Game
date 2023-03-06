using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager bgmManager = null;

    AudioSource bgmAudioSource;
   
    //一つしか存在しないようにする
    void Awake()
    {
        bgmAudioSource = GetComponent<AudioSource>();

        if (bgmManager == null)
        {
            bgmManager = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    //BGMのプロパティ
    public float BgmVolume
    {
        get
        {
            return bgmAudioSource.volume;
        }
        set
        {
            bgmAudioSource.volume = Mathf.Clamp01(value);
        }
    }

    //BGMを流す
    public void PlayBgm(AudioClip clip)
    {
        //クリップを引数に受け取る
        bgmAudioSource.clip = clip;
        if (clip == null)
        {
            return;
        }

        //再生する
        bgmAudioSource.Play();
    }
}
