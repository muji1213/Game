using UnityEngine;

public class Obstacle_Planet : Collisionable_Obstacle, IMovable
{
    [Header("動く速さ")] [SerializeField] float speed;
    [Header("動く向き")] [SerializeField] Vector3 direction;
    [Header("回転方向")] [SerializeField] Vector3 rotateDirection;
    [Header("回転スピード")] [SerializeField] float rotateSpeed;

    [Header("フリスビートリガー")] [SerializeField] CheckFrisbeeEnter checkFrisbeeEnter;

    //トリガーで動くタイプのフラグ
    private bool isMove = false;

    //動かないタイプ
    private bool isStop = false;

    private void Update()
    {
        if (isMove)
        {
            Destroy(this.gameObject, 8);
        }
    }

    public void Move()
    {
        this.transform.position += direction * speed;
        this.transform.Rotate(rotateDirection * rotateSpeed);
    }

    private void FixedUpdate()
    {
        //インスペクタでトリガーが設定されているなら
        if (checkFrisbeeEnter != null)
        {
            //トリガーをフリスビーが通過したら動く
            isMove = checkFrisbeeEnter.CheckEnterFrisbee();
        }
        else
        {
            isStop = true;
        }


        if (isMove || isStop)
        {
            Move();
        }
    }
}
