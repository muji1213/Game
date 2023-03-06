using System.Collections.Generic;
using UnityEngine;

//購入したフリスビーを管理するスクリプト
public class ItemManager : MonoBehaviour
{
    public static ItemManager itemManager = null;

    //アイテムのディクショナリー
    public static Dictionary<int, FrisbeeItem> items;

    //フリスビーのプレハブ（個別にステータスがふってある）
    [SerializeField] private GameObject[] Frisbees = new GameObject[3];

    //初期化したかどうか
    static private bool isInitialized = false;

    private void Awake()
    {
        //初期化
        if (!isInitialized)
        {
            itemManager = this;

            items = new Dictionary<int, FrisbeeItem>();

            items.Add(0, new FrisbeeItem(0, 0, "NormalFrisbee", 0, Resources.Load<Sprite>("NormalFrisbeeSprite"),
                "ビギナーむけのフリスビー、かるくてつかいやすい", Frisbees[0], true));

            items.Add(1, new FrisbeeItem(1, 1, "MetalFrisbee", 2100, Resources.Load<Sprite>("MetalFrisbeeSprite"),
              "とてもオモいフリスビー、カゼにふきとばされにくい", Frisbees[1], false));

            items.Add(2, new FrisbeeItem(2, 2, "クナイフリスビー", 4100, Resources.Load<Sprite>("KunaiSprite"), "ニンジャのフリスビー。Aキーでスガタをケせる", Frisbees[2], false));

            isInitialized = true;
        }
    }

    //アイテムを所持しているかどうか
    //所持しているならtrue,そうでないならfalse
    //numは登録時のアイテム番号を対応する
    //0ならnormalFrisbee
    public bool GetItemFlag(int num)
    {
        return GetItem(num).Obtain;
    }

    //アイテムをゲットしたときに所持フラグをおろす
    //numは登録時のアイテム番号を対応する
    //0ならnormalFrisbee
    public void ActiveItemFlag(int num)
    {
        GetItem(num).Obtain = true;
    }

    //アイテムがディクショナリに存在するかどうか
    public bool ExistItem(int key)
    {
        //引数と同じキーが存在するか
        if (items.ContainsKey(key))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //指定された番号のアイテムを返す
    public FrisbeeItem GetItem(int key)
    {
        //存在するか調べる
        if (ExistItem(key))
        {
            return items[key];
        }
        else
        {
            Debug.Log("存在しません");
            return null;
        }
    }
}
