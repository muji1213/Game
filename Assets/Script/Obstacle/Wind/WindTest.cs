using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTest : MonoBehaviour
{
    [Header("‚©‚ºŒü‚«")] [SerializeField] private Vector3 windVec;
    [Header("•—‚Ì‹­‚³")] [SerializeField] private float windStrength;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Frisbee"))
        {
            //ˆê’è•ûŒü‚É—Í‚ð‚©‚¯‘±‚¯‚é
            Rigidbody frisbeeRb = other.GetComponent<Rigidbody>();
            frisbeeRb.AddForce((windVec) * windStrength);
        }
       
    }
}
