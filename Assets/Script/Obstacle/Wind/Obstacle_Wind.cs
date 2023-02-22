using UnityEngine;

//障害物（風）のスクリプト
//レベルは100にすること
public class Obstacle_Wind : UnCollisionable_Obstacle
{
    [Header("かぜ向き")] [SerializeField] private Vector3 windVec;
    [Header("風の強さ")] [SerializeField] private float windStrength;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume *= (SEManager.seManager.SeVolume / 1.0f);
    }

    //フリスビーが範囲内にいるとき
    private void OnTriggerStay(Collider other)
    {
        //フリスビーかどうか判定
        if (other.CompareTag("Frisbee"))
        {
            if (other.GetComponent<Frisbee>().isBlowed)
            {
                //一定方向に力をかけ続ける
                Rigidbody frisbeeRb = other.GetComponent<Rigidbody>();
                frisbeeRb.AddForce((windVec) * windStrength);
            }
            else
            {
                //一定方向に力をかけ続ける
                Rigidbody frisbeeRb = other.GetComponent<Rigidbody>();
                frisbeeRb.AddForce((windVec) * windStrength / 2);
            }
        }
    }
}
