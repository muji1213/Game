using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEndTrigger : MonoBehaviour
{
    //�{�X���o�p
    [SerializeField] [Header("�Ǔ˂���f��")] GameObject planet;
    [SerializeField] [Header("������΂�����")] private float pow;
    [SerializeField] [Header("������")] private AudioClip explosion;
    [SerializeField] [Header("�����̃G�t�F�N�g")] GameObject burstEff;
    [SerializeField] [Header("���C���J����")] CameraFollower cameraFollower;

    private AudioSource audioSource;

    private void Start()
    {
        //����
        audioSource = GetComponent<AudioSource>();
        audioSource.volume *= (SEManager.seManager.SeVolume / 1.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        //�{�X��������
        if (other.CompareTag("Enemy"))
        {
            //�Փˈʒu�����m
            Vector3 pos = other.ClosestPointOnBounds(this.transform.position);

            //�Փˈʒu�ɔ����̃G�t�F�N�g
            GameObject burst = Instantiate(burstEff, pos, Quaternion.identity);
            Destroy(burst, burst.GetComponent<ParticleSystem>().main.duration);

            //�{�X����Փ˂����f���Ɍ����Ẵx�N�g��
            Vector3 vec = planet.transform.position - pos;

            //�f���𐁂���΂�
            planet.GetComponent<Rigidbody>().AddForce(vec * pow, ForceMode.VelocityChange);
            planet.GetComponent<Rigidbody>().AddTorque(vec * pow, ForceMode.VelocityChange);
            Destroy(planet, 5.0f);

            //�J������U��
            cameraFollower.Shake(0.5f, 1.0f);

            //������
            audioSource.PlayOneShot(explosion);
        }
    }
}
