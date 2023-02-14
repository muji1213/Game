using UnityEngine;

//ステージのステータスを設定します
public class Stage
{
    private int stageNum; //ステージ番号
    private string stageInfo;　//ステージの情報
    private string stageName; //ステージ名
    private Sprite stageImage; //ステージの画像
    private bool isEntered; //初回入場フラグ
    private bool isCompleted; //クリアフラグ

    public Stage(int stageNum, string stageName, string stageInfo, Sprite stageImage, bool isEntered, bool isCompleted)
    {
        this.stageNum = stageNum;
        this.stageInfo = stageInfo;
        this.stageName = stageName;
        this.stageImage = stageImage;
        this.isEntered = isEntered;
        this.isCompleted = isCompleted;
    }

    //ステージ番号を取得
    public int StageNum
    {
        get { return stageNum; }
    }

    //ステージ情報を取得
    public string StageInfo
    {
        get { return stageInfo; }
    }

    //ステージ名を取得
    public string StageName
    {
        get { return stageName; }
    }

    //ステージの画像を取得
    public Sprite StageImage
    {
        get { return stageImage; }
    }

    //ステージに入場済みかどうか
    public bool Entered
    {
        get
        {
            return isEntered;
        }
        set
        {
            isEntered = value;
        }
    }

    //ステージが攻略済みかどうか
    public bool Completed
    {
        get
        {
            return isCompleted;
        }
        set
        {
            isCompleted = value;
        }
    }
}
