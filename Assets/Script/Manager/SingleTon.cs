using UnityEngine;

//シングルトンのジェネリッククラス
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
    /// 派生クラスの初期化
    /// </summary>
    protected virtual void Init()
    {

    }
}
