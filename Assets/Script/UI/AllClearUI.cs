using UnityEngine;

//���ׂăN���A�����ۂɗ������o
public class AllClearUI : MonoBehaviour
{
    [Header("�\�����ɗ���SE")] [SerializeField] AudioClip SE;
    [Header("���o����")] [SerializeField] [Range(0, 1)] float SEVol = 1;
     
    private float volume;
    private void Awake()
    {
        //�X�^�[�g����OFF
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        //���o����BGM���~�߂邽�߁A���ʂ��L�^
        volume = BGMManager.I.BgmVolume;

        //����0��
        BGMManager.I.BgmVolume = 0.0f;

        //SE
        SEManager.I.PlaySE(SEVol, SE);
    }

    public void Deactive()
    {
        //�{�^������������BGM�̉��ʂ�߂�
        BGMManager.I.BgmVolume = volume;

        //��A�N�e�B�u��
        this.gameObject.SetActive(false);
    }
}
