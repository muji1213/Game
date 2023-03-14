using UnityEngine;


//���C�t��UI�̃X�N���v�g�ł�
public class LifeUI : MonoBehaviour
{
    [Header("���C�t�̃v���n�u")] [SerializeField] GameObject lifePrefab;
    [Header("�p�l��")] [SerializeField] GameObject lifePanel;

    //���݃��C�t
    private int currentLife = 0;

    private void Awake()
    {
        this.gameObject.SetActive(false);
    }

    //�ŏ�HP�ݒ�
    public void SetInitialLife(int HP)
    {
        //����HP���擾
        currentLife = HP;

        //HP�������A�n�[�g��UI�𐶐�����
        for (int i = 0; i < currentLife; i++)
        {
            Debug.Log("Life:" + HP);
            //�v���n�u���C���X�^���V�G�C�g
            GameObject life = Instantiate(lifePrefab);
            life.transform.SetParent(lifePanel.transform);

            //�ʒu��傫����ݒ�
            life.transform.localPosition = new Vector3(0, 0, 0);
            life.transform.localRotation = new Quaternion(0, 0, 0, 0);
            life.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    //���C�t������
    public void DestroyLife()
    {
        currentLife -= 1;

        //0�����ɂȂ�Ȃ�
        if (currentLife < 0)
        {
            return;
        }
        //0�ȏ�Ȃ�A�n�[�g�������
        else
        {
            Destroy(lifePanel.transform.GetChild(currentLife).gameObject);
        }

    }
}
