using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    //消滅時間
    [SerializeField][Header("出現から消滅するまでの時間は")]protected int destroyTime;

    protected void Start()
    {
        Destroy(this.gameObject, destroyTime);
    }

    public int DestroyTime
    {
        set
        {
            destroyTime = value;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Frisbee"))
        {
            Debug.Log("HIt");
        }
    }
}
