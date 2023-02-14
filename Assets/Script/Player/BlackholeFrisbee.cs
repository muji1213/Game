using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeFrisbee : Frisbee
{
    //�Փ˂�����Q�������ł�����
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Destroy(collision.gameObject);
        }
    }

    protected override void Dodge()
    {
        return;
    }
}
