using UnityEngine;

//�V���O���g���̃W�F�l���b�N�N���X
public class SingleTon<T> : MonoBehaviour where T: SingleTon<T>
{
    public static T I = null;
    
    private void Awake()
    {
        if(I == null)
        {
            I = this as T;
            I.Init();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// �h���N���X�̏�����
    /// </summary>
    protected virtual void Init()
    {

    }
}
