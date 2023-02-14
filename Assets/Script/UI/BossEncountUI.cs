using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEncountUI : MonoBehaviour
{
    [Header("SE")] [SerializeField] AudioClip SE;

    AudioSource audioSource;

    private Animator anim;

    void Awake()
    {    
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        //音量が合わないため、SEマネージャの割合分をかけ、適性音量にする
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        audioSource.volume *= (SEManager.seManager.SeVolume / 1.0f);

        //SE
        audioSource.PlayOneShot(SE);
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
