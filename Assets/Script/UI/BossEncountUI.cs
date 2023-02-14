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
        //���ʂ�����Ȃ����߁ASE�}�l�[�W���̊������������A�K�����ʂɂ���
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
            //�A�j���[�V�����Đ����I��������A�N�e�B�u��
            this.gameObject.SetActive(false);
        }
    }
}
