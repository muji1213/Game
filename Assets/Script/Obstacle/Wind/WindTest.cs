using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTest : MonoBehaviour
{
    [Header("��������")] [SerializeField] private Vector3 windVec;
    [Header("���̋���")] [SerializeField] private float windStrength;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Frisbee"))
        {
            //�������ɗ͂�����������
            Rigidbody frisbeeRb = other.GetComponent<Rigidbody>();
            frisbeeRb.AddForce((windVec) * windStrength);
        }
       
    }
}
