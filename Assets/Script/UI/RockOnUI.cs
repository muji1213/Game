using UnityEngine;

public class RockOnUI : MonoBehaviour
{
    // �t���X�r�[
    private Transform target;

    // �I�u�W�F�N�g���f���J����
    [SerializeField] Camera mainCamera;

    //UIcam
    [SerializeField] Camera uiCamera;

    [SerializeField] AudioClip rockOnSE;

    private AudioSource audioSource;

    RectTransform rectTransform;

    private void Start()
    {
        //���ʂ�����Ȃ����߁ASE�}�l�[�W���̊������������A�K�����ʂɂ���
        audioSource = GetComponent<AudioSource>();
        audioSource.volume *= (SEManager.seManager.SeVolume / 1);
        rectTransform = this.GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        target = GameObject.FindWithTag("Frisbee").transform;
    }

    private void Update()
    {
        // �I�u�W�F�N�g�̃��[���h���W
        var targetWorldPos = target.position;

        // ���[���h���W���X�N���[�����W�ɕϊ�����
        var targetScreenPos = mainCamera.WorldToScreenPoint(targetWorldPos);


        RectTransform parentUI = rectTransform.parent.GetComponent<RectTransform>();

        // �X�N���[�����W��UI���[�J�����W�ϊ�
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentUI,
            targetScreenPos,
            uiCamera, // �I�[�o�[���C���[�h�̏ꍇ��null
            out var uiLocalPos
        );

        //���W�𔽉f
        rectTransform.localPosition = uiLocalPos;
    }

    //�A�j���[�V�����C�x���g�ŌĂ�
    public void PlayRockOnSE()
    {
        audioSource.PlayOneShot(rockOnSE);
    }
}
