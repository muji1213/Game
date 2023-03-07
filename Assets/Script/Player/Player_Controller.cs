using System.Collections;
using UnityEngine;


//���̃X�N���v�g�̓t���X�r�[�𓊂���O�̃v���C���[�̃X�N���v�g�ł�
public class Player_Controller : MonoBehaviour, IMovable, IDienable, PlayerUnit
{
    public enum State
    {
        Idle,
        Run,
        Jump,
        Shoot,
        Dead
    }

    //�ϐ�
    [Header("���ړ����x")] [SerializeField] float zMoveSpeed;
    [Header("���ړ����x")] [SerializeField] float xMoveSpeed;
    [Header("�W�����v���x")] [SerializeField] float jumpSpeed;
    [Header("�d��")] [SerializeField] float gravity;
    [Header("�t���X�r�[")] [SerializeField] GameObject frisbee;
    [Header("�v���C���[�̉����x")] [SerializeField] AnimationCurve runCurve;
    [Header("�ڒn����`�F�b�N")] [SerializeField] GroundChecker groundChecker;
    [Header("�_���[�W�󂯂�����SE")] [SerializeField] AudioClip damageSE;
    [Header("�_���[�WSE�̉���")] [SerializeField] [Range(0, 1)] float damageSEVol = 1;
    [Header("�_���[�W�󂯂����̃G�t�F�N�g")] ParticleSystem damageEffect;
    [Header("�R�C���擾���̃G�t�F�N�g")] [SerializeField] ParticleSystem coinEffect;
    [Header("�R�C���擾����SE")] [SerializeField] AudioClip coinSE;
    [Header("�R�C��SE�̉���")] [SerializeField] [Range(0, 1)] float coinSEVol = 1;
    [Header("���C�t�A�b�vSE")] [SerializeField] AudioClip lifeUP;
    [Header("���C�t�A�b�vSE�̉���")] [SerializeField] [Range(0, 1)] float lifeUPSEVol = 1;
    [Header("���C�t�A�b�v�G�t�F�N�g")] [SerializeField] ParticleSystem lifeUPEffect;
    [Header("�ǂꂭ�炢����΍ő�HP�ɂȂ邩")] [SerializeField] public int MaxHPTime;

    //X���x
    private float xSpeed;

    //���������Ԃ��v��
    //���������Ԃ������قǃt���X�r�[�̃��C�t��������
    //�����StageManager�ŎQ�Ƃ����
    [HideInInspector] public float runTime = 0.0f;

    //��Q���ɏՓˎ�������ԕ���
    Vector3 dieBlowDirection;

    //�X�e�[�g
    State currentState;

    //�e��L�[
    private bool rightKey; //�E�ړ�
    private bool leftKey; //���ړ�
    private bool jumpKey; //�W�����v
    private bool shootKey; //�t���X�r�[�𓊂���
    private bool poseKey; //�|�[�Y

    //�e�픻��
    private bool isGround = false; //�ڒn����
    private bool isLifeUP = false;

    //�C���X�^���X
    private StageManager stageManager; // �X�e�[�W�}�l�[�W���[
    private Rigidbody rb;  //���W�b�g�{�f�B
    private Player_HurtBox player_HurtBox; //�����蔻��
    private Animator anim; //�A�j���[�^�[

    void Start()
    {
        //�R���|�[�l���g�擾

        //�X�e�[�W�}�l�[�W���[
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();

        //���W�b�g�{�f�B
        rb = GetComponent<Rigidbody>();

        //�����蔻��擾�X�N���v�g
        player_HurtBox = GetComponent<Player_HurtBox>();

        //�A�j���[�^�[
        anim = GetComponent<Animator>();

        currentState = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        //�L�[����
        rightKey = Input.GetKey(KeyCode.RightArrow);
        leftKey = Input.GetKey(KeyCode.LeftArrow);
        jumpKey = Input.GetKeyDown(KeyCode.Space);
        shootKey = Input.GetKeyDown(KeyCode.F);
        poseKey = Input.GetKeyDown(KeyCode.P);

        Debug.Log(currentState + "�ł�");

        switch (currentState)
        {
            case State.Dead:
                break;

            //������Ԃ̂Ƃ��ARun�ɑJ�ڂ���
            case State.Idle:
                currentState = State.Run;
                break;

            //Run��Ԃ̎�
            case State.Run:
                Move();

                //�W�����v�L�[�������ꂽ��W�����v
                if (jumpKey)
                {
                    Jump();
                }
                //�V���[�g�L�[�������ꂽ��V���[�g
                else if (shootKey)
                {
                    Shoot();
                }
                break;

                //�W�����v���͗������\�b�h
            case State.Jump:
                JumpDown();
                break;

            case State.Shoot:
                break;     
        }

        //P�L�[�Ń|�[�Y
        //�X�e�[�W�}�l�[�W���[�̃|�[�Y���\�b�h���Ăяo���A�ꎞ��~������
        if (poseKey)
        {
            stageManager.Pose();
        }

        //���������Ԃ��v��
        runTime += Time.deltaTime;

        //���������Ԃɉ����ăA�j���[�V�����J�[�u�̒l�����ړ����x�ɓK�p����i���������Ԃ������قǑ��x���オ��j
        zMoveSpeed = runCurve.Evaluate(runTime);

        rb.velocity = new Vector3(xSpeed, rb.velocity.y, zMoveSpeed);
    }

