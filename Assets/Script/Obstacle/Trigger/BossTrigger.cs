using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [SerializeField] [Header("�o��������{�X")] GameObject boss;
    [SerializeField] [Header("UI")] GameObject bossEncountUI;
    private bool isOn = false;

    public bool CheckEnterFrisbee()
    {
        return isOn;
    }

    //�t���X�r�[���N��������
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Frisbee"))
        {
            //�{�X���o�������AUI���o��
            boss.SetActive(true);
            bossEncountUI.SetActive(true);
            isOn = true;
        }
    }
}
