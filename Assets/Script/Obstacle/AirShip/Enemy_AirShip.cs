using UnityEngine;
using Cinemachine;

public class Enemy_AirShip : MonoBehaviour, IMovable
{
    [HideInInspector]
    private enum AttackType
    {
        NormalAttack,
        ChargeAttack,
        BombAttack,
    }

    public enum State
    {
        Idle,
        Start,
        Middle,
        End
    }

    [SerializeField] [Header("�t���X�r�[�̐N������")] BossTrigger checkFrisbee;
    [SerializeField] [Header("doll cart")] CinemachineDollyCart dollyCart;
    [SerializeField] [Header("�o�ꎞ�p�X")] CinemachineSmoothPath startPath;
    [SerializeField] [Header("�U���J�n����I���܂Ŏg���ړ��p�X")] CinemachineSmoothPath movePath;
    [SerializeField] [Header("�Ə�UI")] GameObject rockOnUI;
    [SerializeField] [Header("�ʏ�U���̋�")] GameObject bullet_Normal;
    [SerializeField] [Header("�`���[�W�U���̋�")] GameObject bullet_Charge;
    [SerializeField] [Header("��sSE�Đ��pAudioSource")] AudioSource flySource;
    [SerializeField] [Header("�ʏ�U��SE")] AudioClip normalAttackSE;
    [SerializeField] [Header("�ʏ�U����SE�̉���")] [Range(0, 1)] float normalAttackSEVol = 1;
    [SerializeField] [Header("�{���ݒuSE")] AudioClip bombInstalledSE;
    [SerializeField] [Header("�{���ݒuSE�̉���")] [Range(0, 1)] float bombInstalledSEVol = 1;
    [SerializeField] [Header("�`���[�WSE")] AudioClip chargeSE;
    [SerializeField] [Header("�`���[�WSE�̉���")] [Range(0, 1)] float chargeSEVol = 1;
    [SerializeField] [Header("�`���[�W�U��SE")] AudioClip leserSE;
    [SerializeField] [Header("�`���[�W�U��SE�̉���")] [Range(0, 1)] float leserSEVol = 1;
    [SerializeField] [Header("���S���̉�")] ParticleSystem smoke;

    //�t���X�r�[�̃��W�b�g�{�f�B
    Rigidbody frisbeeRb;

    //�X�e�[�g
    private AttackType attackType;
    public State currentState;

    //�A�j���[�^�[
    [SerializeField] [Header("�A�j���[�^�[")] private Animator anim;

    //�o�ꎞ�̃X�s�[�h
    [SerializeField] [Header("�{�X�g���K�[�𓥂�ł���K��ʒu�ɓ�������܂ł̃X�s�[�h")] private float startSpeed;

    //�t���X�r�[�̃Q�[���I�u�W�F�N�g
    private GameObject frisbee;

    //�ʏ�U��------------------------------------

    //���b�N�I���̎���
    [SerializeField] [Header("�Ə����߂Ă��猂�܂ł̎���")] private float rockOnTime;

    //���b�N�I���^�C�}�[
    private float rockOnTimer = 0.0f;

    //�U���܂ł̃f�B���C
    [SerializeField] [Header("���b�N�I�����Ԃ��I�����Ă�����ۂɒe�𔭎˂���܂ł̃f�B���C")] private float attackDuration;

    //�U�����̃G�t�F�N�g
    [SerializeField] ParticleSystem effect1;
    [SerializeField] ParticleSystem effect2;

    //�ʏ�U���̔��ˈʒu
    [SerializeField] [Header("�ʏ�U�����ˈʒu")] GameObject weapon_Normal;

    //�ʏ�U���̎c������
    [SerializeField] [Header("�ʏ�U���̎c������")] float normalAttackDestroyTime;


    //-----------------------------------------

    //�`���[�W�U��---------------------------------------------

    //�`���[�W�U���̕K�v�U����
    [SerializeField] [Header("�`���[�W�U���ɕK�v�ȍU���񐔂�")] private int chargeAttackNum;

    //�U����
    private int attackNum = 1;

    //�`���[�W�U���̌p������
    [SerializeField] [Header("�`���[�W�U���̌p�����Ԃ�")] private int chargeAttackTime;

