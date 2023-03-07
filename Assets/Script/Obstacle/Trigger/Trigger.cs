using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trigger : MonoBehaviour
{
    //トリガーの対象となるタグ
    protected string targetTag;

    //ターゲットとトリガーとの衝突位置
    protected Vector3 colPos;

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            colPos = other.ClosestPointOnBounds(this.transform.position);
            ActiveEvent();
        }
    }

    protected abstract void ActiveEvent();
}
