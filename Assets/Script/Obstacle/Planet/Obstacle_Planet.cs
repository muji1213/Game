using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Planet : Collisionable_Obstacle, IRoatatable
{
    [Header("回転方向")] [SerializeField] Vector3 rotateDirection;
    [Header("回転スピード")] [SerializeField] float rotateSpeed;

    public void Rotate()
    {
        this.transform.Rotate(rotateDirection * rotateSpeed);
    }

    private void FixedUpdate()
    {

    }
}
