using UnityEngine;

public class BossTrigger : Trigger
{
    [SerializeField] [Header("出現させるボス")] GameObject boss;
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
        //ボスを出現させ、UIを出す
        boss.SetActive(true);
        bossEncountUI.SetActive(true);
        isOn = true;
    }
}

