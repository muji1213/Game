using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    //消滅時間
    private float destroyTime;

    //宇宙船のスクリプト
    private Enemy_AirShip airShip;

    protected void Start()
    {
        Init();
        Destroy(this.gameObject, DestroyTime);
    }

    protected void FixedUpdate()
    {
        //宇宙船が終了地点までついた時点で、弾を消滅させる
        if(AirShip.GetState() == Enemy_AirShip.State.End)
        {
            Destroy(this.gameObject);
        }
    }

    //継承先の初期化
    protected virtual void Init()
    {

    }

    //宇宙船のスクリプト
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

    //消滅時間
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
