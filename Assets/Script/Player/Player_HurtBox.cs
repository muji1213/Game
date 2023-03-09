using UnityEngine;


//このスクリプトはプレイヤーおよびフリスビーの衝突判定を取得するスクリプト
public class Player_HurtBox : MonoBehaviour
{
    public enum DeadType
    {
        Fall = 0,
        Collide = 1
    }

    //各種タグ

    //プレイヤータグ
    private string playerTag = "Player";

    //障害物タグ
    private string obstacleTag = "Obstacle";

    //コインタグ
    private string coinTag = "Coin";

    //死亡判定（穴、もしくはゴールが外れた場合）
    private string deadColTag = "DeadCol";

    //死亡種類
    private DeadType type;

    private Player_Controller player; //プレイヤーコントローラー（走る方）
   
    private void Start()
    {
        //各種コンポーネント取得

        //プレイヤー（走る方）
        player = GetComponent<Player_Controller>();
    }

    //衝突判定

    //フリスビーは障害物との衝突判定、およびゴールをすり抜けてしまった際の判定を取る
    //プレイヤーは障害物との衝突判定、および穴に落ちた際の判定を取る
    private void OnCollisionEnter(Collision collision)
    {
        //障害物との衝突判定
        if (collision.gameObject.tag == obstacleTag)
        {

            //障害物との衝突がプレイヤー（走る方）だった場合、死亡判定をTrueにし、死亡アニメーションを再生
            if (this.gameObject.tag == playerTag)
            {
                Vector3 direction = (this.gameObject.transform.position - collision.gameObject.transform.position).normalized;

                type = DeadType.Collide;

                player.TheDie(type, direction);
            }
        }
        else
        {
            
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
                type = DeadType.Fall;

                //死亡メソッド呼び出し
                player.TheDie(type, new Vector3(0, 0, 0));
            }
        }

        //コインと衝突した場合
        if (other.CompareTag(coinTag))
        {
            int coinScore = other.GetComponent<Coin>().Score;

            Debug.Log("コイン取得");

            //コイン取得時のエフェクトを出す
            player.GetCoin(coinScore);
        }

        //ライフアップを取ったらHPを一つ追加する
        if (other.CompareTag("Heart"))
        {
            player.LifeUP();
        }
    }
}
