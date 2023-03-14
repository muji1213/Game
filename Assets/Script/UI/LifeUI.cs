using UnityEngine;


//ライフのUIのスクリプトです
public class LifeUI : MonoBehaviour
{
    [Header("ライフのプレハブ")] [SerializeField] GameObject lifePrefab;
    [Header("パネル")] [SerializeField] GameObject lifePanel;

    //現在ライフ
    private int currentLife = 0;

    private void Awake()
    {
        this.gameObject.SetActive(false);
    }

    //最初HP設定
    public void SetInitialLife(int HP)
    {
        //初期HPを取得
        currentLife = HP;

        //HP分だけ、ハートのUIを生成する
        for (int i = 0; i < currentLife; i++)
        {
            Debug.Log("Life:" + HP);
            //プレハブをインスタンシエイト
            GameObject life = Instantiate(lifePrefab);
            life.transform.SetParent(lifePanel.transform);

            //位置や大きさを設定
            life.transform.localPosition = new Vector3(0, 0, 0);
            life.transform.localRotation = new Quaternion(0, 0, 0, 0);
            life.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    //ライフを消す
    public void DestroyLife()
    {
        currentLife -= 1;

        //0未満にならない
        if (currentLife < 0)
        {
            return;
        }
        //0以上なら、ハートを一つ消す
        else
        {
            Destroy(lifePanel.transform.GetChild(currentLife).gameObject);
        }

    }
}
