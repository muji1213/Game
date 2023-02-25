using UnityEngine;
using System.Collections;

public abstract class Frisbee : MonoBehaviour
{
    //�t���X�r�[�̈ړ����
    //�Ⴆ�Ώ�L�[���������ꍇUP�ɂȂ�
    public enum State
    {
        UP,
        DOWN,
        RIGHT,
        LEFT,
        RIGHTUP,
        LEFTUP,
        RIGHTDOWN,
        LEFTDOWN
    }

    [SerializeField] float speed; //�c���ړ����x
    [SerializeField] float zSpeed; //���ړ����x
    [SerializeField] float maxZspeed; //���ړ��ő呬�x
    [SerializeField] float minZspeed; //���ړ��Œᑬ�x
    [SerializeField] float rotateSpeed; //��]���x
    [SerializeField] float gravity; //�d��
    [SerializeField] float invincibleTime; //���G����
    [SerializeField] float blinkInterval; //���G���Ԓ��̓_�ŊԊu
    [SerializeField] AnimationCurve speedCurve; //�����]�������ۂ̉����x
    [SerializeField] AnimationCurve acceleteCurve; //�����{�^�����������ۂ̃t���X�r�[�̑��x
    [Header("�_���[�W�󂯂�����SE")] [SerializeField] AudioClip damageSE;
    [Header("�_���[�WSE�̉���")] [SerializeField] [Range(0, 1)] float damageSEVol = 1;
    [Header("�_���[�W�󂯂����̃G�t�F�N�g")] [SerializeField] GameObject damageEffect;
    [Header("�R�C���擾���̃G�t�F�N�g")] [SerializeField] ParticleSystem coinEffect;
    [Header("�R�C���擾����SE")] [SerializeField] AudioClip coinSE;
    [Header("�R�C���擾SE�̉���")] [SerializeField] [Range(0, 1)] float coinSEVol = 1;
    [Header("����������SE")] [SerializeField] AudioClip throwSE;
    [Header("����������SE�̉���")][SerializeField] [Range(0, 1)] float throwSEVol = 1;
    [Header("���ʉ��p�̃I�[�f�B�I�\�[�X")] [SerializeField] AudioSource seAudioSource;

    private int HP; //HP
    private float moveTime; //��~�A�������͕����]�����Ă���̌o�ߎ���
    private float moveSpeed; //�ړ����x

    private float invincibleTimer; //���G�o�ߎ���
    private float blinkTimer; //���G���ԓ_�ł̃^�C�}�[
    private float acceleteTime; //�����{�^���������Ă��鎞��
    private float throwSpeed;�@//run�X�e�[�W����X�^�[�g�n�_�ɂ��܂ł̑���
    private Vector3 toVector; //�X�^�[�g�n�_�܂ł̃x�N�g��
    private GameObject StartPos; //�X�^�[�g�n�_
    private ParticleSystem speedEff; //�W�����̃G�t�F�N�g

    //�L�[����
    private bool upKey; //��ړ�
    private bool downKey; //���ړ�
    private bool leftKey; //���ړ�
    private bool rightKey; //�E�ړ�
    private bool rotateRKey; //�E��]
    private bool rotateLKey; //����]
    private bool poseKey; //�|�[�Y
    private bool acceleteKey; //����
    private bool dodgeKey; //���

    //����
    private bool isStart = false; //�t���X�r�[���J�n�ʒu�ɂ������ǂ���(StartPos�ɂ�����true)
    private bool isDead = false; //���S����
    protected bool isInvincible = false; //���G���ǂ���(��Q���ɏՓˌ㖳�G���Ԃ�t����)
    private bool isRetryed = false; //���U���gUI��������Ăяo�����̂�h��

    private State prevState = State.RIGHT; //�O�̏��
    private State currentState; //���ݏ��

    //��Ԉُ�
    //���̉e�����󂯂邩
    public bool isBlowed;

    //�C���X�^���X
    protected Rigidbody rb; //���W�b�g�{�f�B
    private CameraFollower cameraFollower; //�J������Ǐ]������X�N���v�g
    private Frisbee_HurtBox frisbee_Hurt; //�����蔻��
    private Renderer render; //�����_���[
    private StageManager stageManager; //�X�e�[�W�}�l�[�W���[(�|�[�Y�̌Ăяo���A���g���CUI�̌Ăт����ɕK�v)


