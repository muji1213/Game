using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEndTrigger : Trigger
{
    //�{�X���o�p
    [SerializeField] [Header("�Ǔ˂���f��")] GameObject planet;
    [SerializeField] [Header("������΂�����")] private float pow;
    [SerializeField] [Header("������")] private AudioClip explosion;
    [SerializeField] [Header("�������̉���")] [Range(0, 1)] float explosionSEVol = 1;
    [SerializeField] [Header("�����̃G�t�F�N�g")] GameObject burstEff;
    [SerializeField] [Header("���C���J����")] CameraFollower cameraFollower;

    private void Start()
    {
        targetTag = "Enemy";
    }

    protected override void ActiveEvent()
    {
        //�Փˈʒu�ɔ����̃G�t�F�N�g
        GameObject burst = Instantiate(burstEff, colPos, Quaternion.identity);
        Destroy(burst, burst.GetComponent<ParticleSystem>().main.duration);

        //�{�X����Փ˂����f���Ɍ����Ẵx�N�g��
        Vector3 vec = planet.transform.position - colPos;

        //�f���𐁂���΂�
        planet.GetComponent<Rigidbody>().AddForce(vec * pow, ForceMode.VelocityChange);
        planet.GetComponent<Rigidbody>().AddTorque(vec * pow, ForceMode.VelocityChange);
        Destroy(planet, 5.0f);

        //�J������U��
        cameraFollower.Shake(0.5f, 1.0f);

        //������
        SEManager.I.PlaySE(explosionSEVol, explosion);
    }
}
