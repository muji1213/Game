using System.Collections;
using UnityEngine;

//�J�����Ǐ]�p�̃X�N���v�g�ł�
//���̃X�N���v�g�̓J�����̒Ǐ]�A����ёΏۂ̐؂�ւ��A��ʐU���A�J�����̐؂�ւ����s��
public class CameraFollower : MonoBehaviour
{
    private enum State
    {
        RunPhase,
        FrisbeePhase,
        Stop
    }

    //�X�e�[�g
    private State state;

    //�U�������ǂ���
    [HideInInspector]public bool isShake = false;

    //���C���J����
    [SerializeField] GameObject MainCamera;

    //�t���X�r�[���S�[���ɏՓ˂����ۂɐ؂�ւ���J����
    [SerializeField] GameObject finishCamera;

    //�S�[���̃Q�[���I�u�W�F�N�g
    [SerializeField]private GameObject player;

    //�Ώۂɋ߂Â��܂ł̎���
    [SerializeField, Range(0.01f, 1.0f)] private float _positionLerpSpeed = 0.1f;

    //�w��̊p�x�܂ŉ�鑬�x
    [SerializeField, Range(0.1f, 1.0f)] private float _rotationLerpSpeed = 0.1f;

    //�J�����̈ʒu����
    //�Ώۂ̂ǂꂭ�炢�܂��ɂ���
    [SerializeField] private float _distanceForwards = 5f;

    //�Ώۂ̂ǂꂭ�炢��ɂ���
    [SerializeField] private float _distanceUpwards = 1f;

    //�J�����̌�������
    [SerializeField] private Vector3 LookTo;

    private void Start()
    {
        state = State.RunPhase;
        //���C���J������On
        MainCamera.SetActive(true);
    }

    private void Update()
    {
        //�t���X�r�[���J�n�ʒu�ɂ����Ƃ��ɃJ�������Œ肷��
        if (state == State.Stop)
        {
            return;
        }

        //�v���C���[�����ɗ������ہA�������̓t���X�r�[��������ꂽ�ۂɃJ�����̈ʒu���ړ�������
        if (state == State.RunPhase)
        {
            //�Ώۂ̒ǂ������x
            float posSpeed = _positionLerpSpeed;

            //�Ώۂ̈ʒu�ւ̈ړ�
            //posTo�F�Ǐ]�̑Ώ�
            Vector3 posTo = player.transform.position + transform.up * _distanceUpwards + transform.forward * _distanceForwards;

            //���݈ʒu����posTo�֋߂Â�
            transform.position = Vector3.Slerp(transform.position, posTo, posSpeed);

            //�J�����̉�]
            float rotSpeed = _rotationLerpSpeed;

            //�J��������]������
            //LookTo�͂ق��̃N���X����Ăяo�����ۂɈ����Ƃ��ēn�����
            Quaternion rotTo = Quaternion.LookRotation(LookTo);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotTo, rotSpeed);
        }
        else
        {
            //�t���X�r�[���J�n�ʒu�ɂ�����ʒu���Œ肷��
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, player.transform.position.z + _distanceForwards - 3);
        }
    }

    //�J�����̒Ǐ]�Ώۂ�������
    //�ŏ���Unity�����ɒǏ]���A�t���X�r�[�𓊂����u�ԂɁA�t���X�r�[�Ƀ^�[�Q�b�g��ύX
    public void ChangeTarget(GameObject newTarget)
    {
        player = newTarget;
    }

    //�J�����̈ʒu��Ώۂֈړ�����
    //distansForward:�Ώۂ���ǂꂭ�炢�܂���
    //distansUpward:�Ώۂ���ǂꂭ�炢�ォ
    //�J�����̌�������
    public void ZoomToTarget(float distansForward, float distanceUpward, Vector3 LookTo)
    {
        this._distanceForwards = distansForward;
        this._distanceUpwards = distanceUpward;
        this.LookTo = LookTo;
    }

    //�t���X�r�[���J�n�ʒu�ɂ�����J�������Œ肷��
    public void FixCamera(GameObject StartPos)
    {
        this.transform.position = new Vector3(StartPos.transform.position.x, StartPos.transform.position.y,
                                                player.transform.position.z + _distanceForwards - 3);

        //�J�n�X�e�[�g��
        state = State.FrisbeePhase;
    }

    //�t���X�r�[�����S���A�Ǐ]���~������i�t���X�r�[�����֗����Ă������߁j
    public void StopCamera()
    {
        state = State.Stop;
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
        isShake = true;

        float elapsed = 0f; // �o�ߎ���

        //�U�����Ԃ��o�ߎ��Ԉȉ��ł����
        while (elapsed < duration)
        {
            //xy���ꂼ�ꃉ���_���ɐU��
            //1-(elapsed / duration)�Ŏ��Ԍo�߂���قǐU�����������Ȃ�
            var x = pos.x + Random.Range(-2f, 2f) * magnitude * (1 - (elapsed / duration));
            var y = pos.y + Random.Range(-2f, 2f) * magnitude * (1 - (elapsed / duration));

            //���ꂼ��ʒu��K�p
            transform.localPosition = new Vector3(x, y, player.transform.position.z + _distanceForwards - 3);
            elapsed += Time.deltaTime;

            yield return new WaitForSeconds(Time.deltaTime / 60);
        }

        //�U�����I��������A�J�����ʒu�����ɖ߂�
        isShake = false;
        transform.localPosition = pos;
    }

    //�t���X�r�[���S�[���ɏՓ˂�����t�B�j�b�V���J�����֐؂�ւ���i�t�B�j�b�V���J�����̕����[�x�������j
    public void ChangeFinishCamera()
    {
        finishCamera.SetActive(true);
    }

    //���C���J�����ɖ߂�
    public void ChangeNormalCamera()
    {
        finishCamera.SetActive(false);
    }
}
