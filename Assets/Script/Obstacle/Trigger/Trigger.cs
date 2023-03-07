using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trigger : MonoBehaviour
{
    //�g���K�[�̑ΏۂƂȂ�^�O
    protected string targetTag;

    //�^�[�Q�b�g�ƃg���K�[�Ƃ̏Փˈʒu
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