    [SerializeField] [Header("�`���[�W�U�����肪�o��܂ł̎���")] private float chargeAttackDelay;

    //�`���[�W�U���̌o�ߎ���
    private float chargeAttackTimer = 0.0f;


    //�n��------------------------------------------------------------
    [SerializeField] [Header("�������e�̃I�u�W�F�N�g")] GameObject limitBomb;

    //�n���̕K�v�U����
    [SerializeField] [Header("�������e�U���ɕK�v�ȍU����")] private int bombAttackNum;

    //�n���U�����莞�ԍU�����s��Ȃ�����
    [SerializeField] [Header("�������e�U����U�����s��Ȃ�����")] private float bombAttackCoolTime;

    //�U���N�[���^�C���̃^�C�}�[
    private float bombAttackCoolTimer = 0.0f;

    //�{���U���̎c������
    [SerializeField] [Header("���e�̎c������")] float bombDestoryTime;

    //------------------------------------------------------------

    //����P�̈ʒu
    [SerializeField] GameObject weapon1;

    //����2�̈ʒu
    [SerializeField] GameObject weapon2;

    //����R
    [SerializeField] GameObject weapon3;

    //����S
    [SerializeField] GameObject weapon4;

    //�������e�����ʒu
    [SerializeField] GameObject bombWeapon;

    //UI�̃A�j���[�^�[
    [SerializeField] Animator UIanimator;

    private void Start()
    {
        flySource.volume *= (SEManager.I.seVol);

        //�U���^�C�v�͒ʏ�U���ɂ��Ă���
        attackType = AttackType.NormalAttack;

        //�X�e�[�g��Idle
        currentState = State.Idle;

        //����ɗ��ߍU�������Ȃ��悤�A1����n�߂�
        attackNum = 1;

        //�ʒu�ƃp�X��������
        dollyCart.m_Path = startPath;
        dollyCart.m_Position = 0;
    }

    private void Update()
    {
        //�A�j���[�V����
        SetAnim();
        SetUIAnim();

        Move();

        //�U���Ԑ��̎�
        if (currentState == State.Start || currentState == State.Middle)
        {
            //�t���X�r�[���ė������ꍇ�A�U����~����
            if (frisbee.GetComponent<Frisbee>().GetHP == 0)
            {
                currentState = State.End;
            }

            //�X�s�[�h����Ƀt���X�r�[�𓯂��ɂ���
            dollyCart.m_Speed = -frisbeeRb.velocity.z;

            //�`���[�W�U���񐔂ƁA�{���U���񐔂������ɋN�����ꍇ
            if (attackNum % chargeAttackNum == 0 && attackNum % bombAttackNum == 0)
            {
                //�{���D��               
                attackType = AttackType.BombAttack;
            }

            //�U���񐔂��{���U���񐔂ɂȂ�����
            else if (attackNum % bombAttackNum == 0)
            {
                //�{���U����Ԃ�
                attackType = AttackType.BombAttack;
            }
            else if (attackNum % chargeAttackNum == 0)
            {
                //�`���[�W�U����Ԃɂ���
                attackType = AttackType.ChargeAttack;
            }
            else
            {
                //����ȊO�͒ʏ�U��
                attackType = AttackType.NormalAttack;
            }

            switch (attackType)
            {
                //�{���U��
                case AttackType.BombAttack:

                    //�{����̍U�����s��Ȃ����Ԃ��߂�����
                    if (bombAttackCoolTimer >= bombAttackCoolTime)
                    {
                        //�U���񐔂��X�V
                        attackNum += 1;

                        //�^�C�}�[���Z�b�g
                        bombAttackCoolTimer = 0;
                        return;
                    }
                    else if (bombAttackCoolTimer == 0)
                    {
                        BombAttack();
                    }

                    bombAttackCoolTimer += Time.deltaTime;
                    break;

                //�`���[�W�U��
                case AttackType.ChargeAttack:

                    //�`���[�W�U�����Ԃ��߂�����
                    if (chargeAttackTimer >= chargeAttackTime)
                    {
                        //�U���񐔂��X�V
                        attackNum += 1;

                        //�^�C�}�[���Z�b�g
                        chargeAttackTimer = 0;
                        return;
                    }
                    else if (chargeAttackTimer == 0)
                    {
                        ChargeAttack();
                    }

                    chargeAttackTimer += Time.deltaTime;
                    break;

                //�ʏ�U��
                case AttackType.NormalAttack:

                    //���b�N�I���^�C�}�[��i�߂�
                    rockOnTimer += Time.deltaTime;

                    //���Ԃ𓞒B���Ă�����ˌ����x���㏸����
                    if (currentState == State.Middle)
                    {
                        rockOnTimer += Time.deltaTime;
                    }

                    //���b�N�I���^�C�}�[���K��l�𒴂���܂Ń��b�N�I������
                    if (rockOnTimer >= rockOnTime)
                    {
                        Invoke("Attack", attackDuration);
                    }
                    else if (rockOnTimer >= rockOnTime * 0.4f)
                    {
                        //���l�ȏネ�b�N�I������ƏƏ����\�������
                        NormalAttackRockOn();
                    }
                    break;
            }
        }
    }

