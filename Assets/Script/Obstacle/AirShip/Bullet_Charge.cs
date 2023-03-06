using UnityEngine;

public class Bullet_Charge : Bullet
{
    //���ߊJ�n����U�����肪�o��܂ł̎���
    private float attackDelay;
    [SerializeField] [Header("�U������̃I�u�W�F�N�g")] private BoxCollider hitBox;
    [SerializeField] [Header("�U���G�t�F�N�g")] private ParticleSystem leser;

    private float timer = 0.0f;

    new void Start()
    {
        base.Start();
    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();
        //�^�C�}�[�����Ŏ��Ԃ𒴂��������
        if (timer >= destroyTime)
        {
            timer = 0.0f;

            //�����蔻��𖳌���
            hitBox.enabled = false;

            //�G�t�F�N�g�𖳌���
            leser.Stop();

            return;
        }

        //�^�C�}�[���U�����肪�o��܂ł̎��Ԃ𒴂����ꍇ
        if (timer >= attackDelay)
        {
            //�����蔻����o��������
            hitBox.enabled = true;

            //�G�t�F�N�g���o��
            leser.Play();
        }
        else
        {
            //���ߒ��͓����蔻��͖���
            hitBox.enabled = false;
            leser.Stop();
        }

        timer += Time.deltaTime;
    }

    //�\�����̕\������U�����肪�o��܂ł̎���
    public float AttackDelay
    {
        set
        {
            attackDelay = value;
        }
    }
}
