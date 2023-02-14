using UnityEngine;


//このスクリプトはコインを取得した際のスクリプトで
public class Coin : MonoBehaviour
{
    private int point = 200;//取得ポイント

    // ステージマネージャー
    private StageManager stageManager;

    //フリスビーのタグ
    private string frisbeeTag = "Frisbee";

    //プレイヤータグ
    private string playerTag = "Player";

    void Start()
    {
        //ステージマネージャー
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
    }

    //プレイヤーもしくはフリスビーが触れた場合
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(frisbeeTag) || other.CompareTag(playerTag))
        {
            //ステージマネージャーのポイントを増やす
            stageManager.StorePoint(point);

            //その後消滅させる
            Destroy(this.gameObject);
        }
    }
}
