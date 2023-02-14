using UnityEngine;

//このスクリプトはフリスビーのステータスを設定します
public class FrisbeeItem
{
    private int level; //フリスビーのレベル
    private int num; //管理番号
    private string frisname; //フリスビーの名前
    private int price; //値段
    private Sprite frisSprite; //フリスビーの画像をセット
    private GameObject Frisbee; //これに対応するフリスビーのプレハブ
    private bool isObtained;

    [TextArea] [SerializeField] private string frisInfo; //フリスビーの説明文

    public FrisbeeItem(int level, int num, string frisname, int price, Sprite frisSprite, string frisInfo, GameObject Frisbee, bool isObtained)
    {
        this.level = level;
        this.num = num;
        this.frisname = frisname;
        this.price = price;
        this.frisSprite = frisSprite;
        this.frisInfo = frisInfo;
        this.Frisbee = Frisbee;
        this.isObtained = isObtained;
    }

    //名前取得
    public string Name
    {
        get { return frisname; }
    }

    //値段を取得
    public int Price
    {
        get { return price; }
    }

    //フリスビーのフレーバーテキスト
    public string Info
    {
        get { return frisInfo; }
    }

    //フリスビーの画像
    public Sprite Image
    {
        get { return frisSprite; }
    }

    //フリスビーの番号
    public int Num
    {
        get { return num; }
    }

    //対応するフリスビーの取得
    public GameObject SelectedFrisbee
    {
        get { return Frisbee; }
    }

    //レベルを取得
    public int FrisbeeLevel
    {
        get { return level; }
    }

    //所持しているかどうか
    public bool Obtain
    {
        get
        {
            return isObtained;
        }
        set
        {
            isObtained = value;
        }
    }
}
