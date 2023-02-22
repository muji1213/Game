using UnityEngine;
using Cinemachine;

public class StageEnterCamera : MonoBehaviour
{
    private CinemachineDollyCart dollyCart;
    [Header("�p�X")] [SerializeField] private CinemachineSmoothPath path;
    [Header("�J�����̑���")] [SerializeField] [Range(0, 2)] private float speed;

    //���[�r�[���I�������
    private bool isFinished = false;

    //���[�r�[���J�n���邩�ǂ���
    private bool isStart = false;

    //�v���C���[��Ǐ]����X�N���v�g(������ꎞ�̂ݖ����ɂ��Ă���)
    private CameraFollower cameraFollower;

    //���[�r�[��؂�グ��l(Lerp�̂���position��0�ɂȂ�Ȃ�)
    [Header("�p�X��؂�グ�鋗��")] [SerializeField] [Range(0.1f, 10)] private float per;

    private void Awake()
    {
        cameraFollower = GetComponent<CameraFollower>();
        cameraFollower.enabled = false;

        dollyCart = this.GetComponent<CinemachineDollyCart>();

        //�������ς݂����ׂ�
        if (StageSelectManager.stageSelectManager.GetStageEnteredFlag(StageSelectManager.selectedStageNum))
        {
            //����ς݂Ȃ炷���ɊJ�n����
            dollyCart.m_Position = 0;
            dollyCart.m_Path = null;
            isFinished = true;
        }
        else
        {
            //�����łȂ��Ȃ烀�[�r�[�𗬂�
            dollyCart.m_Position = path.PathLength;
        }
    }

    void Update()
    {
        //�X�^�[�g�t���O���~��ĂȂ��Ȃ牽�����Ȃ�
        if (!isStart)
        {
            return;
        }


        //�p�X�̈�芄���܂Ői�񂾂�؂�グ��
        if (dollyCart.m_Position <= per)
        {
            dollyCart.m_Path = null;

            //�I���t���O������
            isFinished = true;

            //�������t���O�����낷
            StageSelectManager.stageSelectManager.ActiveStageEnteredFlag(StageSelectManager.selectedStageNum);
            return;
        }
        else if (dollyCart.m_Position <= path.PathLength)
        {
            //�p�X��H��i���X�ɑ��x�𗎂Ƃ��j
            dollyCart.m_Position = Mathf.Lerp(dollyCart.m_Position, 0, speed * Time.deltaTime);
        }
    }

    //���[�r�[���J�n����
    public void StartMovie()
    {
        if (isStart)
        {
            return;
        }
        isStart = true;
    }


    //�I����ʒm����
    public bool MovieFinished()
    {
        return isFinished;
    }
}
