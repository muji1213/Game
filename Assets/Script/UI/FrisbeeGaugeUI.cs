using UnityEngine;
using UnityEngine.UI;

//このスクリプトは走行時間に応じてゲージをためる
public class FrisbeeGaugeUI : MonoBehaviour
{
    //ゲージのスプライト
    [Header("ゲージのスプライト")] [SerializeField] private Image image;

    public void ChargeGauge(float value)
    {
        //fillAmountでゲージを変化させる
        image.fillAmount = value;
    }
}
