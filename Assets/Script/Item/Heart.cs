using UnityEngine;

public class Heart : MonoBehaviour
{
    private enum State
    {
        Idle,
        Obtained
    }

    //ステート
    private State state;

    //アニメータ
    private Animator anim;

    private void Start()
    {
        state = State.Idle;
        anim = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        //取得アニメーションが終了したら消す
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

            //取得アニメーション再生
            anim.SetTrigger("End");
        }
    }
}
