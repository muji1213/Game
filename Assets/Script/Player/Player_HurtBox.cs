using UnityEngine;


//このスクリプトはプレイヤーおよびフリスビーの衝突判定を取得するスクリプト
public class Player_HurtBox : MonoBehaviour
{
    //各種タグ

    //プレイヤータグ
    private string playerTag = "Player";

    //障害物タグ
    private string obstacleTag = "Obstacle";

    //コインタグ
    private string coinTag = "Coin";

    //死亡判定（穴、もしくはゴールが外れた場合）
    private string deadColTag = "DeadCol";

    //死亡判定
    private bool isDead = false;

    private Player_Controller player; //プレイヤーコントローラー（走る方）
    private CameraFollower cameraFollower; //カメラ追従用のスクリプト

    private void Start()
    {
        //各種コンポーネント取得

        //プレイヤー（走る方）
        player = GetComponent<Player_Controller>();

        //カメラ追従用のスクリプト
        cameraFollower = GameObject.Find("Main Camera").GetComponent<CameraFollower>();
    }

    /// <summary>
    /// 死亡判定取得メソッド
    /// プレイヤーもしくはフリスビーが死亡した際にTrueを返す
    /// </summary>
    /// <returns></returns>
    public bool isDeadCheck()
    {
        if (isDead)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    //衝突判定

    //フリスビーは障害物との衝突判定、およびゴールをすり抜けてしまった際の判定を取る
    //プレイヤーは障害物との衝突判定、および穴に落ちた際の判定を取る
    private void OnCollisionEnter(Collision collision)
    {
        //障害物との衝突判定
        if (collision.gameObject.tag == obstacleTag)
        {
            //衝突時、カメラを振動させる
            cameraFollower.Shake(0.3f, 1f);

            //障害物との衝突がプレイヤー（走る方）だった場合、死亡判定をTrueにし、死亡アニメーションを再生
            if (this.gameObject.tag == playerTag)
            {
                Vector3 direction = (this.gameObject.transform.position - collision.gameObject.transform.position).normalized;

                player.TheDie(1, direction);

                isDead = true;
            }
        }
        else
        {
            isDead = false;
        }
    }

    //死亡判定に衝突した場合（即死）
    private void OnTriggerEnter(Collider other)
    {
        //死亡判定と衝突した場合
        if (other.CompareTag(deadColTag))
        {
            //プレイヤーの場合死亡判定は穴になる
            if (this.gameObject.tag == playerTag)
            {
                //カメラを真上から下方向に投射し、穴に落ちているところが見えるようにする
                cameraFollower.ZoomToTarget(-5, 0, new Vector3(0, -10, 0));

                //死亡メソッド呼び出し
                player.TheDie(0, new Vector3(0, 0, 0));

                //死亡判定をTrueに
                isDead = true;
            }
        }

        //コインと衝突した場合
        if (other.CompareTag(coinTag))
        {
            //コイン取得時のエフェクトを出す
            if (this.gameObject.tag == playerTag)
            {
                player.PlayCoinEffect();
            }
        }

        if (other.CompareTag("Heart"))
        {
            player.LifeUP();
            Debug.Log("lifeup");
        }
    }
}
