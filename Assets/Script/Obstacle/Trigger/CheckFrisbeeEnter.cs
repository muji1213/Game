using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFrisbeeEnter : Trigger
{
    [Header("���C���J����")] [SerializeField] Camera mainCamera;

    //�J�����t�H�����[
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

    //�t���X�r�[���N��������
   
    protected override void ActiveEvent()
    {
        //�U��������
        cameraFollower.Shake(0.2f, 0.3f);
        isOn = true;
    }
}
