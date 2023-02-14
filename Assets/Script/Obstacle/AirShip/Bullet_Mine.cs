using UnityEngine;

public class Bullet_Mine : Bullet
{
    //�X�t�B�A�R���C�_�[(HITBOX)
    private SphereCollider hitBox;

    //���e�̃��f��
    [SerializeField][Header("���e��3D���f��")] GameObject bomb;

    //�����̃G�t�F�N�g
    [SerializeField][Header("�����̃G�t�F�N�g")] ParticleSystem bombEffect;

    //����SE
    [SerializeField][Header("���������ۂ̃G�t�F�N�g")] AudioClip explosionSE;

    //audioSource
    private AudioSource audioSource;

    //����������
    private bool isExposion = false;

    GameObject frisbee;

    new void Start()
    {
        base.Start();

        audioSource = GetComponent<AudioSource>();
        audioSource.volume *= SEManager.seManager.SeVolume / 1;

        hitBox = GetComponent<SphereCollider>();
        hitBox.enabled = false;
    }

    private void FixedUpdate()
    {
        //�t���X�r�[����苗���܂ŋ߂Â����甚������
        if (Vector3.Distance(frisbee.transform.position, this.transform.position) < 5.0f)
        {
            //�����̃G�t�F�N�g��������܂ŉ��x���Ăяo����Ă��܂����߁A�t���O���~��ĂȂ��Ƃ��������s����悤�ɂ���
            if (!isExposion)
            {
                Explosion();
            }
           
        }
        else
        {
            return;
        }
    }


    private void Explosion()
    {
        //����SE
        audioSource.PlayOneShot(explosionSE);

        //�����蔻���L����
        hitBox.enabled = true;

        //���e��3D���f���͏���
        bomb.SetActive(false);

        //�����̃G�t�F�N�g
        bombEffect.Play();

        //�����t���O�����낷
        isExposion = true;

        //�����̃G�t�F�N�g���I�����������
        Destroy(this.gameObject, bombEffect.main.duration);
    }

    //���m�Ώ�
    public GameObject Frisbee
    {
        set
        {
            frisbee = value;
        }
    }
}
