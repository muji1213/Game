using UnityEngine;

//このスクリプトはUnityちゃんの接地判定を取得する
public class GroundChecker : MonoBehaviour
{
    //地面についているかどうか
    private bool isGround = false;

    //地面のタグ
    private string groundTag = "Ground";

    //接地フラグ
    private bool isGroundEnter, isGroundStay, isGroundExit;

    public bool CheckGround()
    {
        //enterとstayがTrueの時だけ接地している判定になる
        if (isGroundEnter || isGroundStay)
        {
            isGround = true;
        }
        //Exitがtrueなら接地していないことになる
        else if (isGroundExit)
        {
            isGround = false;
        }

        isGroundEnter = false;
        isGroundStay = false;
        isGroundExit = false;
        return isGround;
    }

    //地面についたらEnterフラグをTrue
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(groundTag))
        {
            isGroundEnter = true;
        }
    }

    //地面にいる間StayフラグをTrue
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(groundTag))
        {
            isGroundStay = true;
        }
    }

    //地面を離れたらExitをTrue
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(groundTag))
        {
            isGroundExit = true;
        }
    }
}
