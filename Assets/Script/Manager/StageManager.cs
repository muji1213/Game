using System.Collections;
using UnityEngine;
using OnStage;

//���̃X�N���v�g�̓X�e�[�W�V�[���̊Ǘ�������
public class StageManager : MonoBehaviour
{
    [Header("UI�}�l�[�W���[")] [SerializeField] OnStage.UIManager uiManager;

    [Header("�X�e�[�W�֘A")]
    [Header("�m�[�_���[�W�̎��̉��Z�X�R�A")] [SerializeField] int noDamageAddPoint;
    [Header("HP���^���ɂȂ�܂ő��������̉��Z�X�R�A")] [SerializeField] int maxHPAddPoint;

    [Header("�J�����֘A")]
    [Header("�t�B�j�b�V�����o�p�̃J����")] [SerializeField] Camera finishCam;
    [Header("�X�e�[�W����J����")] [SerializeField] StageEnterCamera stageEnterCamera;
    [Header("���C���J�����ɕ\��������L�����o�X")] [SerializeField] Canvas canvas;
    [Header("�t�B�j�b�V���J�����ɉf���L�����o�X")] [SerializeField] GameObject finishCanvas;

    [Header("�I�u�W�F�N�g�֘A")]
    [Header("Player")] [SerializeField] Player_Controller player;
    [Header("�^�[�Q�b�g�̃Q�[���I�u�W�F�N�g")] [SerializeField] GameObject target;

    [Header("���֘A")]
    [Header("BGM")] [SerializeField] AudioClip bgm;
    [Header("�^�[�Q�b�g�Ƃ̏Փˎ���SE")] [SerializeField] AudioClip colSE;
    [Header("�Փ�SE�̉���")] [SerializeField] [Range(0, 1)] float colSEVol = 1;
    [Header("�^�[�Q�b�g���������SE")] [SerializeField] AudioClip blowSE;
    [Header("�������SE�̉���")] [SerializeField] [Range(0, 1)] float blowSEVol = 1;
    [Header("���U���g�\������SE")] [SerializeField] AudioClip successSE;
    [Header("���U���gSE�̉���")] [SerializeField] [Range(0, 1)] float successSEVol = 1;
    [Header("�~�X�����Ƃ���SE")] [SerializeField] AudioClip missedSE;
    [Header("�~�XSE�̉���")] [SerializeField] [Range(0, 1)] float missSEVol = 1;

    //�X�e�[�W�Ŏ擾�����|�C���g��ێ��A�X�e�[�W�N���A��A�Q�[���}�l�[�W���ɉ��Z����
    private int temporarilyScore;

    //���U���g�̉��_����
    private bool isNodamage;
    private bool isMaxHP;

    //�g�[�^���X�R�A
    private int totalScore = 0;

    //�X�e�[�W�̃S�[���ʒu
    private float goalPos;

    //�|�[�Y�����ǂ���
    private bool isPose = false;

    //UI����уv���C���[�̑����L���ɂ�����
    private bool isStart = false;

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
        temporarilyScore = 0;

        //���U���g����̏����l��ݒ�
        isNodamage = true;
        isMaxHP = false;

        //�S�[���ʒu���L�^
        goalPos = target.transform.position.z;

        //BGM
        BGMManager.I.PlayBgm(bgm);

        //�X�^�[�g���̓v���C���[���~
        StopPlayerControll();

