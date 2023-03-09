using UnityEngine;


//�Փ˂���^�C�v�̏�Q���̃N���X�ł�
public class Collisionable_Obstacle : MonoBehaviour
{
    //���W�b�g�{�f�B
    protected Rigidbody rb;

    //�����_���[
    protected MeshRenderer meshRenderer;

    //�X�e�[�W�}�l�[�W���[
    protected StageManager stageManager;

    //��Q���̃��x��
    //�t���X�r�[�̃��x���������ہA������΂����
    public int level;

    //�ԃe�N�X�`��(�t���X�r�[�̃��x���̂ق����������Ɏg��)
    Texture2D redTex;

    protected void Start()
    {
        //�R���|�[�l���g�擾
        rb = GetComponent<Rigidbody>();
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        meshRenderer = GetComponent<MeshRenderer>();

        //�e�N�X�`��
        redTex = Resources.Load<Texture2D>("RedTex");

        //�Q�[���J�n���A�t���X�r�[�̃��x�����m�F���A���g�̂ق������x�����Ⴂ�Ȃ��
        //���g�̂ق��������Ȃ�ʏ�ʂ�\��
        if (GameManager.I.SelectedFrisbeeInfo.FrisbeeLevel > level)
        {
            meshRenderer.material.SetTexture("_MainTex",redTex);
        }
        else
        {
            return;
        }
    }

    //�t���X�r�[�Ɍ��˂��ꂽ�ۂ̏���
    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Frisbee")
        {
            //�t���X�r�[�̃��x���̂ق����Ⴂ�Ȃ�
            if (GameManager.I.SelectedFrisbeeInfo.FrisbeeLevel <= level)
            {
                //���̂܂܂Ȃɂ����Ȃ�
                rb.constraints = RigidbodyConstraints.FreezeAll;
                return;
            }
            else
            {
                rb.mass = 0.0f;

                rb.constraints = RigidbodyConstraints.None;

                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z) * 10;

                //������΂��ꂽ2�b����ł�����
                Destroy(this.gameObject, 2.0f);

                this.gameObject.layer = 15;
            }
        }
    }
}
