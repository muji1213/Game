using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFrisbeeEnter : Trigger
{
    [Header("メインカメラ")] [SerializeField] Camera mainCamera;

    //カメラフォロワー
    private CameraFollower cameraFollower;

    private bool isOn = false;

    private void Start()
    {
        cameraFollower = mainCamera.GetComponent<CameraFollower>();
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
        cameraFollower.Shake(0.2f, 0.3f);
        isOn = true;
    }
}