        //UI��������
        uiManager.Inicialize();
    }

    private void Update()
    {
        //�J�E���g�_�E�����I���܂ő��얳��
        if (!uiManager.isCountDownEnd())
        {
            //�t�F�[�h���I�������
            if (uiManager.isFadeEnd())
            {
                //�X�^�[�g���o�𗬂�
                stageEnterCamera.StartMovie();

                //���o���I�������A�v���C���[�ɃJ������Ǐ]������
                if (stageEnterCamera.MovieFinished())
                {
                    cameraFollower.enabled = true;
                }
                else
                {
                    return;
                }

                //�J�E���g�_�E���X�^�[�g
                uiManager.ActiveCountDownUI();
            }
            return;
        }
        else
        {
            //UI��z�u����
            if (!isStart)
            {
                //�v���C���[�̑����L���ɂ���
                MovePlayer();

                //UI��L���ɂ���
                uiManager.ActiveGaugeUI();
                uiManager.ActiveScoreUI();

                isStart = true;
            }
        }
    }

    private void FixedUpdate()
    {
        //�Q�[�W�����߂�
        uiManager.ChargeGaugeUI(GetRunTime());
    }

    /// <summary>
    /// UI��\��
    /// �n�[�g�̌����Z�b�g
    /// </summary>
    public void SetUIAndLife(int frisbeeHP, bool isLifeUp)
    {
        //UI��\������
        uiManager.ActiveEvaluateUIAndLife(frisbeeHP);

        //�ő�HP�܂ő������t���O��True�ɂ���

        //���C�t�A�b�v������Ă����ꍇ�A4���ő�HP�ɂȂ�
        if (isLifeUp)
        {
            if (frisbeeHP == 4)
            {
                isMaxHP = true;
            }
            else
            {
                isMaxHP = false;
            }
        }
        //����Ă��Ȃ��ꍇ�A3���ő�HP�ɂȂ�
        else
        {
            if (frisbeeHP == 3)
            {
                isMaxHP = true;
            }
            else
            {
                isMaxHP = false;
            }
        }
    }

    //�X�e�[�W�Ŏ擾�����|�C���g���ꎞ�I�ɕێ�����
    //�X�R�A�e�L�X�g��ύX����
    //�r���Ń��^�C�A�����ہA�|�C���g�����Z�ł��Ȃ��悤�ɂ���
    public void StorePoint(int point)
    {
        temporarilyScore += point;
        uiManager.ChangeScoreUI(temporarilyScore);
    }

    //�X�e�[�W�N���A��A�Q�[���}�l�[�W���Ƀ|�C���g�����Z
    public void AddPoint(int score)
    {
        GameManager.I.Point += score;

        //�ꎞ�|�C���g��0��
        temporarilyScore = 0;
    }

    /// <summary>
    /// �t���X�r�[����Q���ɏՓˎ��A���C�tUI�����炵�A�m�[�_���[�W�t���O��false��
    /// </summary>
    public void TakeDamage()
    {
        //UI�̃n�[�g������炷
        uiManager.ReduceHPUI();

        //�U��������
        cameraFollower.Shake(0.3f, 1.0f);

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
        SEManager.I.PlaySE(colSEVol, colSE);

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
        SEManager.I.PlaySE(blowSEVol, blowSE);

        //�t�B�j�b�V���L�����o�X��OFF��
        DeactiveFinishCanvas();

        //���U���g�Ăяo��
        Invoke("ActiveResult", 2.0f);
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
            uiManager.ActivePoseUI();

            //�Q�[�����Ԃ��~
            Time.timeScale = 0.0f;
        }
        else
        {
            //�|�[�Y���Ȃ�
            isPose = false;

            //�|�[�YUI���\��
            uiManager.DeactivePoseUI();

            //�Q�[�����Ԃ�߂�
            Time.timeScale = 1.0f;
        }
    }

    /// <summary>
    /// �v���C���[�������̓t���X�r�[�����S�����ۂɌĂяo��
    /// </summary>
    public void Retry(float deadPos)
    {
        StopPlayerControll();

        //SE
        SEManager.I.PlaySE(missSEVol, missedSE);

        //�ǂꂾ���̊����i�񂾂��̊������o��
        var value = Mathf.Clamp((float)deadPos / (float)goalPos, 0, 1);

        uiManager.ActiveRetryUI(value);
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
        player.enabled = true;
    }

    /// <summary>
    /// �v���C���[�̑���𖳌��ɂ���
    /// </summary>
    public void StopPlayerControll()
    {
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
        SEManager.I.PlaySE(successSEVol, successSE);

        //�J������ύX����
        canvas.worldCamera = finishCam;

        var noDamageAddPoint = 0;
        var maxHPAddPoint = 0;

        //�m�[�_���[�W���ǂ���
        switch (isNodamage)
        {
            //�m�[�_���[�W�Ȃ�ݒ肵�������Z����
            case true:
                noDamageAddPoint = this.noDamageAddPoint;
                break;

            case false:
                noDamageAddPoint = 0;
                break;
        }

        //�ő�HP�܂ő�������
        switch (isMaxHP)
        {
            //�������Ȃ�ݒ肵�������Z����
            case true:
                maxHPAddPoint = this.maxHPAddPoint;
                break;

            case false:
                maxHPAddPoint = 0;
                break;
        }

        //���U���gUI��\��
        uiManager.ActiveResultUI(temporarilyScore, noDamageAddPoint, maxHPAddPoint);

        //�g�[�^���X�R�A���Z�o����
        totalScore = temporarilyScore + maxHPAddPoint + noDamageAddPoint;

        //�Q�[���}�l�[�W���[�ɃX�R�A���Z
        AddPoint(totalScore);

        //�X�e�[�W���N���A�����ɂ���
        StageSelectManager.I.AvtiveStageClearFlag(GameManager.I.SelectedStageInfo.StageNum);
    }

    //�ēx���̃V�[������蒼���i���g���C�j
    public void ReTryThisScene()
    {
        //�Q�[�����Ԃ�߂��A���g���C
        Time.timeScale = 1.0f;
        SceneChanger.I.Retry();
    }

    //�^�C�g����
    public void ToTitleScene()
    {
        //�Q�[�����Ԃ�߂��A�^�C�g����
        Time.timeScale = 1.0f;
        SceneChanger.I.ToTitleScene();
    }

    //�|�[�Y��ʂłÂ�����������Ƃ��A�Q�[���𑱍s������
    public void ContinueGame()
    {
        Time.timeScale = 1.0f;
        uiManager.DeactivePoseUI();
    }

    //�X�e�[�W�Z���N�g��
    public void ToStageSelectScene()
    {
        //�Q�[�����Ԃ�߂��A�X�e�[�W�Z���N�g��
        Time.timeScale = 1.0f;
        SceneChanger.I.ToStageSelectScene();
    }
}
