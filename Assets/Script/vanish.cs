using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vanish : MonoBehaviour
{
    public bool isFlash = false;

    public float interval;
    private Renderer render;
    private float timer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFlash)
        {
            timer += Time.deltaTime;
            if(timer > interval)
            {
                render.enabled = !render.enabled;
                timer = 0.0f;
            }
        }
    }
}
