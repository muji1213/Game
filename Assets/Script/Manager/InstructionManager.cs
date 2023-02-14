using UnityEngine;
using UnityEngine.UI;


public class InstructionManager : SceneChanger
{
    [Header("整列用パネル")] [SerializeField] GameObject panel;
    [Header("パネルの大きさ")] [SerializeField] float panelWidth;
    [Header("左に移動するボタン")] [SerializeField] GameObject goPreButton;
    [Header("右に移動するボタン")] [SerializeField] GameObject goNextButton;

    [Header("BGM")] [SerializeField] AudioClip bgm;
    [Header("左右ボタン押下SE")] [SerializeField] AudioClip buttonSE;

    //パネルの数
    private int panelNum;

    //グリッドのセルサイズ
    private float celWidth;

    //現在見ているパネル
    private int index;

    //パネルの現在座標
    private float currentPanelPosX;

    //パネルの次の座標
    private float nextPanelPosX;


    void Start()
    {
        //BGM
        BGMManager.bgmManager.PlayBgm(bgm);

        //パネル数を数える
        panelNum = panel.transform.childCount;

        //インデックスを0に
        index = 0;

        //一番最初のパネルを画面に映す
        currentPanelPosX = 0;

        //セルサイズを取得
        celWidth = panel.GetComponent<GridLayoutGroup>().cellSize.x;
    }

    private void FixedUpdate()
    {
        //目標の座標まで徐々に近づける
        currentPanelPosX = Mathf.Lerp(currentPanelPosX, nextPanelPosX, 0.1f);

        //目標地点までパネルを移動
        panel.transform.localPosition = new Vector3(currentPanelPosX, 0, 0);

        //目標座標までパネルが近づいたら現在座標を目標座標と同じにする(Lerpは完全一緒にならない)
        if (Mathf.Abs(currentPanelPosX - nextPanelPosX) < 0.01f)
        {
            currentPanelPosX = nextPanelPosX;
        }

        //最後のパネルなら次へボタンは映さない
        if(index == panelNum - 1)
        {
            goNextButton.SetActive(false);
        }
        //最初のパネルなら戻るボタンは映さない
        else if(index == 0)
        {
            goPreButton.SetActive(false);
        }
        //それ以外なら映す
        else
        {
            goNextButton.SetActive(true);
            goPreButton.SetActive(true);
        }
    }

    public void MoveNextPanel()
    {
        //現在座標と目標座標が同じでないなら押せないようにする(連打されると座標がずれる)
        if (currentPanelPosX != nextPanelPosX)
        {
            return;
        }

        index++;

        //インデックスがパネル個数を超えていた場合、足さない
        if (index > panelNum - 1)
        {
            index = panelNum - 1;
            return;
        }
        else
        {
            //SE
            SEManager.seManager.PlaySe(buttonSE);

            //目標座標を設定する
            nextPanelPosX = currentPanelPosX - (panelWidth + celWidth);
        }
    }

    public void MovePrePanel()
    {
        //現在座標と目標座標が同じでないなら押せないようにする(連打されると座標がずれる)
        if (currentPanelPosX != nextPanelPosX)
        {
            return;
        }

        index--;

        //インデックスが0以下なら戻す
        if (index < 0)
        {
            index = 0;
            return;
        }
        else
        {
            //SE
            SEManager.seManager.PlaySe(buttonSE);

            //目標座標を設定
            nextPanelPosX = currentPanelPosX + (panelWidth + celWidth);
        }
    }

}
