using UnityEngine;

public class Heart : MonoBehaviour
{
    private enum State
    {
        Idle,
        Obtained
    }

    //�X�e�[�g
    private State state;

    //�A�j���[�^
    private Animator anim;

    private void Start()
    {
        state = State.Idle;
        anim = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        //�擾�A�j���[�V�������I�����������
        if (state == State.Obtained)
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
            state = State.Obtained;

            //�擾�A�j���[�V�����Đ�
            anim.SetTrigger("End");
        }
    }
}
