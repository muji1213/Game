using UnityEngine;

//�t�F�[�h�C��
public class FadeUI : MonoBehaviour
{
    //�A�j���[�^�[
    private Animator anim;

    //�t�F�[�h���I����Ă��邩
    private bool isAnimEnd = false;
    
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
            this.gameObject.SetActive(false);
        }
    }

    public bool IsFadeEnd
    {
        private set
        {
            isAnimEnd = value;
        }
        get
        {
            return isAnimEnd;
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
