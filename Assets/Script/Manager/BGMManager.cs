using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager bgmManager = null;

    AudioSource bgmAudioSource;
   
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
        bgmAudioSource.clip = clip;
        if (clip == null)
        {
            return;
        }
        bgmAudioSource.Play();
    }
}
