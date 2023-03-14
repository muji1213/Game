using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    //���Ŏ���
    private float destroyTime;

    //�F���D�̃X�N���v�g
    private Enemy_AirShip airShip;

    protected void Start()
    {
        Init();
        Destroy(this.gameObject, DestroyTime);
    }

    protected void FixedUpdate()
    {
        //�F���D���I���n�_�܂ł������_�ŁA�e�����ł�����
        if(AirShip.GetState() == Enemy_AirShip.State.End)
        {
            Destroy(this.gameObject);
        }
    }

    //�p����̏�����
    protected virtual void Init()
    {

    }

    //�F���D�̃X�N���v�g
    public Enemy_AirShip AirShip
    {
        set
        {
            airShip = value;
        }
        get
        {
            return airShip;
        }
    }

    //���Ŏ���
    public float DestroyTime
    {
        set
        {
            destroyTime = value;
        }
        get
        {
            return destroyTime;
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
