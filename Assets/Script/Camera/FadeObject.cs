using UnityEngine;

public class FadeObject : MonoBehaviour
{
    [SerializeField] [Header("���߂��n�܂鋗��")] float startDistance = 10;
    [SerializeField] [Header("���S�ɓ��߂���鋗��")] float hiddenDisanta = 2;

    MeshRenderer meshRenderer;

    private void Start()
    {
        //���b�V�����擾
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        //�J�����ƑΏۂ̋������擾
        var d = Vector3.Distance(Camera.main.transform.position, transform.position);

        //d�����S�ɓ��߂��鋗���ȉ��Ȃ�A���t�@���O��
        if (d <= hiddenDisanta)
        {
            meshRenderer.material.SetFloat("_Alpha", 0.0f);
        }

        //d��hiddenDista�ȏ�AstartDistance�ȉ��Ȃ�0~1.0�̊Ԃŕς��
        else if (d <= startDistance)
        {
            float c = (d - hiddenDisanta) / (startDistance - hiddenDisanta);
            meshRenderer.material.SetFloat("_Alpha", c);
        }
        //d��startDistance�ȏ�Ȃ�A���t�@���P��
        else
        {
            meshRenderer.material.SetFloat("_Alpha", 1.0f);
        }
    }
}
