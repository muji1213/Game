using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [Header("吹っ飛ぶ方向")] [SerializeField] private Vector3 blowVec;
   
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
           //フィニッシュ演出
            StartCoroutine(stageManager.Finish());
           
            //吹っ飛ばす
            rb.AddForce(blowVec * 12);

            //クリア扱いにする
            stageManager.isCleared = true;
        }
    }
}

