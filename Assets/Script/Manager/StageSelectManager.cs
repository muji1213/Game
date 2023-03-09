using UnityEngine;
using System.Collections.Generic;

//ステージ選択画面に関するスクリプトです
public class StageSelectManager : SingleTon<StageSelectManager>
{
    
    //初期化したかどうか、シーン間で保持される
    private static bool isInicialized = false;

    //ステージのディクショナリ
    private static Dictionary<int, Stage> stages;

    //全てのステージをクリアしたフラグ
    private static bool isAllStageCleared = false;

    protected override void Init()
    {
        //初期化されているかどうか
        if (!isInicialized)
        {
            I = this;

            stages = new Dictionary<int, Stage>();

            //ステージ0
            string stage0 = "山にカコまれているクンレンジョウ。カケダシのフリスビストはココでレンシュウだ！";
            stages.Add(0, new Stage(0, "クンレンジョウ", stage0, Resources.Load<Sprite>("Stage0_Image"), false, false));

            //ステージ1
            string stage1 = "カゼがあれくるうキョウコク。イワがむきだしで、アルくだけでもせいいっぱい";
            stages.Add(1, new Stage(1, "ボウフウのキョウコク", stage1, Resources.Load<Sprite>("Stage1_Image"), false, false));

            //ステージ2
            string stage2 = "ウチュウくうかん。いつもよりカラダがカルい！ブラックホールにチュウイだ！";
            stages.Add(2, new Stage(2, "ウチュウ", stage2, Resources.Load<Sprite>("Stage2_Image"), false, false));

            isInicialized = true;
        }
    }


    //全てのステージをクリアしたかどうか
    //すべてクリアしていたらtrue
    //クリアしていないステージがある、もしくは全クリアのフラグが下ろされている場合、falseになる
    public bool CheckAllStageCleared()
    {
        if (isAllStageCleared)
        {
            return false;
        }

        int i = 0;
        while (true)
        {
            //クリアしていなかったらループを抜ける
            if (!isStageCleared(i))
            {
                //クリアしていないステージがあるならfalse
                break;
            }

            //全て回り切ったらtrueを返す
            if (i == stages.Count - 1)
            {
                isAllStageCleared = true;
                return true;
            }
            i++;
        }

        return false;
    }

    //ステージに1回でも入場したか
    public bool isStageEntered(int num)
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
    public bool isStageCleared(int num)
    {
        return GetStage(num).Completed;
    }

    //ステージクリアのフラグを立てる
    //numは登録時のステージ番号に対応する
    public void AvtiveStageClearFlag(int num)
    {
        GetStage(num).Completed = true;
    }

    public bool isPreStageCleared(int num)
    {
        if (isStageCleared(num - 1))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //ディクショナリのステージを取得する
    //numは登録時のステージ番号に対応する
    public Stage GetStage(int stageNum)
    {
        return stages[stageNum];
    }

    public int StageCount()
    {
        return stages.Count;
    }
}