    void Start()
    {
        //�R���|�[�l���g�擾

        //�X�e�[�W�}�l�[�W���[
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();

        //���W�b�g�{�f�B
        rb = GetComponent<Rigidbody>();

        //�����_�[
        render = GetComponent<Renderer>();

        //�����蔻��
        frisbee_Hurt = GetComponent<Frisbee_HurtBox>();

        //�t���X�r�[���ړ����ɉ�ʒ[�ɏW������\�������A�X�s�[�h�����o��
        speedEff = GameObject.Find("Canvas/SpeedEffect").GetComponent<ParticleSystem>();
        speedEff.Play();

        //�J�n�ʒu���擾����
        //�t���X�r�[�����̈ʒu�ɂ����瑀�삪�\�ɂȂ�
        StartPos = GameObject.Find("StartPos");

        //�ŏ�z�ړ����x
        minZspeed = zSpeed;

        //�J�����Ǐ]�p�̃X�N���v�g
        cameraFollower = GameObject.Find("Main Camera").GetComponent<CameraFollower>();

        //�J�����̒Ǐ]�Ώۂ�Unity����񂩂�t���X�r�[�ɕς���
        cameraFollower.ChangeTarget(this.gameObject);

        //�J�����̈ʒu����
        cameraFollower.ZoomToTarget(-5, 0, new Vector3(0, 0, 0));

        //�X�^�[�g�n�_�ɂ��܂ł̑������v�Z
        throwSpeed = (StartPos.transform.position.z - this.transform.position.z) / 1.5f;

        //SE
        SEManager.seManager.PlaySE(throwSEVol,throwSE);
    }


    void Update()
    {
        //�L�[����
        upKey = Input.GetKey(KeyCode.UpArrow);
        downKey = Input.GetKey(KeyCode.DownArrow);
        leftKey = Input.GetKey(KeyCode.LeftArrow);
        rightKey = Input.GetKey(KeyCode.RightArrow);
        rotateRKey = Input.GetKey(KeyCode.R);
        rotateLKey = Input.GetKey(KeyCode.W);
        poseKey = Input.GetKeyDown(KeyCode.P);
        acceleteKey = Input.GetKey(KeyCode.F);
        dodgeKey = Input.GetKeyDown(KeyCode.A);

        if (dodgeKey)
        {
            Dodge();
        }


        //P�L�[���͂Ń|�[�Y
        //�X�e�[�W�}�l�[�W���[�̃��\�b�h��UI�ƈꎞ��~������
        if (poseKey)
        {
            stageManager.Pose();
        }
    }

    protected void FixedUpdate()
    {

        //���S��
        if (isDead)
        {
            Gravity();
            return;
        }

        //Run�X�e�[�W����X�^�[�g�n�_�܂Ńt���X�r�[���΂�
        if (Vector3.Distance(this.gameObject.transform.position, StartPos.transform.position) > 0.1f && !isStart)
        {
            //�X�^�[�g�n�_�܂ł̃x�N�g���𐶐�
            toVector = Vector3.MoveTowards(transform.position, StartPos.transform.position, throwSpeed * Time.deltaTime);

            //�����x�N�g�������Ƃɔ�΂�
            rb.MovePosition(toVector);
        }
        else
        {
            //�J�n�ʒu�Ɉ�苗���܂ŋ߂Â�����X�^�[�g�t���O��true
            isStart = true;

            //�J�������t���X�r�[�̐^���ɌŒ肷��
            //�Œ肷�邱�ƂŃJ�����̕`��͈͊O�Ɉړ��ł��Ȃ��悤�ɂ���
            cameraFollower.FixCamera(StartPos);
        }

        if (isStart)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, zSpeed);

            //���S������擾
            isDead = frisbee_Hurt.isDeadCheck();

            //�e�탁�\�b�h
            Move();
            Rotate();
            Clamp();
            Accelerate();


