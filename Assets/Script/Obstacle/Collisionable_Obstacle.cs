using UnityEngine;


//衝突するタイプの障害物のクラスです
public class Collisionable_Obstacle : MonoBehaviour
{
    //リジットボディ
    protected Rigidbody rb;

    //レンダラー
    protected MeshRenderer meshRenderer;

    //ステージマネージャー
    protected StageManager stageManager;

    //障害物のレベル
    //フリスビーのレベルが高い際、吹っ飛ばされる
    public int level;

    //赤テクスチャ(フリスビーのレベルのほうが高い時に使う)
    Texture2D redTex;

    protected void Start()
    {
        //コンポーネント取得
        rb = GetComponent<Rigidbody>();
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        meshRenderer = GetComponent<MeshRenderer>();

        //テクスチャ
        redTex = Resources.Load<Texture2D>("RedTex");

        //ゲーム開始時、フリスビーのレベルを確認し、自身のほうがレベルが低いなら赤
        //自身のほうが高いなら通常通り表示
        if (GameManager.I.SelectedFrisbeeInfo.FrisbeeLevel > level)
        {
            meshRenderer.material.SetTexture("_MainTex",redTex);
        }
        else
        {
            return;
        }
    }

    //フリスビーに激突された際の処理
    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Frisbee")
        {
            //フリスビーのレベルのほうが低いなら
            if (GameManager.I.SelectedFrisbeeInfo.FrisbeeLevel <= level)
            {
                //そのままなにもしない
                rb.constraints = RigidbodyConstraints.FreezeAll;
                return;
            }
            else
            {
                rb.mass = 0.0f;

                rb.constraints = RigidbodyConstraints.None;

                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z) * 10;

                //吹っ飛ばされた2秒後消滅させる
                Destroy(this.gameObject, 2.0f);

                this.gameObject.layer = 15;
            }
        }
    }
}
