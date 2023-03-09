using UnityEngine;

//フェードイン
public class FadeUI : MonoBehaviour
{
    //アニメーター
    private Animator anim;

    //フェードが終わっているか
    private bool isAnimEnd = false;
    
    void Start()
    {
        anim = GetComponent<Animator>();
       /* StartAnim();*/
    }

    // Update is called once per frame
    void Update()
    {
        //現在再生中のアニメーションを取得
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0); // layerNo:Base Layer == 0

        //終わったら
        if (stateInfo.normalizedTime >= 1.0f)
        {
            //フラグをおろす
            isAnimEnd = true;

            //フェードを消す
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

    //各種２つアニメーションを用意しておく
   /* //ランダムで乱数を生成し、アニメーターのパラメータ(type)で決定する
    private void StartAnim()
    {
        int random = Random.Range(0,2);
        anim.SetInteger("type", random);
    }*/
}
