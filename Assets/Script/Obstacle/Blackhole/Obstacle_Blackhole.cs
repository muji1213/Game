using UnityEngine;

public class Obstacle_Blackhole : UnCollisionable_Obstacle
{
    [Header("�����񂹂̋���")] [SerializeField] float force;

    private Rigidbody frisbeeRb;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume *= (SEManager.I.seVol);
    }

    private void OnTriggerStay(Collider other)
    {
        //�t���X�r�[�������񂹔���ɓ����Ă���ꍇ
        if (other.CompareTag("Frisbee"))
        {
            //���W�b�g�{�f�B���擾
            frisbeeRb = other.gameObject.GetComponent<Rigidbody>();

            //�t���X�r�[���玩�g�ɑ΂��Ẵx�N�g������
            Vector3 direction = (this.transform.position - other.gameObject.transform.position).normalized;

            //�����񂹂�
            frisbeeRb.AddForce(direction * force);
        }
    }
}
