using UnityEngine;
using UnityEngine.UI;

//�X�e�[�W���̎擾�|�C���g��\������
public class ScoreUI : MonoBehaviour
{
    //�X�R�A��\������e�L�X�g
    [Header("�X�R�A�\���e�L�X�g")][SerializeField]private Text pointText;

    public void ChangeNum(int num)
    {
        pointText.text = num.ToString();
    }
}
