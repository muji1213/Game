using System.Collections;
using UnityEngine;


//���̃X�N���v�g�̓t���X�r�[�𓊂���O�̃v���C���[�̃X�N���v�g�ł�
public class Player_Controller : MonoBehaviour
{
    //�ϐ�
    [Header("���ړ����x")] [SerializeField] float zSpeed;
    [Header("���ړ����x")] [SerializeField] float xSpeed;
    [Header("�W�����v���x")] [SerializeField] float jumpSpeed;
    [Header("�d��")] [SerializeField] float gravity;
    [Header("�t���X�r�[")] [SerializeField] GameObject frisbee;
    [Header("�v���C���[�̉����x")] [SerializeField] AnimationCurve runCurve;
    [Header("�ڒn����`�F�b�N")] [SerializeField] GroundChecker groundChecker;
    [Header("�_���[�W�󂯂�����SE")] [SerializeField] AudioClip damageSE;
    [Header("�_���[�W�󂯂����̃G�t�F�N�g")] ParticleSystem damageEffect;
    [Header("�R�C���擾���̃G�t�F�N�g")] [SerializeField] ParticleSystem coinEffect;
    [Header("�R�C���擾����SE")] [SerializeField] AudioClip coinSE;
    [Header("����SE")] [SerializeField] AudioClip runSE;
    [Header("���C�t�A�b�vSE")] [SerializeField] AudioClip lifeUP;
    [Header("���C�t�A�b�v�G�t�F�N�g")] [SerializeField] ParticleSystem lifeUPEffect;
    [Header("�ǂꂭ�炢����΍ő�HP�ɂȂ邩")] [SerializeField] public int MaxHPTime;

    //���������Ԃ��v��
    //���������Ԃ������قǃt���X�r�[�̃��C�t��������
    //�����StageManager�ŎQ�Ƃ����
    [HideInInspector] public float runTime = 0.0f;

    //�e��L�[
    private bool rightKey; //�E�ړ�
    private bool leftKey; //���ړ�
    private bool jumpKey; //�W�����v
    private bool shootKey; //�t���X�r�[�𓊂���
    private bool poseKey; //�|�[�Y

    //�e�픻��
    private bool isGround = false; //�ڒn����
    private bool isRun = false; //�����Ă��邩�ǂ���
    private bool isJump = false; //�W�����v�����ǂ���
    private bool isShoot = false; //�t���X�r�[�𓊂������ǂ���
    private bool isLifeUP = false;

    //�C���X�^���X
    private StageManager stageManager; // �X�e�[�W�}�l�[�W���[
    private Rigidbody rb;  //���W�b�g�{�f�B
    private Player_HurtBox player_HurtBox; //�����蔻��
    private Animator anim; //�A�j���[�^�[
    private AudioSource audioSource;

    void Start()
    {
        //�R���|�[�l���g�擾
        audioSource = GetComponent<AudioSource>();

        //�X�e�[�W�}�l�[�W���[
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();

        //���W�b�g�{�f�B
        rb = GetComponent<Rigidbody>();

        //�����蔻��擾�X�N���v�g
        player_HurtBox = GetComponent<Player_HurtBox>();

        //�A�j���[�^�[
        anim = GetComponent<Animator>();
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

        Jump();

        //F�L�[�Ńt���X�r�[�𓊂���
        if (shootKey)
        {
            Shoot();
        }

        //P�L�[�Ń|�[�Y
        //�X�e�[�W�}�l�[�W���[�̃|�[�Y���\�b�h���Ăяo���A�ꎞ��~������
        if (poseKey)
        {
            stageManager.Pose();
        }
    }

    private void FixedUpdate()
    {
        if (!audioSource.enabled)
        {
            audioSource.enabled = true;
        }
        audioSource.pitch = zSpeed / 3;

        //�ڒn�����GroundChecker�̃��\�b�h����擾
        isGround = groundChecker.CheckGround();

        //�������ɏd�͂�������
        Gravity();

        //�A�j���[�V�����̑J�ڂ�����
        SetAnimation();

        if (!player_HurtBox.isDeadCheck())
        {
            //�t���X�r�[�𓊂��Ă��Ȃ���Έړ��𑱂���
            if (!isShoot)
            {
                Move();
            }
            else
            {
                return;
            }
        }
    }

