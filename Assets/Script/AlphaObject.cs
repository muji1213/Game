using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaObject : MonoBehaviour
{
    [SerializeField] Material material;
    [SerializeField] GameObject player;

    float alpha;
    public Texture2D texture;

    public bool Red;
    
    // Start is called before the first frame update
    void Start()
    {
       
        alpha = material.GetFloat("_Alpha");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(this.gameObject.transform.position, player.transform.position) < 1.0)
        {
            alpha = 0.3f;
            material.SetFloat("_Alpha", alpha);
        }
        else
        {
            alpha = 1.0f;
            material.SetFloat("_Alpha", alpha);
        }
        if (Red)
        {
            material.SetTexture("_MainTex", texture);
        }
    }
}
