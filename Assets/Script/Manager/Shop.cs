using UnityEngine;
using UnityEngine.UI;

//このスクリプトはショップの購入に関するスクリプトです
public class Shop : MonoBehaviour
{
    [Header("ポイントを表示するテキスト")] [SerializeField] Text pointText;
    [Header("グリッドパネル")] [SerializeField] GameObject itemPanel;
    [Header("生成する子要素")] [SerializeField] GameObject itemPrefab;

    [Header("BGM")] [SerializeField] AudioClip bgm;
    [Header("戻るボタンのSE")] [SerializeField] AudioClip backSE;
    [Header("戻るSEの音量")] [SerializeField] [Range(0, 1)] float backSEVol = 1;
    [Header("購入時の音")] [SerializeField] AudioClip buySE;
    [Header("購入時の音の音量")] [SerializeField] [Range(0, 1)] float buySEVol = 1;
    [Header("ポイントが足りないときの音")] [SerializeField] AudioClip blockSE;
    [Header("ポイントが足りないときの音の音量")] [SerializeField] [Range(0, 1)] float blockSEVol = 1;

    //前のポイント
    private int prePoint;

    private void Awake()
    {
        BGMManager.I.PlayBgm(bgm);

        //0番はもとから所持しているので入れない
        for (int i = 1; i < ItemManager.items.Count; i++)
        {
            //パネルを生成
            CreateItemPanel(i);
        }
    }

    private void CreateItemPanel(int num)
    {
        //パネルのひな形を生成
        GameObject item = Instantiate(itemPrefab);

        //子オブジェクトのボタンを取得
        Button buyButton = item.transform.Find("BuyButton").transform.GetChild(0).GetComponent<Button>();

        //子オブジェクトのボタンのテキストを取得
        Text buyButtonText = buyButton.transform.GetChild(1).GetComponent<Text>();

        //子オブジェクトの売り切れ表示を取得
        GameObject soldOut = item.transform.Find("SoldOut").gameObject;

        //パネルを親に（グリッドレイアウトがついている）
        item.transform.SetParent(itemPanel.transform);

        //位置や大きさを調整
        item.transform.localScale = new Vector3(1, 1, 1);
        item.transform.localPosition = new Vector3(0, 0, 0);

        //画像、名前、フレーバーテキスト、値段を設定
        item.transform.Find("Image/FrisbeeImage").GetComponent<Image>().sprite =
                                           ItemManager.I.GetItem(num).Image;
        item.transform.Find("Text/FrisName").GetComponent<Text>().text =
                                            ItemManager.I.GetItem(num).Name;
        item.transform.Find("Text/FrisInfo").GetComponent<Text>().text =
                                           ItemManager.I.GetItem(num).Info;
        item.transform.Find("Text/Price").GetComponent<Text>().text =
                                           "ネダン：" + ItemManager.I.GetItem(num).Price.ToString() + "ポイント";


        //ボタンに処理を追加
        buyButton.onClick.AddListener(() => BuyItem(num, soldOut));

        //ボタンのテキストを設定
        buyButtonText.text = "買う";

        //生成時すでに所持しているなら売り切れ表示を
        if (ItemManager.I.GetItemFlag(num))
        {
            soldOut.SetActive(true);
        }
        //そうでないなら非表示に
        else
        {
            soldOut.SetActive(false);
        }
    }

    //アイテムマネージャーに登録
    //ItemManagerはDictionaryでアイテムを管理
    //購入すると、アイテムマネージャーの所持フラグがONになる
    public void BuyItem(int num, GameObject soldOut)
    {
        //ボタンの親のオブジェクトを取得（アイテムのスクリプトがついている）

        //連打対策
        if (ItemManager.I.GetItemFlag(num))
        {
            Debug.Log("すでに所持しています");
        }
        else if (GameManager.I.Point < ItemManager.I.GetItem(num).Price)
        {
            SEManager.I.PlaySE(blockSEVol, blockSE);
            Debug.Log("所持ポイントが足りません");
        }
        else
        {
            //SE
            SEManager.I.PlaySE(buySEVol, buySE);

            //売り切れ表示をする
            soldOut.SetActive(true);

            //所持ポイントを引く
            GameManager.I.Point -= ItemManager.I.GetItem(num).Price;

            //持ってないなら、フラグをONに
            ItemManager.I.ActiveItemFlag(num);
        }
    }

    void Update()
    {
        //前回とポイントが変わった時だけテキストを変更する
        if (prePoint != GameManager.I.Point)
        {
            pointText.text = GameManager.I.Point.ToString();
            prePoint = GameManager.I.Point;
        }
    }

    //ステージセレクトへ
    public void ToStageSelect()
    {
        SEManager.I.PlaySE(backSEVol, backSE);
        SceneChanger.I.ToStageSelectScene();
    }
}

