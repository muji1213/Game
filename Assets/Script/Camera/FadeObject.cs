using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeObject : MonoBehaviour
{
    [SerializeField] [Header("���߂��n�܂鋗��")] float startDistance = 10;
    [SerializeField] [Header("���S�ɓ��߂���鋗��")] float hiddenDisanta = 2;

    MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        var d = Vector3.Distance(Camera.main.transform.position, transform.position);

      
        if (d <= hiddenDisanta)
        {
            meshRenderer.material.SetFloat("_Alpha", 0.0f);
        }

        else if (d <= startDistance)
        {
            float c = (d - hiddenDisanta) / (startDistance - hiddenDisanta);
            meshRenderer.material.SetFloat("_Alpha", c);
        }
        else
        {
            meshRenderer.material.SetFloat("_Alpha", 1.0f);
        }
    }
}
