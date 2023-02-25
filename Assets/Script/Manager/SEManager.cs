using UnityEngine;

public class SEManager : MonoBehaviour
{
    public static SEManager seManager = null;

    //�I�[�f�B�I�\�[�X
    public AudioSource seAudioSource;

    //����
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
    //�����Ŏ󂯎�����{�����[��(0 ~ 256)�Ƀ^�C�g���Őݒ肵��SE�̃{�����[���̊���(0 ~ 1)�������čĐ�����
    public void PlaySE(float volume, AudioClip clip)
    {
        seAudioSource.volume = volume;
        seAudioSource.volume *= seVol;
        seAudioSource.PlayOneShot(clip);
    }
}