    private void FixedUpdate()
    {
        //�ڒn�����GroundChecker�̃��\�b�h����擾
        isGround = groundChecker.CheckGround();

        //�������ɏd�͂�������
        Gravity();

        if (player_HurtBox.isDeadCheck())
        {
            currentState = State.Dead;
        }
    }

    /// <summary>
    /// �ړ����\�b�h
    /// ���E�L�[�ŉ��ړ�
    /// �O���ւ͎����Ői�ݑ�����
    /// �O���ւ̈ړ����x�̓A�j���[�V�����J�[�u�Őݒ�B
    /// ���������Ԃ������Ōv������
    /// </summary>
    public void Move()
    {
        //�X�e�[�g��ύX
        currentState = State.Run;

        //�A�j���[�^�[�̃u�[����ύX
        SetAnimation(true);

        if (isGround)
        {
            //�E�L�[�ŉE��
            if (rightKey)
            {
                xSpeed = xMoveSpeed;
            }

            //���L�[�ō���
            else if (leftKey)
            {
                xSpeed = -xMoveSpeed;
            }
            //���͖����͍��E�ړ����Ȃ�
            else
            {
                xSpeed = 0.0f;
            }
        }
    }

    /// <summary>
    /// �W�����v���\�b�h
    /// Space�L�[�ŃW�����v
    /// �W�����v���̓t���X�r�[���������Ȃ�
    /// </summary>
    public void Jump()
    {
        //�ڒn���Ă��邩�W�����v�L�[�������ꂽ
        if (isGround)
        {
            //�X�e�[�g��ύX
            currentState = State.Jump;

            //�A�j���[�^�[�̃u�[����ύX
            SetAnimation(true);

            //������֗͂�������
            rb.AddForce(transform.up * jumpSpeed);
        }
    }

    public void JumpDown()
    {
        if (!isGround)
        {
            //�W�����v�t���O��True�����x�����ȉ��ɂȂ�����W�����v��false��
            if (rb.velocity.y < -1.0f && currentState == State.Jump)
            {
                Debug.Log("�W�����v����");

                //�A�j���[�^�[�̃u�[����ύX
                SetAnimation(false);

                //�X�e�[�g��ύX
                currentState = State.Run;
            }
        }
    }

    /// <summary>
    /// �t���X�r�[�𓊂��郁�\�b�h
    /// �W�����v���͓������Ȃ�
    /// </summary>
    public void Shoot()
    {
        //�ڒn���łȂ��Ƃ��������Ȃ�
        if (!isGround)
        {
            return;
        }

        //�X�e�[�g��ύX
        currentState = State.Shoot;

        int frisbeeHP = CaluculateFrisbeeHP((int)runTime);

        //UI��\��
        stageManager.SetUIAndLife(frisbeeHP, isLifeUP);

        Debug.Log(frisbeeHP);

        //�t���X�r�[�𐶐�
        GameObject threwfrisbee = Instantiate(StageSelectManager.selectedFrisbee.SelectedFrisbee, new Vector3(transform.position.x, this.transform.position.y, this.transform.position.z + 2), Quaternion.Euler(new Vector3(90, 90, 0)));
        threwfrisbee.GetComponent<Frisbee>().SetHP(frisbeeHP);

        //���������_�Ńv���C���[���~������
        rb.velocity = Vector3.zero;

        //��������̓X�e�[�W�}�l�[�W���[�̃��\�b�h�Ńv���C���[�̑���𖳌��ɂ��A�t���X�r�[��������ł���悤�ɂ���
        stageManager.StopPlayerControll();

        //�������u��Unity�����͔�\����
        this.gameObject.SetActive(false);
    }


    public void TheDie(int type, Vector3 direction)
    {
        dieBlowDirection = direction;
        currentState = State.Dead;
        StartCoroutine(Die(type));
    }

