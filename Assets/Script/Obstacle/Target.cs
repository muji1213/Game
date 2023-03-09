using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [Header("‚Á”ò‚Ô•ûŒü")] [SerializeField] private Vector3 blowVec;
   
    private Rigidbody rb;
   
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Frisbee")
        {     
            //‚Á”ò‚Î‚·
            rb.AddForce(blowVec * 12);
        }
    }
}

