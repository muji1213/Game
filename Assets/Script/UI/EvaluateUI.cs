using UnityEngine;
using UnityEngine.UI;

//�t���X�r�[�𓊂������̕]���ł�
//���s�����ɉ����ĕ]�����オ��܂�
public class EvaluateUI : MonoBehaviour
{
    //�X�v���C�g
    [Header("�G�N�Z�����g")] [SerializeField] Sprite excellent;
    [Header("�O���[�g")] [SerializeField] Sprite great;
    [Header("�O�b�h")] [SerializeField] Sprite good;

    [Header("SE")] [SerializeField] AudioClip SE;
    [Header("SE�̉���")] [SerializeField] [Range(0, 1)] float SEVol = 1;

    [Header("�摜�̃C���X�^���X")] [SerializeField] private Image sprite;

    private void Awake()
    {
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        //�X�e�[�W�}�l�[�W��������active�ɂ����̂ŁAEnable�ň�x�����Ă�
        SEManager.I.PlaySE(SEVol, SE);
    }

    //type��Frisbee��HP
    //�ő�R
    public void Evaluate(int type)
    {
        //�t���X�r�[�𓊂�������HP�ŕ]�����ς��
        switch (type)
        {
            case 1:
                sprite.sprite = good;
                break;

            case 2:
                sprite.sprite = great;
                break;

            case 3:
                sprite.sprite = excellent;
                break;

            case 4:
                sprite.sprite = excellent;
                break;
        }
    }
}
