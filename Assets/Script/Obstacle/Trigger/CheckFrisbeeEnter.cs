using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFrisbeeEnter : Trigger
{
    private bool isOn = false;

    private void Start()
    {
        targetTag = "Frisbee";
    }

    public bool CheckEnterFrisbee()
    {
        return isOn;
    }

    //フリスビーが侵入したら
   
    protected override void ActiveEvent()
    {
        //振動させる
        GameObject.Find("Main Camera").GetComponent<CameraFollower>().Shake(0.2f, 0.3f);
        isOn = true;
    }
}
