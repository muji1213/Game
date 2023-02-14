using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    //è¡ñ≈éûä‘
    [SerializeField][Header("èoåªÇ©ÇÁè¡ñ≈Ç∑ÇÈÇ‹Ç≈ÇÃéûä‘ÇÕ")]protected int destroyTime;

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
