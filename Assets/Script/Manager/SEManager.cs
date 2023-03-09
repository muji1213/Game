using UnityEngine;

public class SEManager : SingleTon<SEManager>
{
    //�I�[�f�B�I�\�[�X
    public AudioSource seAudioSource;

    //����
    public float seVol;

    protected override void Init()
    {
        seAudioSource = GetComponent<AudioSource>();
    }

    //SE�̃{�����[���ݒ�
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


    //SE�̍Đ�
    //�����Ŏ󂯎�����{�����[��(0 ~ 1)�Ƀ^�C�g���Őݒ肵��SE�̃{�����[���̊���(0 ~ 1)�������čĐ�����
    public void PlaySE(float volume, AudioClip clip)
    {
        seAudioSource.volume = volume;
        seAudioSource.volume *= seVol;
        seAudioSource.PlayOneShot(clip);
    }
}