            //���G��
            if (isInvincible)
            {
                if (invincibleTimer < invincibleTime)
                {
                    //�_�Ŏ��ԃ^�C�}�[
                    blinkTimer += Time.deltaTime;

                    //���G���_�ł�����
                    //�C���^�[�o���𒴂�����
                    if (blinkTimer > blinkInterval)
                    {
                        //�\����\���𔽓]
                        render.enabled = !render.enabled;

                        //�^�C�}�[���Z�b�g
                        blinkTimer = 0.0f;
                    }

                    //���C���[���ق��̃��C���[�ɂ��邱�ƂŁA��Q���Ƃ̏Փ˂����������
                    this.gameObject.layer = 10;

                    //���G���Ԍv��
                    invincibleTimer += Time.deltaTime;
                }
                else
                {
                    //���G�I����A���C���[��߂�
                    this.gameObject.layer = 9;

                    //�^�C�}�[���Z�b�g
                    invincibleTimer = 0.0f;

                    //�F��߂�
                    render.enabled = true;

                    //�t���O��߂�
                    isInvincible = false;
                }
            }
        }
    }

    /// <summary>
    /// �ړ��p���\�b�h
    /// </summary>
    public void Move()
    {
        moveSpeed = speed;
        if (upKey && rightKey)
        {
            currentState = State.RIGHTUP;
        }
        else if (upKey && leftKey)
        {
            currentState = State.LEFTUP;
        }
        else if (downKey && rightKey)
        {
            currentState = State.RIGHTDOWN;
        }
        else if (downKey & leftKey)
        {
            currentState = State.LEFTDOWN;
        }
        else if (upKey)
        {
            currentState = State.UP;
        }
        else if (downKey)
        {
            currentState = State.DOWN;
        }
        else if (leftKey)
        {
            currentState = State.LEFT;
        }
        else if (rightKey)
        {
            currentState = State.RIGHT;
        }
        else
        {
            //�������͂��Ă��Ȃ��ꍇ�A0
            moveSpeed = 0.0f;
            moveTime = 0.0f;
        }

        //�O��̏�Ԃƈ�����瑬�x�����Z�b�g����
        if (prevState != currentState)
        {
            moveTime = 0f;
        }
        else
        {
            //�����{�^�������������Ă���΁AmoveTime�����Z�����
            moveTime += Time.deltaTime;
        }

        //�A�j���[�V�����J�[�u�Ɋ�Â��ĉ���������
        //moveTime�������Ȃ�قǁA���x���オ��悤�ɃA�j���[�V�����J�[�u��ݒ�
        moveSpeed *= speedCurve.Evaluate(moveTime);

        //�e�����ړ�
        switch (currentState)
        {
            case State.UP:
                rb.velocity = new Vector3(0, moveSpeed, rb.velocity.z);
                break;
            case State.DOWN:
                rb.velocity = new Vector3(0, -moveSpeed, rb.velocity.z);
                break;
            case State.LEFT:
                rb.velocity = new Vector3(-moveSpeed, 0, rb.velocity.z);
                break;
            case State.RIGHT:
                rb.velocity = new Vector3(moveSpeed, 0, rb.velocity.z);
                break;
            case State.RIGHTUP:
                rb.velocity = new Vector3(moveSpeed, moveSpeed, rb.velocity.z);
                break;
            case State.LEFTUP:
                rb.velocity = new Vector3(-moveSpeed, moveSpeed, rb.velocity.z);
                break;
            case State.RIGHTDOWN:
                rb.velocity = new Vector3(moveSpeed, -moveSpeed, rb.velocity.z);
                break;
            case State.LEFTDOWN:
                rb.velocity = new Vector3(-moveSpeed, -moveSpeed, rb.velocity.z);
                break;
        }

        //�O��̏�Ԃ��L�^
        prevState = currentState;
    }

    /// <summary>
    /// ��]������
    /// </summary>
    public void Rotate()
    {
        //R�L�[�ŉE�ɉ�]
        if (rotateRKey)
        {
            this.transform.Rotate(rotateSpeed * Time.deltaTime, 0, 0);
        }

        //L�L�[�ō���]
        if (rotateLKey)
        {
            this.transform.Rotate(-rotateSpeed * Time.deltaTime, 0, 0);
        }
    }

    /// <summary>
    /// �������\�b�h
    /// F�L�[���������x���㏸�i��������葬�x�ȏ�ɂȂ�Ȃ��j
    /// </summary>
    public void Accelerate()
    {
        //�����L�[������
        if (acceleteKey)
        {
            //�������Ԃɉ��Z���Ă���
            acceleteTime += Time.deltaTime;

            //�A�j���[�V�����J�[�u��0����n�܂�E���オ��ɂ��AacceleteTime����������قǁAzSpeed���オ��悤�ɂ���
            //�A�j���[�V�����J�[�u��K�p����̂ł͂Ȃ��A���Z���邱��
            zSpeed += acceleteCurve.Evaluate(acceleteTime);
        }
        else
        {
            //�L�[�𗣂����ꍇ�A�������Ԃ����X�Ɍ������Ă���
            acceleteTime -= Time.deltaTime;

            //������0�ȉ��ɂȂ�Ȃ�
            if (acceleteTime < 0)
            {
                acceleteTime = 0.0f;
            }

            //zspeed���猸�Z����
            //���̍��������猸�Z���Ă������߁A�}���ɑ��x��������悤�ɂ��Ă���
            zSpeed -= acceleteCurve.Evaluate(acceleteTime);
        }

        //zSpeed��maxZspeed������Ȃ��悤�ɂ���
        if (zSpeed > maxZspeed)
        {
            zSpeed = maxZspeed;
        }

        //zSpeed��minZspeed�������Ȃ��悤�ɂ���
        else if (zSpeed < minZspeed)
        {
            zSpeed = minZspeed;
        }
    }

    public void TheDie(int type)
    {
        StartCoroutine(Die(type));
    }

    /// <summary>
    /// ���S���ɌĂяo��
    /// </summary>
    private IEnumerator Die(int type)
    {
        if (isRetryed)
        {
            yield break;
        }


        //���S�ʒu���L�^
        stageManager.deadPos = this.transform.position.z;

        //�W����������
        speedEff.Stop();

        //�J�������~�߂�
        cameraFollower.StopCamera();

        switch (type)
        {
            case 0:
                Time.timeScale = 0.0f;

                yield return new WaitForSecondsRealtime(0.5f);

                Time.timeScale = 1.0f;

                break;

            case 1:
                break;
        }

        //�������ɗ��Ƃ��A�t�F�[�h�A�E�g������
        rb.AddForce(new Vector3(0, -0.5f, -0.5f), ForceMode.Impulse);

        //���g���CUI��\��
        stageManager.Retry();

        isRetryed = true;

        yield return new WaitForSeconds(2);

        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// �ړ��͈͂��J�����͈͓��Ɏ��߂�
    /// </summary>
    public void Clamp()
    {
        Vector3 pos = transform.position;

        float distance = pos.z - Camera.main.transform.position.z;
        // ��ʍ����̃��[���h���W���r���[�|�[�g����擾
        Vector3 min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));

        // ��ʉE��̃��[���h���W���r���[�|�[�g����擾
        Vector3 max = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, distance));

        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        transform.position = pos;
    }

    /// <summary>
    // �t���X�r�[�����C�t�O�ɂȂ����Ƃ��ɌĂяo��
    //  �d�͂����ɋ��������邱�ƂŁA��ʂ���t�F�[�h�A�E�g������
    //�@����ȊO�̎��͏d�͂������Ȃ�
    /// </summary>
    public void Gravity()
    {
        if (isDead)
        {
            gravity = 20;
        }
        else
        {
            gravity = 0;
        }
        rb.AddForce(new Vector3(0, -gravity, 0), ForceMode.Acceleration);
    }

    /// <summary>
    /// //���C�t�����炷
    /// </summary>
    public void ReduceLife()
    {
        SEManager.seManager.PlaySE(damageSEVol, damageSE);
        HP -= 1;
        stageManager.ReduceHPUI();
    }

    /// <summary>
    /// ���G���\�b�h
    /// </summary>
    /// <param name="time">���G����</param>
    public void Invincible(int time)
    {
        //���G���Ԃ������̎��Ԃɂ���
        invincibleTime = time;

        //���G�t���O��true��
        isInvincible = true;
    }

    //�R�C���擾���̃G�t�F�N�g
    //�G�t�F�N�g�̓C���X�y�N�^����ݒ�
    public void PlayCoinEffect()
    {
        SEManager.seManager.PlaySE(coinSEVol, coinSE);
        coinEffect.Play();
    }

    public void PlayDamageEffect(Vector3 pos)
    {
        GameObject damageEffect = Instantiate(this.damageEffect, pos, Quaternion.identity);
        Destroy(damageEffect, damageEffect.GetComponent<ParticleSystem>().main.duration);
    }

    /// <summary>
    ///    //HP���Z�b�g����
    /// </summary>
    /// <param name="HP"></param>
    public void SetHP(int HP)
    {
        this.HP = HP;
    }

    /// <summary>
    /// HP��Ԃ�
    /// </summary>
    public int GetHP
    {
        get { return HP; }
    }

    protected abstract void Dodge();
}
