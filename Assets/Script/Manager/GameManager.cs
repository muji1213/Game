using UnityEngine;

public class GameManager : SingleTon<GameManager>
{
    //ステージで取得したポイントを保持する
    //デバッグ用に[hide]にしないこと
    private int point;

    //ステージセレクトで選んだフリスビー
    private FrisbeeItem selectedFrisbee;

    //選択したステージ
    private Stage selectedStage;


    public int Point
    {
        set
        {
            point = value;
        }
        get
        {
            return point;
        }
    }

    public FrisbeeItem SelectedFrisbeeInfo
    {
        set
        {
            selectedFrisbee = value;
        }
        get
        {
            return selectedFrisbee;
        }
    }

    public Stage SelectedStageInfo
    {
        set
        {
            selectedStage = value;
        }
        get
        {
            return selectedStage;
        }
    }
}
