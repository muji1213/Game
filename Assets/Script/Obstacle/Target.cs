using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [Header("������ԕ���")] [SerializeField] private Vector3 blowVec;
   
    private StageManager stageManager;
    private Rigidbody rb;
   
    // Start is called before the first frame update
    void Start()
    {
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
      
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Frisbee")
        {
           //�t�B�j�b�V�����o
            StartCoroutine(stageManager.Finish());
           
            //������΂�
            rb.AddForce(blowVec * 12);

            //�N���A�����ɂ���
            stageManager.isCleared = true;
        }
    }
}

