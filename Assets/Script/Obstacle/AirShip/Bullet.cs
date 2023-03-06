using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    //���Ŏ���
    protected float destroyTime;

    //�F���D�̃X�N���v�g
    protected Enemy_AirShip airShip;

    protected void Start()
    {
        Destroy(this.gameObject, destroyTime);
    }

    protected void FixedUpdate()
    {
        //�F���D���I���n�_�܂ł������_�ŁA�e�����ł�����
        if(airShip.GetState() == Enemy_AirShip.State.Dead)
        {
            Destroy(this.gameObject);
        }
    }

    //�F���D�̃X�N���v�g
    public Enemy_AirShip AirShip
    {
        set
        {
            airShip = value;
        }
    }

    //���Ŏ���
    public float DestroyTime
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
