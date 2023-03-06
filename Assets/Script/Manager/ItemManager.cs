using System.Collections.Generic;
using UnityEngine;

//�w�������t���X�r�[���Ǘ�����X�N���v�g
public class ItemManager : MonoBehaviour
{
    public static ItemManager itemManager = null;

    //�A�C�e���̃f�B�N�V���i���[
    public static Dictionary<int, FrisbeeItem> items;

    //�t���X�r�[�̃v���n�u�i�ʂɃX�e�[�^�X���ӂ��Ă���j
    [SerializeField] private GameObject[] Frisbees = new GameObject[3];

    //�������������ǂ���
    static private bool isInitialized = false;

    private void Awake()
    {
        //������
        if (!isInitialized)
        {
            itemManager = this;

            items = new Dictionary<int, FrisbeeItem>();

            items.Add(0, new FrisbeeItem(0, 0, "NormalFrisbee", 0, Resources.Load<Sprite>("NormalFrisbeeSprite"),
                "�r�M�i�[�ނ��̃t���X�r�[�A���邭�Ă����₷��", Frisbees[0], true));

            items.Add(1, new FrisbeeItem(1, 1, "MetalFrisbee", 2100, Resources.Load<Sprite>("MetalFrisbeeSprite"),
              "�ƂĂ��I�����t���X�r�[�A�J�[�ɂӂ��Ƃ΂���ɂ���", Frisbees[1], false));

            items.Add(2, new FrisbeeItem(2, 2, "�N�i�C�t���X�r�[", 4100, Resources.Load<Sprite>("KunaiSprite"), "�j���W���̃t���X�r�[�BA�L�[�ŃX�K�^���P����", Frisbees[2], false));

            isInitialized = true;
        }
    }

    //�A�C�e�����������Ă��邩�ǂ���
    //�������Ă���Ȃ�true,�����łȂ��Ȃ�false
    //num�͓o�^���̃A�C�e���ԍ���Ή�����
    //0�Ȃ�normalFrisbee
    public bool GetItemFlag(int num)
    {
        return GetItem(num).Obtain;
    }

    //�A�C�e�����Q�b�g�����Ƃ��ɏ����t���O�����낷
    //num�͓o�^���̃A�C�e���ԍ���Ή�����
    //0�Ȃ�normalFrisbee
    public void ActiveItemFlag(int num)
    {
        GetItem(num).Obtain = true;
    }

    //�A�C�e�����f�B�N�V���i���ɑ��݂��邩�ǂ���
    public bool ExistItem(int key)
    {
        //�����Ɠ����L�[�����݂��邩
        if (items.ContainsKey(key))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //�w�肳�ꂽ�ԍ��̃A�C�e����Ԃ�
    public FrisbeeItem GetItem(int key)
    {
        //���݂��邩���ׂ�
        if (ExistItem(key))
        {
            return items[key];
        }
        else
        {
            Debug.Log("���݂��܂���");
            return null;
        }
    }
}
