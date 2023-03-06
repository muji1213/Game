using UnityEngine;
using UnityEngine.UI;

//このスクリプトはカウントダウンの数字の表示およびアニメーションの制御です
public class CountDown : MonoBehaviour
{
    //各種スプライト
    [SerializeField] Sprite one;
    [SerializeField] Sprite two;
    [SerializeField] Sprite three;
    [SerializeField] Sprite go;
    [SerializeField] Image numberImage;

    [Header("321の音")] [SerializeField] AudioClip countdownSE;
    [Header("Goの音")] [SerializeField] AudioClip goSE;
    [Header("SEの音量")] [SerializeField] [Range(0, 1)] float SEVol = 1;
    [Header("バックグラウンド")] [SerializeField] GameObject bg;

    //カウントダウン時間（3,2,1,Goなので+1秒する）
    private float countdownTimer = 4.0f;

    //アニメーター
    private Animator anim;

    //現在の数字
    private Sprite currentSprite = null;

    //前の数字
    private Sprite preSprite = null;

    //カウントダウンが終わったかどうか
    [HideInInspector] public bool isCountdownEnd = false;

    //オーディオソース
    private AudioSource audioSource;

    void Start()
    {
        bg.SetActive(false);
        //アニメーターを取得
        anim = GetComponent<Animator>();

        //オーディオソース
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// カウントダウンアニメーションを再生
    /// </summary>
    private void PlayStartAnimation()
    {
        anim.Play("Countdown_start", 0, 0);
    }

    public void StartCountDown()
    {
        if (!bg.activeSelf)
        {
            bg.SetActive(true); ;
        }

        //カウントダウンが0以下なら
        if (countdownTimer < 0)
        {
            //フラグを立てる
            isCountdownEnd = true;

            //このゲームオブジェクトは無効に
            this.gameObject.SetActive(false);
        }
        //カウントダウンが1以下なら
        else if (countdownTimer < 1)
        {
            //スプライトを変更
            numberImage.sprite = go;

            //現在のスプライトを更新
            currentSprite = numberImage.sprite;
        }
        //カウントダウンが2以下なら
        else if (countdownTimer < 2)
        {
            //スプライトを更新
            numberImage.sprite = one;

            //現在のスプライトを更新
            currentSprite = numberImage.sprite;
        }
        //カウントダウンが3以下なら
        else if (countdownTimer < 3)
        {
            //スプライトを更新
            numberImage.sprite = two;

            //現在のスプライトを更新
            currentSprite = numberImage.sprite;
        }
        //カウントダウンが4以下なら
        else
        {
            //スプライトを更新
            numberImage.sprite = three;

            //現在のスプライトを更新
            currentSprite = numberImage.sprite;
        }

        //スプライトの更新があった場合、アニメーション再生
        if (preSprite != currentSprite)
        {
            if (currentSprite != go)
            {
                //GoならGo用のSE
                SEManager.seManager.PlaySE(SEVol, countdownSE);
            }
            else
            {
                //それ以外なら、１２３用のSE
                SEManager.seManager.PlaySE(SEVol, goSE);
            }
            //アニメーションを再生する
            PlayStartAnimation();
        }


        //カウントダウンする
        countdownTimer -= Time.deltaTime;

        //現在のスプライトを記憶させる
        preSprite = currentSprite;
    }
}
