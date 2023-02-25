using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

//ステージ選択画面に関するスクリプトです
public class StageSelectManager : SceneChanger
{
    //スタティックはシーン間でも情報が失われない
    public static StageSelectManager stageSelectManager = null;

    // eventSystemを取得するための変数宣言
    [Header("イベントシステム")] [SerializeField] EventSystem eventSystem;

    [Header("ステージへ行く際のダイアログ")] [SerializeField] GameObject startSelectUI;
    [Header("スタートボタン")] [SerializeField] private Button startButton;
    [Header("フリスビー選択のボタンプレハブ")] [SerializeField] GameObject selectButtonPrefab;
    [Header("アイテムパネル")] [SerializeField] GameObject itemPanel;
    [Header("フリスビーのフレーバーテキスト表示用テキスト")] [SerializeField] Text frisInfoText;
    [Header("ステージ名テキスト")] [SerializeField] Text stageNameText;
    [Header("ステージ説明テキスト")] [SerializeField] Text stageInfoText;
    [Header("選択中のステージ画像表示")] [SerializeField] Image stageSprite;
    [Header("ステージ選択ボタンのひな形")] [SerializeField] GameObject stageSelectButtonPrefab;
    [Header("パネル")] [SerializeField] GameObject stagePanel;
    [Header("全てのステージをクリアした際に表示するUI")] [SerializeField] GameObject allClearUI;
    [Header("選択中のフリスビーの上に表示する矢印")] [SerializeField] GameObject arrow;
    [Header("ステージシーンへいくためのボタン")] [SerializeField] Button goStageButton;

    [Header("BGM")] [SerializeField] AudioClip bgm;
    [Header("ボタン押下時のSE")] [SerializeField] AudioClip inputSE;
    [Header("ボタンSEの音量")] [SerializeField] [Range(0, 1)] float inputSEVol = 1;
    [Header("キャンセル時のSE")] [SerializeField] AudioClip cancelSE;
    [Header("キャンセルSEの音量")] [SerializeField] [Range(0, 1)] float canselSEVol = 1;

    //初期化したかどうか、シーン間で保持される
    private static bool isInicialized = false;

    //ステージのディクショナリ
    public static Dictionary<int, Stage> stages;

    //選択されたステージ番号を格納する
    public static int selectedStageNum;

    //ステージセレクトで選ばれたフリスビー
    public static FrisbeeItem selectedFrisbee;

    //全てのステージをクリアしたフラグ
    private static bool isAllStageCleared = false;

    private void Awake()
    {
        //初期化されているかどうか
        if (!isInicialized)
        {
            stageSelectManager = this;

            stages = new Dictionary<int, Stage>();

            //ステージ0
            string stage0 = "山にカコまれているクンレンジョウ。カケダシのフリスビストはココでレンシュウだ！";
            stages.Add(0, new Stage(0, "クンレンジョウ", stage0, Resources.Load<Sprite>("Stage0_Image"), false, true));

            //ステージ1
            string stage1 = "カゼがあれくるうキョウコク。イワがむきだしで、アルくだけでもせいいっぱい";
            stages.Add(1, new Stage(1, "ボウフウのキョウコク", stage1, Resources.Load<Sprite>("Stage1_Image"), false, true));

            //ステージ2
            string stage2 = "ウチュウくうかん。いつもよりカラダがカルい！ブラックホールにチュウイだ！";
            stages.Add(2, new Stage(2, "ウチュウ", stage2, Resources.Load<Sprite>("Stage2_Image"), false, false));

            isInicialized = true;
        }
    }

    private void Start()
    {
        //ステージ選択時の確認ダイアログをOFF
        startSelectUI.SetActive(false);

        //ステージ選択ボタンはステージが初めて選択されるまでOFF
        startButton.interactable = false;

        //フリスビー説明テキストをOFF
        frisInfoText.text = null;

        //登録してあるステージ分だけボタンを生成
        for (int i = 0; i < stages.Count; i++)
        {
            CreateStageSelectButton(i);
        }

        //全てのステージをクリアしたかどうかチェック
        if (!isAllStageCleared)
        {
            //全クリしていたら
            if (CheckAllStageCleared())
            {
                //演出を出す
                allClearUI.SetActive(true);

                //staticなフラグを下す
                isAllStageCleared = true;
            }
        }

        BGMManager.bgmManager.PlayBgm(bgm);
    }

    //全てのステージをクリアしたかどうか
    //すべてクリアしていたらtrue
    private bool CheckAllStageCleared()
    {
        int i = 0;
        while (true)
        {
            //クリアしていなかったらループを抜ける
            if (!GetStageClearFlag(i))
            {
                //クリアしていないステージを点滅させる
                stagePanel.transform.GetChild(i).GetComponent<Animator>().Play("Flash");
                break;
            }

            //全て回り切ったらtrueを返す
            if (i == stages.Count - 1)
            {
                return true;
            }
            i++;
        }

        return false;
    }


    //ショップのシーンへ飛ぶ
    public void ToShopScene()
    {
        SEManager.seManager.PlaySE(inputSEVol, inputSE);
        GameManager.gameManager.NextScene("Shop");
        SceneManager.LoadScene("Shop");
    }