    public void Move()
    {
        //�t���X�r�[�����m������ړ�����
        if (checkFrisbee.CheckEnterFrisbee() && currentState == State.Idle)
        {
            flySource.Play();
            dollyCart.m_Speed = startSpeed;

            //�t���X�r�[���擾
            frisbee = GameObject.FindWithTag("Frisbee");
            frisbeeRb = frisbee.GetComponent<Rigidbody>();
        }

        //�X�^�[�g�ʒu�ɂ�����p�X��ύX
        if (dollyCart.m_Position == startPath.PathLength && Mathf.Abs(this.transform.position.z - GameObject.FindWithTag("Frisbee").transform.position.z) < 20f)
        {
            //�t���O�����낷
            currentState = State.Start;

            //�p�X��ύX
            dollyCart.m_Path = movePath;

            //�p�X�̏����ʒu�Ɉړ�����
            dollyCart.m_Position = movePath.PathLength;          
        }

        //�I���ʒu�ɂ�����
        if (dollyCart.m_Path == movePath && currentState == State.End)
        {
            rockOnUI.SetActive(false);
            dollyCart.m_Speed = -frisbeeRb.velocity.z;
        }

        //�p�X�̍ŏI�n�_�܂ōs�������A�N�e�B�u��
        if (dollyCart.m_Path == movePath && dollyCart.m_Position == 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    //���b�N�I�����A�Ə���\������
    private void NormalAttackRockOn()
    {
        //�Ə����\������ĂȂ��̂ŕ\��
        if (!rockOnUI.activeSelf)
        {
            rockOnUI.SetActive(true);
        }

        UIanimator.SetTrigger("RockOn");
    }

    //�ʏ�U��
    private void Attack()
    {
        SEManager.I.PlaySE(normalAttackSEVol, normalAttackSE);

        //�U���Ɠ����Ƀ^�C�}�[���Z�b�g
        rockOnTimer = 0.0f;

        //�U���Ɠ����ɏƏ�������
        rockOnUI.SetActive(false);

        //�U���񐔍X�V
        attackNum += 1;

        //�A�j���[�V����
        anim.SetTrigger("NormalAttack");

        //���ˌ�����e����o
        GameObject bullet1 = Instantiate(bullet_Normal, weapon_Normal.transform.position, Quaternion.identity);
        Bullet_Normal bullet_normal1 = bullet1.GetComponent<Bullet_Normal>();
        bullet_normal1.TargetPos = frisbee.transform.position;
        bullet_normal1.DestroyTime = normalAttackDestroyTime;
        bullet_normal1.AirShip = this;
        effect1.Play();
    }

    //�`���[�W�U��
    private void ChargeAttack()
    {
        SEManager.I.PlaySE(chargeSEVol, chargeSE);

        anim.SetTrigger("ChargeAttack");

        //�`���[�W�U���I�u�W�F�N�g���A����1���琶��
        GameObject leser1 = Instantiate(bullet_Charge, weapon1.transform.position, Quaternion.identity);
        leser1.transform.SetParent(weapon1.transform);
        Bullet_Charge bullet_Charge1 = leser1.GetComponent<Bullet_Charge>();

        //�`���[�W�U���̌p�����ԁA����ї��ߊJ�n������ۂɍU�����肪�o��܂ł̎��Ԃ�n���i�ݒ�̓C���X�y�N�^�Łj
        bullet_Charge1.DestroyTime = chargeAttackTime;
        bullet_Charge1.AttackDelay = chargeAttackDelay;
        bullet_Charge1.AirShip = this;

        //�`���[�W�U���I�u�W�F�N�g���A����2���琶��
        GameObject leser2 = Instantiate(bullet_Charge, weapon2.transform.position, Quaternion.identity);
        leser2.transform.SetParent(weapon2.transform);
        Bullet_Charge bullet_Charge2 = leser2.GetComponent<Bullet_Charge>();

        //�`���[�W�U���̌p�����ԁA����ї��ߊJ�n������ۂɍU�����肪�o��܂ł̎��Ԃ�n���i�ݒ�̓C���X�y�N�^�Łj
        bullet_Charge2.DestroyTime = chargeAttackTime;
        bullet_Charge2.AttackDelay = chargeAttackDelay;
        bullet_Charge2.AirShip = this;


        //���Ԓn�_�𒴂����ꍇ�A�ˏo�ꏊ��������
        if (currentState == State.Middle)
        {
            //�`���[�W�U���I�u�W�F�N�g���A����3���琶��
            GameObject leser3 = Instantiate(bullet_Charge, weapon3.transform.position, Quaternion.identity);
            leser3.transform.SetParent(weapon3.transform);
            Bullet_Charge bullet_Charge3 = leser3.GetComponent<Bullet_Charge>();

            //�`���[�W�U���̌p�����ԁA����ї��ߊJ�n������ۂɍU�����肪�o��܂ł̎��Ԃ�n���i�ݒ�̓C���X�y�N�^�Łj
            bullet_Charge3.DestroyTime = chargeAttackTime;
            bullet_Charge3.AttackDelay = chargeAttackDelay;
            bullet_Charge3.AirShip = this;

            //�`���[�W�U���I�u�W�F�N�g���A����3���琶��
            GameObject leser4 = Instantiate(bullet_Charge, weapon4.transform.position, Quaternion.identity);
            leser4.transform.SetParent(weapon4.transform);
            Bullet_Charge bullet_Charge4 = leser4.GetComponent<Bullet_Charge>();

            //�`���[�W�U���̌p�����ԁA����ї��ߊJ�n������ۂɍU�����肪�o��܂ł̎��Ԃ�n���i�ݒ�̓C���X�y�N�^�Łj
            bullet_Charge4.DestroyTime = chargeAttackTime;
            bullet_Charge4.AttackDelay = chargeAttackDelay;
            bullet_Charge4.AirShip = this;
        }

        Invoke("PlayLeserSE", chargeAttackDelay);
    }

    //���[�USE
    private void PlayLeserSE()
    {
        SEManager.I.PlaySE(leserSEVol, leserSE);
    }

    //�{���U��
    private void BombAttack()
    {
        SEManager.I.PlaySE(bombInstalledSEVol, bombInstalledSE);

        //�{���𐶐��A�t���X�r�[�̈ʒu�Ǝc�����Ԃ�n��
        GameObject bomb = Instantiate(limitBomb, bombWeapon.transform.position, Quaternion.identity);
        Bullet_Mine bullet_mine1 = bomb.GetComponent<Bullet_Mine>();
        bullet_mine1.Frisbee = frisbee;
        bullet_mine1.DestroyTime = bombDestoryTime;
        bullet_mine1.AirShip = this;
    }


    //�A�j���[�V�����ݒ�
    private void SetAnim()
    {
        //�F���D�̃A�j���[�V����
        anim.SetBool("isStart", currentState == State.Start);
    }

    //�Ə��̃A�j���[�V����
    private void SetUIAnim()
    {
        //���ˎ��Ԃ�����قǃA�j���[�V��������������
        UIanimator.SetFloat("Speed", 1 + rockOnTimer);
    }

    //�g���K�[
    private void OnTriggerEnter(Collider other)
    {
        //�I������A�ȍ~�U�����s��Ȃ�
        if (other.CompareTag("End"))
        {
            currentState = State.End;
            smoke.Play();
        }

        //���Ԓn�_�A�U�����x���㏸����
        if (other.CompareTag("Middle"))
        {
            currentState = State.Middle;
        }
    }

    //�X�e�[�g��Ԃ�
    public State GetState()
    {
        return currentState;
    }
}
