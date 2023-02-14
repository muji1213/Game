using System.Collections;
using UnityEngine;

//���̃X�N���v�g�̓X�e�[�W�V�[���̊Ǘ�������
public class StageManager : SceneChanger
{
    [Header("���C���J�����ɕ\��������L�����o�X")] [SerializeField] Canvas canvas;
    [Header("�t�B�j�b�V���J�����ɉf���L�����o�X")] [SerializeField] GameObject finishCanvas;

    [Header("Player")] [SerializeField] Player_Controller player;

    [Header("�t�B�j�b�V�����o�p�̃J����")] [SerializeField] Camera finishCam;
    [Header("�X�e�[�W�N���A��ɕ\������UI")] [SerializeField] GameObject resultUI;
    [Header("�|�[�YUI")] [SerializeField] GameObject poseUI;
    [Header("���g���CUI")] [SerializeField] GameObject retryUI;
    [Header("�|�C���gUI")] [SerializeField] GameObject scoreUI;
    [Header("���C�tUI")] [SerializeField] GameObject lifeUI;
    [Header("�Q�[�WUI")] [SerializeField] GameObject gaugeUI;
    [Header("�]��UI")] [SerializeField] GameObject evaluateUI;

    [Header("���U���g�̃X�N���v�g")] [SerializeField] ResultUI result;
    [Header("�J�E���g�_�E���X�N���v�g")] [SerializeField] CountDown countDown;
    [Header("�t�F�[�h�C���̃X�N���v�g")] [SerializeField] Fade fadeIn;

    [Header("�^�[�Q�b�g�̃Q�[���I�u�W�F�N�g")] [SerializeField] GameObject target;

    [Header("BGM")] [SerializeField] AudioClip bgm;
    [Header("�^�[�Q�b�g�Ƃ̏Փˎ���SE")] [SerializeField] AudioClip colSE;
    [Header("�^�[�Q�b�g���������SE")] [SerializeField] AudioClip blowSE;
    [Header("���U���g�\������SE")] [SerializeField] AudioClip successSE;
    [Header("�~�X�����Ƃ���SE")] [SerializeField] AudioClip missedSE;
    [Header("�X�e�[�W����J����")] [SerializeField] StageEnterCamera stageEnterCamera;

    [HideInInspector] public int temporarilypoint; //�X�e�[�W�Ŏ擾�����|�C���g��ێ��A�X�e�[�W�N���A��A�Q�[���}�l�[�W���ɉ��Z����

    //���U���g�̉��_����
    [HideInInspector] public bool isNodamage;
    [HideInInspector] public bool isMaxHP;

    //�X�e�[�W�̃S�[���ʒu
    [HideInInspector] public float goalPos;

    //�X�e�[�W�̎��S�ʒu
    [HideInInspector] public float deadPos;

    //�|�[�Y�����ǂ���
    private bool isPose = false;

    //UI����уv���C���[�̑����L���ɂ�����
    private bool isStart = false;

    //�N���A�������ǂ����i����̓S�[���̃Q�[���I�u�W�F�N�g������True�ɂ����j
    [HideInInspector] public bool isCleared = false;

    //�J�����Ǐ]�p�̃X�N���v�g
    private CameraFollower cameraFollower;

    //�t�B�j�b�V�����o�p�̃J����
    private FinishCamera finishCamera;

    private void Start()
    {
        //�e��R���|�[�l���g
        cameraFollower = GameObject.Find("Main Camera").GetComponent<CameraFollower>();
        finishCamera = GameObject.Find("Finish Camera").GetComponent<FinishCamera>();

        //�t�B�j�b�V���J�����ɉf���L�����o�X�͔�\����
        finishCanvas.SetActive(false);

        //�X�e�[�W�Ŏ擾�����|�C���g��������
        temporarilypoint = 0;

        //���U���g����̏����l��ݒ�
        isNodamage = true;
        isMaxHP = false;

        //�S�[���ʒu���L�^
        goalPos = target.transform.position.z;

        //BGM
        BGMManager.bgmManager.PlayBgm(bgm);

        StopPlayerControll();
    }

    private void Update()
    {
        //�J�E���g�_�E�����I���܂ő��얳��
        if (!countDown.isCountdownEnd)
        {
            //�t�F�[�h���I�������
            if (fadeIn.isAnimEnd)
            {
                stageEnterCamera.StartMovie();

                if (stageEnterCamera.MovieFinished())
                {
                    cameraFollower.enabled = true;             
                }
                else
                {
                    return;
                }

                //�J�E���g�_�E���X�^�[�g
                countDown.StartCountDown();
            }
            return;
        }
        else
        {
            //UI��z�u����
            if (!isStart)
            {
                MovePlayer();
                scoreUI.SetActive(true);
                gaugeUI.SetActive(true);
                isStart = true;
            }
            //�S�[���̃Q�[���I�u�W�F�N�g�ɏՓˌ�A���U���g���Ăяo��
            if (isCleared)
            {
                //���U���gUI�Ăяo��
                Invoke("ActiveResult", 2.0f);

                //�N���A�t���O��false
                isCleared = false;

                result.SetPoint();
            }
        }
    }

