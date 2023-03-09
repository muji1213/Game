using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OnStage
{
    public class UIManager : MonoBehaviour
    {
        [Header("カウントダウンUI")] [SerializeField] GameObject CountDownUI;
        [Header("ステージクリア後に表示するUI")] [SerializeField] GameObject resultUI;
        [Header("ポーズUI")] [SerializeField] GameObject poseUI;
        [Header("リトライUI")] [SerializeField] GameObject retryUI;
        [Header("スコアUI")] [SerializeField] GameObject scoreUI;
        [Header("ライフUI")] [SerializeField] GameObject lifeUI;
        [Header("ゲージUI")] [SerializeField] GameObject gaugeUI;
        [Header("評価UI")] [SerializeField] GameObject evaluateUI;
        [Header("フェード")] [SerializeField] GameObject fadeUI;


        //UIは最初全て非表示
        public void Inicialize()
        {
            resultUI.SetActive(false);
            poseUI.SetActive(false);
            retryUI.SetActive(false);
            scoreUI.SetActive(false);
            gaugeUI.SetActive(false);
            evaluateUI.SetActive(false);
            fadeUI.SetActive(true);
        }

        /// <summary>
        /// フェードを有効にする
        /// </summary>
        public void ActiveFadeUI()
        {
            fadeUI.SetActive(true);
        }

        /// <summary>
        /// フェードが終わっているかどうか
        /// </summary>
        /// <returns></returns>
        public bool isFadeEnd()
        {
            return fadeUI.GetComponent<FadeUI>().IsFadeEnd;
        }

        /// <summary>
        /// カウントダウンUIを有効にする
        /// </summary>
        public void ActiveCountDownUI()
        {
            CountDownUI.GetComponent<CountDownUI>().StartCountDown();
        }

        /// <summary>
        /// カウントダウンが終わっているかどうか
        /// </summary>
        public bool isCountDownEnd()
        {
            return CountDownUI.GetComponent<CountDownUI>().CountdownEnd;
        }

        /// <summary>
        /// スコアUIを有効にする
        /// </summary>
        public void ActiveScoreUI()
        {
            scoreUI.SetActive(true);
        }

        /// <summary>
        /// ゲージを有効にする
        /// </summary>
        public void ActiveGaugeUI()
        {
            gaugeUI.SetActive(true);
        }

        /// <summary>
        /// UIを表示
        /// ハートの個数をセット
        /// </summary>
        public void ActiveEvaluateUIAndLife(int frisbeeHP)
        {
            //Good,Great,Excellentいづれかを表示する
            evaluateUI.SetActive(true);
            evaluateUI.GetComponent<EvaluateUI>().Evaluate(frisbeeHP);

            //
            lifeUI.SetActive(true);
            lifeUI.GetComponent<LifeUI>().SetInitialLife(frisbeeHP);
        }


        /// <summary>
        /// フリスビーが障害物に衝突時、ライフを減らす
        /// </summary>
        public void ReduceHPUI()
        {
            lifeUI.GetComponent<LifeUI>().DestroyLife();
        }

        /// <summary>
        /// ポーズUIを有効にする
        /// </summary>
        public void ActivePoseUI()
        {
            poseUI.SetActive(true);
        }

        /// <summary>
        /// ポーズUIを無効にする
        /// </summary>
        public void DeactivePoseUI()
        {
            poseUI.SetActive(false);
        }

        /// <summary>
        /// リトライUIを有効にする
        /// </summary>
        public void ActiveRetryUI(float value)
        {
            retryUI.SetActive(true);
            retryUI.GetComponent<RetryUI>().SetClearPer(value);
        }

        /// <summary>
        /// 走行距離に応じてゲージをためる
        /// </summary>
        /// <param name="value"></param>
        public void ChargeGaugeUI(float value)
        {
            gaugeUI.GetComponent<FrisbeeGaugeUI>().ChargeGauge(value);
        }


        /// <summary>
        /// リザルト画面を有効にする
        /// </summary>
        public void ActiveResultUI(int score, int nodamage, int maxHP)
        {
            //リザルトUIを表示
            resultUI.SetActive(true);
            resultUI.GetComponent<ResultUI>().SetPoint(score, nodamage, maxHP);
        }

        /// <summary>
        /// コイン取得時などに、スコアの表示を変える
        /// </summary>
        /// <param name="num"></param>
        public void ChangeScoreUI(int num)
        {
            scoreUI.GetComponent<ScoreUI>().ChangeNum(num);
        }
    }
}
