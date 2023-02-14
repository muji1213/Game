using UnityEngine;

public class Bullet_Mine : Bullet
{
    //スフィアコライダー(HITBOX)
    private SphereCollider hitBox;

    //爆弾のモデル
    [SerializeField][Header("爆弾の3Dモデル")] GameObject bomb;

    //爆発のエフェクト
    [SerializeField][Header("爆発のエフェクト")] ParticleSystem bombEffect;

    //爆発SE
    [SerializeField][Header("爆発した際のエフェクト")] AudioClip explosionSE;

    //audioSource
    private AudioSource audioSource;

    //爆発したか
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
        //フリスビーが一定距離まで近づいたら爆発する
        if (Vector3.Distance(frisbee.transform.position, this.transform.position) < 5.0f)
        {
            //爆発のエフェクトが消えるまで何度も呼び出されてしまうため、フラグが降りてないときだけ実行するようにする
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
        //爆発SE
        audioSource.PlayOneShot(explosionSE);

        //当たり判定を有効に
        hitBox.enabled = true;

        //爆弾の3Dモデルは消す
        bomb.SetActive(false);

        //爆発のエフェクト
        bombEffect.Play();

        //爆発フラグをおろす
        isExposion = true;

        //爆発のエフェクトが終了したら消す
        Destroy(this.gameObject, bombEffect.main.duration);
    }

    //感知対象
    public GameObject Frisbee
    {
        set
        {
            frisbee = value;
        }
    }
}
