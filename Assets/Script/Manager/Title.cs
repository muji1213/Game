using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [SerializeField] [Header("BGM")] AudioClip bgm;
    [SerializeField] [Header("�������̉�")] AudioClip inputSE;
   
    [Header("BGM�X���C�_�[")] [SerializeField] Slider bgmSlider;
    [Header("SE�X���C�_�[")] [SerializeField] Slider seSlider;
    [Header("SE�X���C�_�[�𓮂������Ƃ��ɖ�SE")] [SerializeField] AudioClip seSliderSE;

    //�e�{�����[��
    private static float bgmVol;
    private static float seVol;

    //������
    private static bool isInitialized = false;

    private void Start()
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
        BGMManager.I.PlayBgm(bgm);

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
        BGMManager.I.BgmVolume = bgmVol;
        SEManager.I.SEVolume = seVol;
    }


    //SE�X���C�_�[����}�E�X�𗣂�������SE��炷
    public void SEVolChanged()
    {
        SEManager.I.PlaySE(1, seSliderSE);
    }

    //�����V�[����
    public void ToInstrucion()
    {
        SEManager.I.PlaySE(seVol, inputSE);
        SceneChanger.I.ToInstructionScene();
    }

    //�X�e�[�W�Z���N�g��
    public void ToStageSelect()
    {
        SEManager.I.PlaySE(seVol, inputSE);
        SceneChanger.I.ToStageSelectScene();
    }
}
