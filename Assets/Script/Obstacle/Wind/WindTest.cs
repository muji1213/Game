using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTest : MonoBehaviour
{
    [Header("かぜ向き")] [SerializeField] private Vector3 windVec;
    [Header("風の強さ")] [SerializeField] private float windStrength;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Frisbee"))
        {
            //一定方向に力をかけ続ける
            Rigidbody frisbeeRb = other.GetComponent<Rigidbody>();
            frisbeeRb.AddForce((windVec) * windStrength);
        }
       
    }
}