    /// <summary>
    /// UI��\��
    /// �n�[�g�̌����Z�b�g
    /// </summary>
    public void SetUI(int frisbeeHP)
    {
        //
        evaluateUI.SetActive(true);
        evaluateUI.GetComponent<EvaluateUI>().Evaluate(frisbeeHP);

        //
        lifeUI.SetActive(true);
        lifeUI.GetComponent<Life>().SetInitialLife(frisbeeHP);
    }

    //�X�e�[�W�Ŏ擾�����|�C���g���ꎞ�I�ɕێ�����
    //�r���Ń��^�C�A�����ہA�|�C���g�����Z�ł��Ȃ��悤�ɂ���
    public void StorePoint(int point)
    {
        temporarilypoint += point;
    }

    //�X�e�[�W�N���A��A�Q�[���}�l�[�W���Ƀ|�C���g�����Z
    public void AddPoint()
    {
        GameManager.gameManager.point += temporarilypoint;

        //�ꎞ�|�C���g��0��
        temporarilypoint = 0;
    }

    /// <summary>
    /// �t���X�r�[����Q���ɏՓˎ��A���C�t�����炷
    /// </summary>
    public void ReduceHPUI()
    {
        lifeUI.GetComponent<Life>().DestroyLife();

        //�m�[�_���[�W�t���O��OFF��
        if (isNodamage)
        {
            isNodamage = false;
        }
    }

    //�t���X�r�[���I�ɓ����������̉��o
    public IEnumerator Finish()
    {
        //SE
        SEManager.seManager.PlaySe(colSE);

        //�t�B�j�b�V���L�����o�X��ON��
        ActiveFinishCanvas();

        //�J�����؂�ւ�
        cameraFollower.ChangeFinishCamera();

        //�U��������
        finishCamera.Shake(0.5f, 1.0f);

        //��莞�Ԏ~�߂�i��ʐU���͂���j
        Time.timeScale = 0.0f;

        yield return new WaitForSecondsRealtime(0.7f);

        Time.timeScale = 1.0f;

        //SE
        SEManager.seManager.PlaySe(blowSE);

        //�t�B�j�b�V���L�����o�X��OFF��
        DeactiveFinishCanvas();
    }

    /// <summary>
    /// �|�[�Y���\�b�h
    /// P�L�[�ŌĂяo�������
    /// </summary>
    public void Pose()
    {
        //�|�[�Y���łȂ����
        if (!isPose)
        {
            //�|�[�Y�t���O�����낷
            isPose = true;

            //�|�[�YUI�\��
            poseUI.SetActive(true);

            //�Q�[�����Ԃ��~
            Time.timeScale = 0.0f;
        }
        else
        {
            //�|�[�Y���Ȃ�
            isPose = false;

            //�|�[�YUI���\��
            poseUI.SetActive(false);

            //�Q�[�����Ԃ�߂�
            Time.timeScale = 1.0f;
        }
    }

    /// <summary>
    /// �v���C���[�������̓t���X�r�[�����S�����ۂɌĂяo��
    /// </summary>
    public void Retry()
    {
        SEManager.seManager.PlaySe(missedSE);
        var value = Mathf.Clamp((float)deadPos / (float)goalPos, 0, 1);
        retryUI.SetActive(true);
        retryUI.GetComponent<RetryUI>().SetClearPer(value);
    }

    /// <summary>
    /// �v���C���[�̑��������Ԃ��ő�HP�ɂȂ�܂łɕK�v�Ȏ��Ԃ̂ǂꂭ�炢�̊�������Ԃ�
    /// ����̓Q�[�W�Ɏg�p����
    /// </summary>
    /// <returns></returns>
    public float GetRunTime()
    {
        return Mathf.Clamp((float)player.runTime / (float)player.MaxHPTime, 0, 1);
    }

    /// <summary>
    /// �v���C���[�̑����L���ɂ���
    /// </summary>
    public void MovePlayer()
    {
        Debug.Log("�����L����");
        player.enabled = true;
    }

    /// <summary>
    /// �v���C���[�̑���𖳌��ɂ���
    /// </summary>
    public void StopPlayerControll()
    {
        Debug.Log("����𖳌���");
        player.enabled = false;
    }

    /// <summary>
    /// �t�B�j�b�V�����o�p�̃L�����o�X��L���ɂ���
    /// </summary>
    public void ActiveFinishCanvas()
    {
        finishCanvas.SetActive(true);
    }

    /// <summary>
    /// �t�B�j�b�V�����o�p�̃L�����o�X�𖳌��ɂ���
    /// </summary>
    public void DeactiveFinishCanvas()
    {
        finishCanvas.SetActive(false);
    }

    /// <summary>
    /// ���U���g��ʂ�L���ɂ���
    /// </summary>
    public void ActiveResult()
    {
        //SE
        SEManager.seManager.PlaySe(successSE);

        //�J������ύX����
        canvas.worldCamera = finishCam;

        //���U���gUI��\��
        resultUI.SetActive(true);

        //�X�e�[�W���N���A�����ɂ���
        StageSelectManager.stageSelectManager.AvtiveStageClearFlag(StageSelectManager.selectedStageNum);
    }
}
