using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    //���Ŏ���
    [SerializeField][Header("�o��������ł���܂ł̎��Ԃ�")]protected int destroyTime;

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
