using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEncountUI : MonoBehaviour
{
    [Header("SE")] [SerializeField] AudioClip SE;
    [Header("音量")] [SerializeField] [Range(0, 1)] float SEVol = 1;

    private Animator anim;

    void Awake()
    {
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        //SE
        SEManager.seManager.PlaySE(SEVol, SE);
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            //アニメーション再生が終わったら非アクティブに
            this.gameObject.SetActive(false);
        }
    }
}
