using UnityEngine;

//���̃X�N���v�g��Unity�����̐ڒn������擾����
public class GroundChecker : MonoBehaviour
{
    //�n�ʂɂ��Ă��邩�ǂ���
    private bool isGround = false;

    //�n�ʂ̃^�O
    private string groundTag = "Ground";

    //�ڒn�t���O
    private bool isGroundEnter, isGroundStay, isGroundExit;

    public bool CheckGround()
    {
        //enter��stay��True�̎������ڒn���Ă��锻��ɂȂ�
        if (isGroundEnter || isGroundStay)
        {
            isGround = true;
        }
        else if (isGroundExit)
        {
            isGround = false;
        }

        isGroundEnter = false;
        isGroundStay = false;
        isGroundExit = false;
        return isGround;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(groundTag))
        {
            isGroundEnter = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(groundTag))
        {
            isGroundStay = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(groundTag))
        {
            isGroundExit = true;
        }
    }
}
