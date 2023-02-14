using UnityEngine;

public class SEManager : MonoBehaviour
{
    public static SEManager seManager = null;
    AudioSource seAudioSource;

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

    //SEのボリューム
    public float SeVolume
    {
        get
        {
            return seAudioSource.volume;
        }
        set
        {
            seAudioSource.volume = Mathf.Clamp01(value);
        }
    }

    //SEを鳴らす
    public void PlaySe(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }
        seAudioSource.PlayOneShot(clip);
    }
}
