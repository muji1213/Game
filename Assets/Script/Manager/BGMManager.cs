using UnityEngine;

public class BGMManager : SingleTon<BGMManager>
{
    AudioSource bgmAudioSource;

    protected override void Init()
    {
        bgmAudioSource = GetComponent<AudioSource>();
    }

    //BGM�̃v���p�e�B
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

    //BGM�𗬂�
    public void PlayBgm(AudioClip clip)
    {
        //�N���b�v�������Ɏ󂯎��
        bgmAudioSource.clip = clip;
        if (clip == null)
        {
            return;
        }

        //�Đ�����
        bgmAudioSource.Play();
    }
}
