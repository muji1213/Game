using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    [Header("���߂��|�C���g�e�L�X�g")] [SerializeField] Text getPointText;
    [Header("�m�[�_���[�W�{�[�i�X�e�L�X�g")] [SerializeField] Text nodamageText;
    [Header("�ő�HP�{�[�i�X�e�L�X�g")] [SerializeField] Text maxHPText;
    [Header("�g�[�^���|�C���g")] [SerializeField] Text totalPointText;

    //�|�C���g��ݒ�
    public void SetPoint(int score, int nodamage, int maxHP)
    {
        //�X�e�[�W���ł��߂��|�C���g����
        getPointText.text = score.ToString();

        //�m�[�_���[�W���Z�|�C���g�\��
        nodamageText.text = nodamage.ToString();

        //�ő�HP���Z�|�C���g
        maxHPText.text = maxHP.ToString();

        //�g�[�^����\��
        totalPointText.text = (nodamage + maxHP + score).ToString() + "�|�C���g";
    }
}
