using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFrisbeeEnter : MonoBehaviour
{
    private bool isOn = false;

    public bool CheckEnterFrisbee()
    {
        return isOn;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Frisbee") || other.CompareTag("Player"))
        {
            GameObject.Find("Main Camera").GetComponent<CameraFollower>().Shake(0.2f, 0.3f);
            isOn = true;
        }
    }
}
