using UnityEngine;

//���ׂăN���A�����ۂɗ������o
public class AllClearUI : MonoBehaviour
{
    [Header("�\�����ɗ���SE")] [SerializeField] AudioClip SE;

    private float volume;
    private void Awake()
    {
        //�X�^�[�g����OFF
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        //���o����BGM���~�߂邽�߁A���ʂ��L�^
        volume = BGMManager.bgmManager.BgmVolume;

        //����0��
        BGMManager.bgmManager.BgmVolume = 0.0f;

        //SE
        SEManager.seManager.PlaySe(SE);
    }

    public void Deactive()
    {
        //�{�^������������BGM�̉��ʂ�߂�
        BGMManager.bgmManager.BgmVolume = volume;

        //��A�N�e�B�u��
        this.gameObject.SetActive(false);
    }
}
