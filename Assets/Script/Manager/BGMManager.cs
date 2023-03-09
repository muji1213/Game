using UnityEngine;

public class BGMManager : SingleTon<BGMManager>
{
    AudioSource bgmAudioSource;

    protected override void Init()
    {
        bgmAudioSource = GetComponent<AudioSource>();
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
