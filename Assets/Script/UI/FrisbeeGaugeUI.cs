using UnityEngine;
using UnityEngine.UI;

//���̃X�N���v�g�͑��s���Ԃɉ����ăQ�[�W�����߂�
public class FrisbeeGaugeUI : MonoBehaviour
{
    //�Q�[�W�̃X�v���C�g
    [Header("�Q�[�W�̃X�v���C�g")] [SerializeField] private Image image;

    public void ChargeGauge(float value)
    {
        //fillAmount�ŃQ�[�W��ω�������
        image.fillAmount = value;
    }
}
