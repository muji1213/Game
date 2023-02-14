using UnityEngine;
using UnityEngine.UI;

//このスクリプトはショップの購入に関するスクリプトです
public class ShopManager : SceneChanger
{
    [Header("ポイントを表示するテキスト")] [SerializeField] Text pointText;
    [Header("グリッドパネル")] [SerializeField] GameObject itemPanel;
    [Header("生成する子要素")] [SerializeField] GameObject itemPrefab;

    [Header("BGM")] [SerializeField] AudioClip bgm;
    [Header("購入時の音")] [SerializeField] AudioClip buySE;
    [Header("ポイントが足りないときの音")] [SerializeField]AudioClip blockSE;

    //前のポイント
    private int prePoint;

    private void Awake()
    {
        //0番はもとから所持しているので入れない
        for (int i = 1; i < ItemManager.items.Count; i++)
        {
            //パネルを生成
            CreateItemPanel(i);
        }
    }

    private void Start()
    {
        //BGM
        BGMManager.bgmManager.PlayBgm(bgm);
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
                                           ItemManager.itemManager.GetItem(num).Image;
        item.transform.Find("Text/FrisName").GetComponent<Text>().text =
                                            ItemManager.itemManager.GetItem(num).Name;
        item.transform.Find("Text/FrisInfo").GetComponent<Text>().text =
                                           ItemManager.itemManager.GetItem(num).Info;
        item.transform.Find("Text/Price").GetComponent<Text>().text =
                                           "ネダン：" + ItemManager.itemManager.GetItem(num).Price.ToString() + "ポイント";


        //ボタンに処理を追加
        buyButton.onClick.AddListener(() => BuyItem(num, soldOut));

        //ボタンのテキストを設定
        buyButtonText.text = "買う";

        //生成時すでに所持しているなら売り切れ表示を
        if (ItemManager.itemManager.GetItemFlag(num))
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
        if (ItemManager.itemManager.GetItemFlag(num))
        {
            Debug.Log("すでに所持しています");
        }
        else if (GameManager.gameManager.point < ItemManager.itemManager.GetItem(num).Price)
        {
            SEManager.seManager.PlaySe(blockSE);
            Debug.Log("所持ポイントが足りません");
        }
        else
        {
            //SE
            SEManager.seManager.PlaySe(buySE);

            //売り切れ表示をする
            soldOut.SetActive(true);

            //所持ポイントを引く
            GameManager.gameManager.point -= ItemManager.itemManager.GetItem(num).Price;

            //持ってないなら、フラグをONに
            ItemManager.itemManager.ActiveItemFlag(num);
        }
    }

    void Update()
    {
        //前回とポイントが変わった時だけテキストを変更する
        if (prePoint != GameManager.gameManager.point)
        {
            pointText.text = GameManager.gameManager.point.ToString();
            prePoint = GameManager.gameManager.point;
        }
    }
}

