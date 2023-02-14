using UnityEngine;

public class Heart : MonoBehaviour
{
    //�A�j���[�^
    private Animator anim;

    //�擾���ꂽ��
    private bool isGot = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        //�擾�A�j���[�V�������I�����������
        if (isGot)
        {
            if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isGot = true;

            //�擾�A�j���[�V�����Đ�
            anim.SetTrigger("End");
        }
    }
}
