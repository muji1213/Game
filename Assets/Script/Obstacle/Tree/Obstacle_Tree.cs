using UnityEngine;

//障害物（木）のスクリプト
public class Obstacle_Tree : Collisionable_Obstacle, IRoatatable
{
    [Header("倒壊する速さ")] [SerializeField] private float rotateSpeed;
    [Header("倒壊する方向")] [SerializeField] private Vector3 rotateVec;
    [Header("プレイヤー感知のスクリプト")] [SerializeField] private CheckFrisbeeEnter checkFrisbeeEnter;

    [Header("倒壊時のSE")] [SerializeField] AudioClip rotateSE;
    [Header("倒壊時のSEの音量")] [SerializeField] [Range(0, 1)] float rotateSEVol = 1;

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
        Rotate();
    }

    //フリスビーが特定の位置に接触したら
    public void Rotate()
    {
        if (checkFrisbeeEnter.CheckEnterFrisbee())
        {
            //回転させる
            this.transform.Rotate(rotateVec * rotateSpeed);

            //消滅させるフラグを立てる
            isDestroy = true;

            if (!isSounded)
            {
                SEManager.I.PlaySE(rotateSEVol, rotateSE);
                isSounded = true;
            }
            else
            {
                return;
            }
        }
    }
}
