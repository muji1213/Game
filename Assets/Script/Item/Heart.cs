using UnityEngine;

public class Heart : MonoBehaviour
{
    //アニメータ
    private Animator anim;

    //取得されたか
    private bool isGot = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        //取得アニメーションが終了したら消す
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

            //取得アニメーション再生
            anim.SetTrigger("End");
        }
    }
}
