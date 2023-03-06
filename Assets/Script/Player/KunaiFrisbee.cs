using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunaiFrisbee : Frisbee
{
    [SerializeField] [Header("�������")] private int dodgeTime;
    [SerializeField] [Header("����N�[���^�C��")] private int dodgeCoolTime;
    [SerializeField] [Header("���G�t�F�N�g")] private ParticleSystem smoke;

    private Color color;

    //������Ă��邩
    private bool isDodge = false;

    //����\��Ԃ�
    private bool canDodge = true;

    //����N�[���^�C�}�[
    private float dodgeCoolTimer = 0.0f;

    //��������
    private void OnEnable()
    {
        transform.Rotate(0, 180, 0);
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();

        if (!isDodge)
        {
            //�N�[���^�C�����Ȃ�
            if (dodgeCoolTimer < dodgeCoolTime)
            {
                //�^�C�����v��
                dodgeCoolTimer += Time.deltaTime;

                //����͂ł��Ȃ�
                canDodge = false;
            }
            else
            {
                //
                canDodge = true;
            }
        }    
    }

    protected override void Dodge()
    {
        //��𒆂łȂ����A��e���G���Ԓ��łȂ�
        if (!isDodge && !isInvincible && canDodge)
        {
            StartCoroutine(TheDodge(dodgeTime));

            //�^�C�}�[���Z�b�g
            dodgeCoolTimer = 0.0f;

            //��𒆃t���O
            isDodge = true;
        }
    }

    private IEnumerator TheDodge(int time)
    {
        //�G�t�F�N�g
        smoke.Play();

        //���C���[���ق��̃��C���[�ɂ��邱�ƂŁA��Q���Ƃ̏Փ˂����������
        this.gameObject.layer = 10;

        //�������ɂ���
        this.gameObject.GetComponent<MeshRenderer>().material.color = new Color(color.a, color.g, color.b, 0.3f);

        yield return new WaitForSeconds(time);

        //��莞�ԓ����x����߂�
        this.gameObject.GetComponent<MeshRenderer>().material.color = new Color(color.a, color.g, color.b, 1.0f);

        //���C���[���ق��̃��C���[�ɂ��邱�ƂŁA��Q���Ƃ̏Փ˂����������
        this.gameObject.layer = 9;

        //��𒆂łȂ�
        isDodge = false;

        //�G�t�F�N�g
        smoke.Play();
    }
}
