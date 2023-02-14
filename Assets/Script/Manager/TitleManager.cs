using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : SceneChanger
{
    [SerializeField] [Header("�����{�^����SE")] AudioClip SE;
    [SerializeField] [Header("BGM")] AudioClip bgm;

    [Header("BGM�X���C�_�[")] [SerializeField] Slider bgmSlider;
    [Header("SE�X���C�_�[")] [SerializeField] Slider seSlider;

    [Header("SE�X���C�_�[�𓮂������Ƃ��ɖ�SE")] [SerializeField] AudioClip seSliderSE;

    //�e�{�����[��
    private static float bgmVol;
    private static float seVol;

    //������
    private static bool isInitialized = false;

    void Start()
    {
        //������
        if (!isInitialized)
        {
            //���ꂼ�ꉹ�ʂ�1
            bgmVol = 0.6f;
            seVol = 0.6f;
            isInitialized = true;
        }

        //BGM
        BGMManager.bgmManager.PlayBgm(bgm);

        //2��ڈȍ~�́Astatic�ŕێ����Ă������ʂ�K�p����
        bgmSlider.value = bgmVol;
        seSlider.value = seVol;
    }

    private void FixedUpdate()
    {
        //�X���C�_�[�̒l��ێ�
        bgmVol = bgmSlider.value;
        seVol = seSlider.value;

        //���ʃ}�l�[�W���̃{�����[���̒l�ɓK�p����
        BGMManager.bgmManager.BgmVolume = bgmVol;
        SEManager.seManager.SeVolume = seVol;
    }

    //�����V�[���֔��
    public void ToInstruction()
    {
        //SE
        SEManager.seManager.PlaySe(SE);

        GameManager.gameManager.NextScene("Instruction");
        SceneManager.LoadScene("Instruction");
    }

    //SE�X���C�_�[����}�E�X�𗣂�������SE��炷
    public void SEVolChanged()
    {
        SEManager.seManager.PlaySe(seSliderSE);
    }
}
