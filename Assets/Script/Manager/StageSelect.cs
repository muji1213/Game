using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    // eventSystemを取得するための変数宣言
    [Header("イベントシステム")] [SerializeField] EventSystem eventSystem;

    [Header("------ステージセレクト画面に関する設定---------")]
    [Header("フリスビー選択画面を表示するボタン")] [SerializeField] private Button displayFrisbeeSelectButton;
    [Header("フリスビー選択のボタンプレハブ")] [SerializeField] GameObject selectButtonPrefab;
    [Header("ステージ名テキスト")] [SerializeField] Text stageNameText;
    [Header("ステージフレーバーテキスト")] [SerializeField] Text stageInfoText;
    [Header("選択中のステージ画像表示")] [SerializeField] Image stageSprite;
    [Header("ステージ選択ボタンのプレハブ")] [SerializeField] GameObject stageSelectButtonPrefab;
    [Header("ステージボタンパネル")] [SerializeField] GameObject stagePanel;
    [Header("全てのステージをクリアした際に表示するUI")] [SerializeField] GameObject allClearUI;

    [Header("------フリスビー選択画面に関する設定--------")]
    [Header("フリスビーパネル")] [SerializeField] GameObject frisbeeSelectPanel;
    [Header("フリスビー選択画面")] [SerializeField] GameObject startSelectUI;
    [Header("選択中のフリスビーの上に表示する矢印")] [SerializeField] GameObject arrow;
    [Header("フリスビーのフレーバーテキスト表示用テキスト")] [SerializeField] Text frisInfoText;
    [Header("ステージシーンへいくためのボタン")] [SerializeField] Button goStageButton;

    [Header("---------音に関する設定----------")]
    [Header("BGM")] [SerializeField] AudioClip bgm;
    [Header("ボタン押下時のSE")] [SerializeField] AudioClip inputSE;
    [Header("ボタンSEの音量")] [SerializeField] [Range(0, 1)] float inputSEVol = 1;
    [Header("キャンセル時のSE")] [SerializeField] AudioClip cancelSE;
    [Header("キャンセルSEの音量")] [SerializeField] [Range(0, 1)] float cancelSEVol = 1;

    private void Start()
    {
        //ステージ選択時の確認ダイアログをOFF
        startSelectUI.SetActive(false);

        //ステージ選択ボタンはステージが初めて選択されるまでOFF
        displayFrisbeeSelectButton.interactable = false;

        //フリスビー説明テキストをOFF
        frisInfoText.text = null;

        //登録してあるステージ分だけボタンを生成
        //生成したステージがクリアしていないなら、点滅させる
        for (int i = 0; i < StageSelectManager.I.StageCount(); i++)
        {
            //0番目は必ず生成する
            if (i == 0)
            {
                CreateStageSelectButton(i, StageSelectManager.I.IsStageCleared(0));
            }
            //1番目以降
            else if (i >= 1)
            {
                //ひとつ前のステージをクリアしていないなら生成しない
                if (!StageSelectManager.I.IsPreStageCleared(i))
                {
                    
                }
                //クリアしているなら生成する
                else
                {
                    CreateStageSelectButton(i, StageSelectManager.I.IsStageCleared(i));
                }
            }
        }

        //全てのステージをクリアしたかどうかチェック

        //全クリしていたら
        if (StageSelectManager.I.CheckAllStageCleared())
        {
            //演出を出す
            allClearUI.SetActive(true);
        }

        BGMManager.I.PlayBgm(bgm);
    }

    //ステージに挑戦するか確認するUIを表示
    public void ActiveStartUI()
    {
        //所持中のフリスビーを全て調べる
        foreach (FrisbeeItem frisbeeItem in ItemManager.items.Values)
        {
            //所持中かどうか判定
            if (ItemManager.I.GetItemFlag(frisbeeItem.Num))
            {
                //すでにパネルが作られているなら、削除して作り直す
                CheckChild(frisbeeItem.Name);
                CreateFrisbeeSelectButton(frisbeeItem);
            }
        }

        SEManager.I.PlaySE(inputSEVol, inputSE);

        //UIを見えるようにする
        startSelectUI.SetActive(true);
    }

    //パネルの子オブジェクトを調べる。(所持中のフリスビー調べる)
    //同じ名前のフリスビーのボタンがあるなら削除する
    private void CheckChild(string name)
    {
        foreach (Transform child in frisbeeSelectPanel.transform)
        {
            if (name == child.name)
            {
                Destroy(child.gameObject);
                return;
            }
        }
        return;
    }

    //ステージセレクトのボタンを生成
    //引数isClearedがfalseなら点滅させる
    private void CreateStageSelectButton(int stageNum, bool isCleared)
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

        //処理を追加
        button.onClick.AddListener(() => DisplayStageInfo(stageNum));

        if (!isCleared)
        {
            selectStageButton.GetComponent<Animator>().Play("Flash");
        }
    }

    //ボタンをクリックした際、ステージ情報をUIに表示する
    public void DisplayStageInfo(int stageNum)
    {
        SEManager.I.PlaySE(inputSEVol, inputSE);

        //選択されたステージ
        Stage selectedStage = StageSelectManager.I.GetStage(stageNum);

        //情報を取得
        stageNameText.text = selectedStage.StageName;
        stageInfoText.text = selectedStage.StageInfo;
        stageSprite.sprite = selectedStage.StageImage;

        //スタートボタンを押せるように
        displayFrisbeeSelectButton.interactable = true;

        //ステージ番号を代入する
        GameManager.I.SelectedStageInfo = selectedStage;
    }

    //フリスビーの選択ボタンを生成
    private void CreateFrisbeeSelectButton(FrisbeeItem frisbeeItem)
    {
        //ボタンを生成
        GameObject selectButton = Instantiate(selectButtonPrefab);

        //Grid layoutの子供にする(ItemPanelがGrid)
        selectButton.transform.SetParent(frisbeeSelectPanel.transform);

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
        SEManager.I.PlaySE(inputSEVol, inputSE);

        //フリスビーの特性を表示
        frisInfoText.text = frisbeeItem.Info;

        //選ばれているフリスビーを代入
        GameManager.I.SelectedFrisbeeInfo = frisbeeItem;
    }

    //ステージに挑戦するか確認するUIを消す
    public void DeactiveStartUI()
    {
        //矢印を消す
        arrow.SetActive(false);

        //SEを鳴らす
        SEManager.I.PlaySE(cancelSEVol, cancelSE);

        //フリスビー選択画面を消す
        startSelectUI.SetActive(false);

        //選択中のフリスビーを解除する
        GameManager.I.SelectedFrisbeeInfo = null;

        //選択中のステージを解除する
        GameManager.I.SelectedStageInfo = null;
    }

    //ステージにいく処理
    //フリスビーを選択していない場合、行けない
    public void ToStageScene()
    {
        if (GameManager.I.SelectedFrisbeeInfo == null)
        {
            return;
        }

        SceneChanger.I.ToStageScene(GameManager.I.SelectedStageInfo.StageNum);
    }

    //ショップのシーンへ飛ぶ
    public void ToShopScene()
    {
        SEManager.I.PlaySE(inputSEVol, inputSE);
        SceneChanger.I.ToShopScene();
    }

    //タイトルへ
    public void ToTitleScene()
    {
        SEManager.I.PlaySE(cancelSEVol, cancelSE);
        SceneChanger.I.ToTitleScene();
    }
}
