using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [SerializeField] [Header("èoåªÇ≥ÇπÇÈÉ{ÉX")] GameObject boss;
    [SerializeField] [Header("UI")] GameObject bossEncountUI;
    private bool isOn = false;

    public bool CheckEnterFrisbee()
    {
        return isOn;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Frisbee"))
        {
            boss.SetActive(true);
            bossEncountUI.SetActive(true);
            isOn = true;
        }
    }
}
