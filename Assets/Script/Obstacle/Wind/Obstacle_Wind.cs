using UnityEngine;

//��Q���i���j�̃X�N���v�g
//���x����100�ɂ��邱��
public class Obstacle_Wind : UnCollisionable_Obstacle
{
    [Header("��������")] [SerializeField] private Vector3 windVec;
    [Header("���̋���")] [SerializeField] private float windStrength;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume *= (SEManager.seManager.SeVolume / 1.0f);
    }

    //�t���X�r�[���͈͓��ɂ���Ƃ�
    private void OnTriggerStay(Collider other)
    {
        //�t���X�r�[���ǂ�������
        if (other.CompareTag("Frisbee"))
        {
            if (other.GetComponent<Frisbee>().isBlowed)
            {
                //�������ɗ͂�����������
                Rigidbody frisbeeRb = other.GetComponent<Rigidbody>();
                frisbeeRb.AddForce((windVec) * windStrength);
            }
            else
            {
                //�������ɗ͂�����������
                Rigidbody frisbeeRb = other.GetComponent<Rigidbody>();
                frisbeeRb.AddForce((windVec) * windStrength / 2);
            }
        }
    }
}
