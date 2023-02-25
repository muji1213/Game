using UnityEngine;

//このスクリプトはポーズUIです
public class PoseUI : SceneChanger
{
    //ステージマネージャー
    private StageManager stageManager;
    
    private void Start()
    {
        //初期では非表示
        this.gameObject.SetActive(false);

        //ステージマネージャーを取得
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
    }

    //コンテニューボタンを押した場合
    public void Continue()
    {
        //再度進行する
        stageManager.Pose();
    }
}
