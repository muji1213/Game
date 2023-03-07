using UnityEngine;

public class Frisbee_HurtBox : MonoBehaviour
{
    //各種タグ

    //フリスビータグ
    private string frisbeeTag = "Frisbee";

    //障害物タグ
    private string obstacleTag = "Obstacle";

    //ターゲットタグ（ゴール）
    private string targetTag = "Target";

    //コインタグ
    private string coinTag = "Coin";

    //死亡判定（穴、もしくはゴールが外れた場合）
    private string deadColTag = "DeadCol";

    //死亡判定
    private bool isDead = false;

    private Frisbee frisbee; //フリスビーのスクリプト
    private CameraFollower cameraFollower; //カメラ追従用のスクリプト
    private MeshCollider meshCollider;

    private void Start()
    {
        //各種コンポーネント取得

        //フリスビー
        frisbee = GetComponent<Frisbee>();

        //カメラ追従用のスクリプト
        cameraFollower = GameObject.Find("Main Camera").GetComponent<CameraFollower>();

        //メッシュコライダー
        meshCollider = GetComponent<MeshCollider>();
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
            meshCollider.enabled = false;
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


            //障害物との衝突がフリスビーだった場合
            //
            //分岐
            //障害物のレベルがフリスビーより低い場合何もしない
            //
            //障害物のレベルがフリスビーより高かった場合
            //　ライフが0より大きければ、無敵時間にし、フリスビーのライフを減らす
            //　ライフが0以下ならば、死亡判定をTrueに
            if (this.gameObject.tag == frisbeeTag)
            {
                //障害物のスクリプトを取得
                Collisionable_Obstacle obstacle = collision.gameObject.GetComponent<Collisionable_Obstacle>();

                //フリスビーと障害物のレベルを比較
                //フリスビーの方が低ければ
                if (obstacle.level >= StageSelectManager.selectedFrisbee.FrisbeeLevel)
                {
                    //ライフを減らす
                    frisbee.ReduceLife();
                    frisbee.PlayDamageEffect(collision.contacts[0].point);

                    //ライフが0以下になったら
                    if (frisbee.GetHP <= 0)
                    {

                        //死亡判定をTrueに
                        frisbee.TheDie(0);
                        isDead = true;

                    }
                    //0より大きければ
                    else
                    {
                        //無敵時間に
                        frisbee.Invincible(3);
                    }
                }
                //フリスビーのレベルのほうが高かった場合
                else
                {
                    //何もしない
                    return;
                }
            }
        }
        else
        {
            isDead = false;
        }

        //フリスビーがターゲット（ゴール）と衝突した場合
        if (collision.gameObject.tag == targetTag)
        {
            //フリスビーの操作を無効にする
            frisbee.enabled = false;

            meshCollider.enabled = false;

            //SEを止める
            this.GetComponent<AudioSource>().enabled = false;
        }
    }

    //死亡判定に衝突した場合（即死）
    private void OnTriggerEnter(Collider other)
    {
        //死亡判定と衝突した場合
        if (other.CompareTag(deadColTag))
        {
            //フリスビーの場合
            if (this.gameObject.tag == frisbeeTag)
            {
                //SEを止める
                this.GetComponent<AudioSource>().enabled = false;

                //死亡判定をTrueに
                frisbee.TheDie(1);
                isDead = true;
            }
        }


        //ボスの攻撃にあたった場合
        if (other.CompareTag("Attack"))
        {
            frisbee.ReduceLife();
            frisbee.PlayDamageEffect(other.ClosestPointOnBounds(this.gameObject.transform.position));

            if (frisbee.GetHP <= 0)
            {
                isDead = true;
                
                frisbee.TheDie(0);
            }
            else
            {
                //無敵時間に
                frisbee.Invincible(3);
            }
        }

        //コインと衝突した場合
        if (other.CompareTag(coinTag))
        {
            if (this.gameObject.tag == frisbeeTag)
            {
                frisbee.PlayCoinEffect();
            }

        }
    }
}
