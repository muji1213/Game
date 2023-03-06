using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    //消滅時間
    protected float destroyTime;

    //宇宙船のスクリプト
    protected Enemy_AirShip airShip;

    protected void Start()
    {
        Destroy(this.gameObject, destroyTime);
    }

    protected void FixedUpdate()
    {
        //宇宙船が終了地点までついた時点で、弾を消滅させる
        if(airShip.GetState() == Enemy_AirShip.State.Dead)
        {
            Destroy(this.gameObject);
        }
    }

    //宇宙船のスクリプト
    public Enemy_AirShip AirShip
    {
        set
        {
            airShip = value;
        }
    }

    //消滅時間
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
