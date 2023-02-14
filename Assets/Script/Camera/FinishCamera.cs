using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���̃X�N���v�g�̓t���X�r�[���S�[���ɏՓ˂����ۂɎg���J�����̃X�N���v�g�ł�
public class FinishCamera : MonoBehaviour
{
    //�S�[���̃Q�[���I�u�W�F�N�g
    [SerializeField] private GameObject target;

    //�J�����̉�]���x
    [SerializeField, Range(0.1f, 1.0f)] private float _rotationLerpSpeed = 0.1f;

    //���̃J����
    private Camera finishCamera;

    private void Start()
    {
        finishCamera = this.GetComponent<Camera>();
        this.gameObject.SetActive(false);
    }
    private void Update()
    {
        float rotSpeed = _rotationLerpSpeed;

        //��ɃS�[���̃Q�[���I�u�W�F�N�g�̕��������悤�ɂ���
        Quaternion rotTo = Quaternion.LookRotation(target.transform.position - this.transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotTo, rotSpeed);
    }

    //�J�����U�����\�b�h
    //duration: �U������
    //magnitude: �U���̑傫��
    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(TheShake(duration, magnitude));
    }

    private IEnumerator TheShake(float duration, float magnitude)
    {
        //�U���J�n�n�_���L�^
        Vector3 pos = transform.localPosition;

        float elapsed = 0f; // �o�ߎ���

        //�U�����Ԃ��o�ߎ��Ԉȉ��ł����
        while (elapsed < duration)
        {
            //xy���ꂼ�ꃉ���_���ɐU��
            //1-(elapsed / duration)�Ŏ��Ԍo�߂���قǐU�����������Ȃ�
            var x = pos.x + Random.Range(-2f, 2f) * magnitude * (1 - (elapsed / duration));
            var y = pos.y + Random.Range(-2f, 2f) * magnitude * (1 - (elapsed / duration));

            //���ꂼ��ʒu��K�p
            transform.localPosition = new Vector3(x, y, pos.z);
            elapsed += Time.unscaledDeltaTime;

            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }

        //�U�����I��������A�J�����ʒu�����ɖ߂�
        transform.localPosition = pos;

        //�U���I����A�`��Ώۂ𑝉�������
        ChangeCallingMask();
    }

    // �t�B�j�b�V�����o�ł́A�Փ˒���A��p�w�i�A����уS�[���̃Q�[���I�u�W�F�N�g�����f��Ȃ�
    // �t�B�j�b�V�����o�I����ɐ�p�w�i���\���A�ق��̃I�u�W�F�N�g��\������
    private void ChangeCallingMask()
    {
        //���C���[12�̓G�t�F�N�g
        //���C���[14�̓t�B�j�b�V�����C���[�ł�

        //�S�Ẵ��C���[��\��
        finishCamera.cullingMask = -1;

        //�G�t�F�N�g���C���[�̔�\��
        finishCamera.cullingMask &= ~(1 << 12);

        //�t�B�j�b�V�����C���[�̔�\��
        finishCamera.cullingMask &= ~(1 << 14);
    }
}
