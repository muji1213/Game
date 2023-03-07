using UnityEngine;

public class BossTrigger : Trigger
{
    [SerializeField] [Header("�o��������{�X")] GameObject boss;
    [SerializeField] [Header("UI")] GameObject bossEncountUI;
    private bool isOn = false;

    private void Start()
    {
        targetTag = "Frisbee";
    }

    public bool CheckEnterFrisbee()
    {
        return isOn;
    }

    protected override void ActiveEvent()
    {
        //�{�X���o�������AUI���o��
        boss.SetActive(true);
        bossEncountUI.SetActive(true);
        isOn = true;
    }
}