    //ステージに挑戦するか確認するUIを表示
    public void ActiveStartUI()
    {
        //所持中のフリスビーを全て調べる
        foreach (FrisbeeItem frisbeeItem in ItemManager.items.Values)
        {
            //所持中かどうか判定
            if (ItemManager.itemManager.GetItemFlag(frisbeeItem.Num))
            {
                //すでにパネルが作られているなら、削除して作り直す
                CheckChild(frisbeeItem.Name);
                CreateFrisbeeSelectButton(frisbeeItem);
            }
        }

        SEManager.seManager.PlaySE(inputSEVol, inputSE);
        //UIを見えるようにする
        startSelectUI.SetActive(true);
    }

    //パネルの子オブジェクトを調べる。(所持中のフリスビー調べる)
    //同じ名前のフリスビーのボタンがあるなら削除する
    private void CheckChild(string name)
    {
        foreach (Transform child in itemPanel.transform)
        {
            if (name == child.name)
            {
                Destroy(child.gameObject);
                return;
            }
        }
        return;
    }

    //ステージに1回でも入場したか
    public bool GetStageEnteredFlag(int num)
    {
        return GetStage(num).Entered;
    }

    //ステージ入場フラグを立てる
    public void ActiveStageEnteredFlag(int num)
    {
        GetStage(num).Entered = true;
    }

    //ステージをクリアしているか
    //numは登録時のステージ番号に対応する
    public bool GetStageClearFlag(int num)
    {
        return GetStage(num).Completed;
    }

    //ステージクリアのフラグを立てる
    //numは登録時のステージ番号に対応する
    public void AvtiveStageClearFlag(int num)
    {
        GetStage(num).Completed = true;
    }

    //ディクショナリのステージを取得する
    //numは登録時のステージ番号に対応する
    private Stage GetStage(int stageNum)
    {
        return stages[stageNum];
    }


    //ステージセレクトのボタンを生成
    private void CreateStageSelectButton(int stageNum)
    {
        //ボタンを生成
        GameObject selectStageButton = Instantiate(stageSelectButtonPrefab);

        //パネルに付ける（グリッドレイアウトがついている）
        selectStageButton.transform.SetParent(stagePanel.transform);

        //大きさや位置調整
        selectStageButton.transform.localPosition = new Vector3(0, 0, 0);
        selectStageButton.transform.localScale = new Vector3(1, 1, 1);

        //ボタンを取得
        Button button = selectStageButton.transform.GetChild(0).GetComponent<Button>();

        //ひとつ前のステージが未クリアなら無効にする
        if (stageNum != 0)
        {
            if (!GetStageClearFlag(stageNum - 1))
            {
                button.interactable = false;
            }
        }

        //処理を追加
        button.onClick.AddListener(() => DisplayStageInfo(stageNum));
    }

    //ボタンをクリックした際、ステージ情報をUIに表示する
    public void DisplayStageInfo(int stageNum)
    {
        SEManager.seManager.PlaySE(inputSEVol, inputSE);
        //選択されたステージ
        Stage selectedStage = GetStage(stageNum);

        //情報を取得
        stageNameText.text = selectedStage.StageName;
        stageInfoText.text = selectedStage.StageInfo;
        stageSprite.sprite = selectedStage.StageImage;

        //スタートボタンを押せるように
        startButton.interactable = true;

        //ステージ番号を代入する
        selectedStageNum = selectedStage.StageNum;
    }

    //フリスビーの選択ボタンを生成
    private void CreateFrisbeeSelectButton(FrisbeeItem frisbeeItem)
    {
        //ボタンを生成
        GameObject selectButton = Instantiate(selectButtonPrefab);

        //Grid layoutの子供にする(ItemPanelがGrid)
        selectButton.transform.SetParent(itemPanel.transform);

        //大きさや位置を調整
        selectButton.name = frisbeeItem.Name;
        selectButton.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        selectButton.transform.localPosition = new Vector3(0f, 0f, 0f);

        //画像を設定
        selectButton.GetComponent<Image>().sprite = frisbeeItem.Image;

        //文字を削除
        selectButton.transform.GetChild(0).GetComponent<Text>().text = null;

        //ボタンに処理を追加
        selectButton.GetComponent<Button>().onClick.AddListener(() => GetSelectedFrisbeeInfo(frisbeeItem));

        //ステージシーンへ行くボタンは非アクティブに
        goStageButton.interactable = false;

        //フリスビー情報文を初期化
        frisInfoText.text = "フリスビーを選択してください";
    }

    //選択されたフリスビーの情報を表示する
    public void GetSelectedFrisbeeInfo(FrisbeeItem frisbeeItem)
    {
        if (!arrow.activeSelf)
        {
            arrow.SetActive(true);
        }

        //ステージシーンへ行くボタンをアクティブに
        goStageButton.interactable = true;

        //矢印を表示する
        arrow.transform.localPosition = eventSystem.currentSelectedGameObject.transform.localPosition;

        //SE
        SEManager.seManager.PlaySE(inputSEVol, inputSE);

        //フリスビーの特性を表示
        frisInfoText.text = frisbeeItem.Info;

        //選ばれているフリスビーを代入
        selectedFrisbee = frisbeeItem;
    }


    //ステージに挑戦するか確認するUIを消す
    public void DeactiveStartUI()
    {
        arrow.SetActive(false);
        SEManager.seManager.PlaySE(canselSEVol, cancelSE);
        startSelectUI.SetActive(false);
    }

    //ステージにいく処理
    //フリスビーを選択していない場合、行けない
    public void ToStageScene()
    {
        if (selectedFrisbee == null)
        {
            return;
        }
        GameManager.gameManager.NextScene("Stage" + selectedStageNum);
        SceneManager.LoadScene("Stage" + selectedStageNum);
    }
}
