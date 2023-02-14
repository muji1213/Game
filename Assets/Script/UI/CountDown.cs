using UnityEngine;
using UnityEngine.UI;

//���̃X�N���v�g�̓J�E���g�_�E���̐����̕\������уA�j���[�V�����̐���ł�
public class CountDown : MonoBehaviour
{
    //�e��X�v���C�g
    [SerializeField] Sprite one;
    [SerializeField] Sprite two;
    [SerializeField] Sprite three;
    [SerializeField] Sprite go;
    [SerializeField] Image numberImage;

    [Header("321�̉�")] [SerializeField] AudioClip countdownSE;
    [Header("Go�̉�")] [SerializeField] AudioClip goSE;
    [Header("�o�b�N�O���E���h")] [SerializeField] GameObject bg;

    //�J�E���g�_�E�����ԁi3,2,1,Go�Ȃ̂�+1�b����j
    private float countdownTimer = 4.0f;

    //�A�j���[�^�[
    private Animator anim;

    //���݂̐���
    private Sprite currentSprite = null;

    //�O�̐���
    private Sprite preSprite = null;

    //�J�E���g�_�E�����I��������ǂ���
    [HideInInspector] public bool isCountdownEnd = false;

    void Start()
    {
        bg.SetActive(false);
        //�A�j���[�^�[���擾
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// �J�E���g�_�E���A�j���[�V�������Đ�
    /// </summary>
    private void PlayStartAnimation()
    {
        anim.Play("Countdown_start", 0, 0);
    }

    public void StartCountDown()
    {
        if (!bg.activeSelf)
        {
            bg.SetActive(true); ;
        }

        //�J�E���g�_�E����0�ȉ��Ȃ�
        if (countdownTimer < 0)
        {
            //�t���O�𗧂Ă�
            isCountdownEnd = true;

            //���̃Q�[���I�u�W�F�N�g�͖�����
            this.gameObject.SetActive(false);
        }
        //�J�E���g�_�E����1�ȉ��Ȃ�
        else if (countdownTimer < 1)
        {
            //�X�v���C�g��ύX
            numberImage.sprite = go;

            //���݂̃X�v���C�g���X�V
            currentSprite = numberImage.sprite;
        }
        //�J�E���g�_�E����2�ȉ��Ȃ�
        else if (countdownTimer < 2)
        {
            //�X�v���C�g���X�V
            numberImage.sprite = one;

            //���݂̃X�v���C�g���X�V
            currentSprite = numberImage.sprite;
        }
        //�J�E���g�_�E����3�ȉ��Ȃ�
        else if (countdownTimer < 3)
        {
            //�X�v���C�g���X�V
            numberImage.sprite = two;

            //���݂̃X�v���C�g���X�V
            currentSprite = numberImage.sprite;
        }
        //�J�E���g�_�E����4�ȉ��Ȃ�
        else
        {
            //�X�v���C�g���X�V
            numberImage.sprite = three;

            //���݂̃X�v���C�g���X�V
            currentSprite = numberImage.sprite;
        }

        //�X�v���C�g�̍X�V���������ꍇ
        if (preSprite != currentSprite)
        {
            if(currentSprite != go)
            {
                SEManager.seManager.PlaySe(countdownSE);
            }
            else
            {
                SEManager.seManager.PlaySe(goSE);
            }
            //�A�j���[�V�������Đ�����
            PlayStartAnimation();
        }


        //�J�E���g�_�E������
        countdownTimer -= Time.deltaTime;

        //���݂̃X�v���C�g���L��������
        preSprite = currentSprite;
    }
}