using UnityEngine;

//障害物（木）のスクリプト
public class Obstacle_Tree : Collisionable_Obstacle, IMovable
{
    [Header("倒壊する速さ")] [SerializeField] private float rotateSpeed;
    [Header("倒壊する方向")] [SerializeField] private Vector3 rotateVec;
    [Header("プレイヤー感知のスクリプト")] [SerializeField] private CheckFrisbeeEnter checkFrisbeeEnter;

    [Header("倒壊時のSE")] [SerializeField] AudioClip rotateSE;

    //消滅させるフラグ
    private bool isDestroy = false;

    //SEのフラグ
    private bool isSounded = false;

    private void FixedUpdate()
    {
        if (isDestroy)
        {
            //消滅させないと回転し続けるため消す
            Destroy(this.gameObject, 7.0f);
        }
        Move();
    }

    //フリスビーが特定の位置に接触したら
    public void Move()
    {
        if (checkFrisbeeEnter.CheckEnterFrisbee())
        {
            //回転させる
            this.transform.Rotate(rotateVec * rotateSpeed);

            //消滅させるフラグを立てる
            isDestroy = true;

            if (!isSounded)
            {
                SEManager.seManager.PlaySe(rotateSE);
                isSounded = true;
            }
            else
            {
                return;
            }
        }
    }
}