    /// <summary>
    /// �ړ����\�b�h
    /// ���E�L�[�ŉ��ړ�
    /// �O���ւ͎����Ői�ݑ�����
    /// �O���ւ̈ړ����x�̓A�j���[�V�����J�[�u�Őݒ�B
    /// ���������Ԃ������Ōv������
    /// </summary>
    private void Move()
    {
        //���������Ԃ��v��
        runTime += Time.deltaTime;

        //���������Ԃɉ����ăA�j���[�V�����J�[�u�̒l�����ړ����x�ɓK�p����i���������Ԃ������قǑ��x���オ��j
        zSpeed = runCurve.Evaluate(runTime);

        //�ڒn���Ă���Ƃ��ɂ������ړ��ł��Ȃ�
        if (isGround)
        {
            isRun = true;

            //�E�L�[�ŉE��
            if (rightKey)
            {
                rb.velocity = new Vector3(xSpeed, rb.velocity.y, zSpeed);
            }

            //���L�[�ō���
            else if (leftKey)
            {
                rb.velocity = new Vector3(-xSpeed, rb.velocity.y, zSpeed);
            }
            //���͖����͍��E�ړ����Ȃ�
            else
            {
                rb.velocity = new Vector3(0, rb.velocity.y, zSpeed);
            }
        }
    }

    /// <summary>
    /// �W�����v���\�b�h
    /// Space�L�[�ŃW�����v
    /// �W�����v���̓t���X�r�[���������Ȃ�
    /// </summary>
    private void Jump()
    {
        //�ڒn���Ă��邩�W�����v�L�[�������ꂽ
        if (jumpKey && isGround)
        {
            //�W�����v�t���O��True��
            isJump = true;

            //������֗͂�������
            rb.AddForce(transform.up * jumpSpeed);
        }

        //�W�����v�t���O��True�����x�����ȉ��ɂȂ�����W�����v��false��
        if (rb.velocity.y < -5.0f && isJump)
        {
            isJump = false;
        }
    }

    /// <summary>
    /// �t���X�r�[�𓊂��郁�\�b�h
    /// �W�����v���͓������Ȃ�
    /// </summary>
    private void Shoot()
    {
        //�ڒn���łȂ��Ƃ��������Ȃ�
        if (!isGround)
        {
            return;
        }

        int frisbeeHP = CaluculateFrisbeeHP((int)runTime);

        //UI��\��
        stageManager.SetUI(frisbeeHP);

        Debug.Log(frisbeeHP);

        //�t���X�r�[�𐶐�
        GameObject threwfrisbee = Instantiate(StageSelectManager.selectedFrisbee.SelectedFrisbee, new Vector3(transform.position.x, this.transform.position.y, this.transform.position.z + 2), Quaternion.Euler(new Vector3(90, 90, 0)));
        threwfrisbee.GetComponent<Frisbee>().SetHP(frisbeeHP);

        //�t���X�r�[�̓������t���O��True��
        isShoot = true;

        //���������_�Ńv���C���[���~������
        rb.velocity = Vector3.zero;

        //��������̓X�e�[�W�}�l�[�W���[�̃��\�b�h�Ńv���C���[�̑���𖳌��ɂ��A�t���X�r�[��������ł���悤�ɂ���
        stageManager.StopPlayerControll();

        //
        this.gameObject.SetActive(false);
    }


    public void TheDie(int type, Vector3 direction)
    {
        StartCoroutine(Die(type, direction));
    }

    /// <summary>
    /// �v���C���[����Q���A�������͌��ɗ������ۂɌĂԃ��\�b�h
    /// </summary>
    /// <returns></returns>
    private IEnumerator Die(int type, Vector3 direciton)
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
                rb.velocity = Vector3.zero;
                rb.useGravity = true;
                rb.AddForce(direciton, ForceMode.Impulse);
                SEManager.seManager.PlaySe(damageSE);
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
    private void Gravity()
    {
        rb.AddForce(new Vector3(0, -gravity, 0), ForceMode.Acceleration);
    }

    /// <summary>
    /// �A�j���[�V�����J�ڗp�̃��\�b�h
    /// �e��bool�őJ�ڂ����Ă���
    /// </summary>
    private void SetAnimation()
    {
        //�����Ă���Ƃ�
        anim.SetBool("Run", isRun);

        //�ڒn���Ă���Ƃ�
        anim.SetBool("Ground", isGround);

        //�W�����v���Ă���Ƃ�
        anim.SetBool("Jump", isJump);

        //���������Ԃ������قǁA�A�j���[�V��������������
        anim.SetFloat("Speed", zSpeed / 6);
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
        SEManager.seManager.PlaySe(coinSE);
        coinEffect.Play();
    }

    /// <summary>
    /// ���C�t�A�b�v�擾
    /// �t���X�r�[��HP��1������
    /// </summary>

    public void LifeUP()
    {
        //SE
        SEManager.seManager.PlaySe(lifeUP);
        isLifeUP = true;

        //�G�t�F�N�g
        lifeUPEffect.Play();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="runTime"></param>
    /// <returns></returns>

    private int CaluculateFrisbeeHP(int runTime)
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
