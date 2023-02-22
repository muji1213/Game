using UnityEngine;

public class FadeObject : MonoBehaviour
{
    [SerializeField] [Header("透過が始まる距離")] float startDistance = 10;
    [SerializeField] [Header("完全に透過される距離")] float hiddenDisanta = 2;

    MeshRenderer meshRenderer;

    private void Start()
    {
        //メッシュを取得
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        //カメラと対象の距離を取得
        var d = Vector3.Distance(Camera.main.transform.position, transform.position);

        //dが完全に透過する距離以下ならアルファを０に
        if (d <= hiddenDisanta)
        {
            meshRenderer.material.SetFloat("_Alpha", 0.0f);
        }

        //dがhiddenDista以上、startDistance以下なら0~1.0の間で変わる
        else if (d <= startDistance)
        {
            float c = (d - hiddenDisanta) / (startDistance - hiddenDisanta);
            meshRenderer.material.SetFloat("_Alpha", c);
        }
        //dがstartDistance以上ならアルファを１に
        else
        {
            meshRenderer.material.SetFloat("_Alpha", 1.0f);
        }
    }
}
