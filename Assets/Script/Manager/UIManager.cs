using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OnStage
{
    public class UIManager : MonoBehaviour
    {
        [Header("�J�E���g�_�E��UI")] [SerializeField] GameObject CountDownUI;
        [Header("�X�e�[�W�N���A��ɕ\������UI")] [SerializeField] GameObject resultUI;
        [Header("�|�[�YUI")] [SerializeField] GameObject poseUI;
        [Header("���g���CUI")] [SerializeField] GameObject retryUI;
        [Header("�X�R�AUI")] [SerializeField] GameObject scoreUI;
        [Header("���C�tUI")] [SerializeField] GameObject lifeUI;
        [Header("�Q�[�WUI")] [SerializeField] GameObject gaugeUI;
        [Header("�]��UI")] [SerializeField] GameObject evaluateUI;
        [Header("�t�F�[�h")] [SerializeField] GameObject fadeUI;


        //UI�͍ŏ��S�Ĕ�\��
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
        /// �t�F�[�h��L���ɂ���
        /// </summary>
        public void ActiveFadeUI()
        {
            fadeUI.SetActive(true);
        }

        /// <summary>
        /// �t�F�[�h���I����Ă��邩�ǂ���
        /// </summary>
        /// <returns></returns>
        public bool isFadeEnd()
        {
            return fadeUI.GetComponent<FadeUI>().IsFadeEnd;
        }

        /// <summary>
        /// �J�E���g�_�E��UI��L���ɂ���
        /// </summary>
        public void ActiveCountDownUI()
        {
            CountDownUI.GetComponent<CountDownUI>().StartCountDown();
        }

        /// <summary>
        /// �J�E���g�_�E�����I����Ă��邩�ǂ���
        /// </summary>
        public bool isCountDownEnd()
        {
            return CountDownUI.GetComponent<CountDownUI>().CountdownEnd;
        }

        /// <summary>
        /// �X�R�AUI��L���ɂ���
        /// </summary>
        public void ActiveScoreUI()
        {
            scoreUI.SetActive(true);
        }

        /// <summary>
        /// �Q�[�W��L���ɂ���
        /// </summary>
        public void ActiveGaugeUI()
        {
            gaugeUI.SetActive(true);
        }

        /// <summary>
        /// UI��\��
        /// �n�[�g�̌����Z�b�g
        /// </summary>
        public void ActiveEvaluateUIAndLife(int frisbeeHP)
        {
            //Good,Great,Excellent���Âꂩ��\������
            evaluateUI.SetActive(true);
            evaluateUI.GetComponent<EvaluateUI>().Evaluate(frisbeeHP);

            //
            lifeUI.SetActive(true);
            lifeUI.GetComponent<LifeUI>().SetInitialLife(frisbeeHP);
        }


        /// <summary>
        /// �t���X�r�[����Q���ɏՓˎ��A���C�t�����炷
        /// </summary>
        public void ReduceHPUI()
        {
            lifeUI.GetComponent<LifeUI>().DestroyLife();
        }

        /// <summary>
        /// �|�[�YUI��L���ɂ���
        /// </summary>
        public void ActivePoseUI()
        {
            poseUI.SetActive(true);
        }

        /// <summary>
        /// �|�[�YUI�𖳌��ɂ���
        /// </summary>
        public void DeactivePoseUI()
        {
            poseUI.SetActive(false);
        }

        /// <summary>
        /// ���g���CUI��L���ɂ���
        /// </summary>
        public void ActiveRetryUI(float value)
        {
            retryUI.SetActive(true);
            retryUI.GetComponent<RetryUI>().SetClearPer(value);
        }

        /// <summary>
        /// ���s�����ɉ����ăQ�[�W�����߂�
        /// </summary>
        /// <param name="value"></param>
        public void ChargeGaugeUI(float value)
        {
            gaugeUI.GetComponent<FrisbeeGaugeUI>().ChargeGauge(value);
        }


        /// <summary>
        /// ���U���g��ʂ�L���ɂ���
        /// </summary>
        public void ActiveResultUI(int score, int nodamage, int maxHP)
        {
            //���U���gUI��\��
            resultUI.SetActive(true);
            resultUI.GetComponent<ResultUI>().SetPoint(score, nodamage, maxHP);
        }

        /// <summary>
        /// �R�C���擾���ȂǂɁA�X�R�A�̕\����ς���
        /// </summary>
        /// <param name="num"></param>
        public void ChangeScoreUI(int num)
        {
            scoreUI.GetComponent<ScoreUI>().ChangeNum(num);
        }
    }
}
