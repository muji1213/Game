using UnityEngine;

//�t�F�[�h�C��
public class Fade : MonoBehaviour
{
    //�A�j���[�^�[
    private Animator anim;

    //�t�F�[�h���I��������ǂ���
    [HideInInspector] public bool isAnimEnd = false;
    
    void Start()
    {
        anim = GetComponent<Animator>();
       /* StartAnim();*/
    }

    // Update is called once per frame
    void Update()
    {
        //���ݍĐ����̃A�j���[�V�������擾
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0); // layerNo:Base Layer == 0

        //�I�������
        if (stateInfo.normalizedTime >= 1.0f)
        {
            //�t���O�����낷
            isAnimEnd = true;

            //�t�F�[�h������
            Destroy(this.gameObject);
        }
    }

    //�e��Q�A�j���[�V������p�ӂ��Ă���
   /* //�����_���ŗ����𐶐����A�A�j���[�^�[�̃p�����[�^(type)�Ō��肷��
    private void StartAnim()
    {
        int random = Random.Range(0,2);
        anim.SetInteger("type", random);
    }*/
}