    /// <summary>
    /// �v���C���[����Q���A�������͌��ɗ������ۂɌĂԃ��\�b�h
    /// </summary>
    /// <returns></returns>
    public IEnumerator Die(int type)
    {
        //���S�ʒu���L�^
        stageManager.deadPos = this.transform.position.z;

        //�A�j���[�V�����Đ�
        PlayDieAnim(type);

        switch (type)
        {
            //���ɗ��������ꍇ
            case 0:

                break;

            //��Q���ɏՓ˂����ꍇ
            case 1:
                //���x�����Z�b�g
                rb.velocity = Vector3.zero;

                //�d�͂�L���ɂ���
                rb.useGravity = true;

                //��Q�����玩���ɂ����Ẵx�N�g��(direction)�ɗ͂�������
                rb.AddForce(dieBlowDirection, ForceMode.Impulse);

                //SE
                SEManager.seManager.PlaySE(damageSEVol, damageSE);
                break;
        }


        //�X�e�[�W�}�l�[�W���[�̃��\�b�h�Ń��g���CUI���Ăяo��
        stageManager.Retry();

        //�X�e�[�W�}�l�[�W���[�̃��\�b�h�Ńv���C���[�̑���𖳌��ɂ���
        stageManager.StopPlayerControll();

        //�~�X������͈�莞�Ԍ�ɃQ�[�����Ԃ��~�߂�i���g���CUI�̔w�i�����������Ȃ�̂�h���j
        yield return new WaitForSeconds(1.0f);

        Time.timeScale = 0.0f;
    }

    /// <summary>
    /// �d�̓��\�b�h
    /// ��ɉ������ɏd�ʂ�������
    /// �X�e�[�W�ɂ���ĕύX�ł���悤�ɂ���
    /// </summary>
    public void Gravity()
    {
        rb.AddForce(new Vector3(0, -gravity, 0), ForceMode.Acceleration);
    }

    /// <summary>
    /// �A�j���[�V�����J�ڗp�̃��\�b�h
    /// currentState��Run�̏�ԂŌĂяo����SetAnimation(true)�ɂ���ƁA�A�j���[�^�[�̃p�����[�^Run��
    /// true�ɂȂ�
    /// </summary>
    public void SetAnimation(bool judge)
    {
        switch (currentState)
        {
            case State.Run:
                anim.SetBool("Run", judge);
                break;

            case State.Jump:
                anim.SetBool("Jump", judge);
                break;
        }

        //�ڒn���Ă���Ƃ�
        anim.SetBool("Ground", isGround);

        //���������Ԃ������قǁA�A�j���[�V��������������
        anim.SetFloat("Speed", zMoveSpeed / 6);
    }

    /// <summary>
    /// �~�X�����ۂ̃A�j���[�V����
    /// ���ɗ������ہA��Q���ɏՓ˂����ۂ̂Q��
    /// ����̓v���C���[�̓����蔻����擾����X�N���v�g����Ăяo��
    /// </summary>
    /// <param name="type"></param>
    public void PlayDieAnim(int type)
    {
        switch (type)
        {
            //����������
            case 0:
                anim.Play("Unity_Chan_Die1");
                break;

            //��Q���ɂ���������
            case 1:
                anim.Play("Unity_Chan_Die2");
                break;
        }
    }

    //�R�C���擾���̃G�t�F�N�g
    //�G�t�F�N�g�̓C���X�y�N�^����ݒ�
    public void PlayCoinEffect()
    {
        SEManager.seManager.PlaySE(coinSEVol, coinSE);
        coinEffect.Play();
    }

    /// <summary>
    /// ���C�t�A�b�v�擾
    /// �t���X�r�[��HP��1������
    /// </summary>

    public void LifeUP()
    {
        //SE
        SEManager.seManager.PlaySE(lifeUPSEVol, lifeUP);
        isLifeUP = true;

        //�G�t�F�N�g
        lifeUPEffect.Play();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="runTime"></param>
    /// <returns></returns>

    public int CaluculateFrisbeeHP(int runTime)
    {
        //���C�t�A�b�v���擾�����Ȃ�P�ǉ�
        var extraHP = 0;

        if (isLifeUP)
        {
            extraHP = 1;
        }

        //���������Ԃ�MaxHPTime�̈��l�ȉ��������ꍇ�ŏ�������
        if (runTime >= MaxHPTime)
        {
            return 3 + extraHP;
        }
        else if (runTime >= MaxHPTime * 2 / 3)
        {
            return 2 + extraHP;
        }
        else
        {
            return 1 + extraHP;
        }
    }
}
