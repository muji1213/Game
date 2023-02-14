using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


//���̃X�N���v�g�̓v���C���[�A�������̓t���X�r�[�����S�����ۂɌĂяo�����UI�̃X�N���v�g�ł�
//�X�e�[�W�̂ǂ̂��炢�܂Ői�񂾂����Q�[�W�ŕ\�����܂�
public class RetryUI : MonoBehaviour
{
    [Header("�\���̃Q�[�W")] [SerializeField] Image forwardGauge;

    //targetScaleX���L�^�������ǂ���
    private bool isRecorded = false;

    //�Q�[�W�̏����̑傫��
    private float scaleX = 0;

    //�Q�[�W�̖ڕW�̑傫��
    private float targetScaleX;

    //�Q�[�W�̃A�j���[�V�������x
    private float easing = 0.05f;

    void Awake()
    {
        forwardGauge.transform.localScale = new Vector2(0, 1);
        //�����͔�\��
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //���[�v�Ō������Ȃ���߂Â�
        scaleX = Mathf.Lerp(scaleX, targetScaleX, easing);
        forwardGauge.transform.localScale = new Vector2(scaleX, 1.0f);
    }

    //���g���C���������ꍇ
    public void Retry()
    {
        //�Q�[�����Ԃ�߂��A�ēx�V�[����ǂݍ���
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //�X�e�[�W�Z���N�g�ւ��������ꍇ
    public void ReturnStageSelect()
    {
        //�Q�[�����Ԃ�߂��A�X�e�[�W�Z���N�g��
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("StageSelect");
    }

    public void SetClearPer(float per)
    {
        //���Ăяo���ꂽ��ڍs�������Ȃ�
        if (isRecorded)
        {
            return;
        }

        //�ڕW�̑傫�����L�^
        targetScaleX = per;

        //�t���O�����낷
        isRecorded = true;
    }
}
