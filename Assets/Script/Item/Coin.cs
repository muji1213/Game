using UnityEngine;


//このスクリプトはコインを取得した際のスクリプトで
public class Coin : MonoBehaviour
{
    private int score = 200;//取得ポイント

    //フリスビーのタグ
    private string frisbeeTag = "Frisbee";

    //プレイヤータグ
    private string playerTag = "Player";

    //プレイヤーもしくはフリスビーが触れた場合
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(frisbeeTag) || other.CompareTag(playerTag))
        {
            //消滅させる
            Destroy(this.gameObject);
        }
    }

    //コイン取得時のスコアを返す
    public int Score
    {
        get
        {
            return score;
        }
    }
}
