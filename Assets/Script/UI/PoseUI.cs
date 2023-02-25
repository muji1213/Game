using UnityEngine;

//���̃X�N���v�g�̓|�[�YUI�ł�
public class PoseUI : SceneChanger
{
    //�X�e�[�W�}�l�[�W���[
    private StageManager stageManager;
    
    private void Start()
    {
        //�����ł͔�\��
        this.gameObject.SetActive(false);

        //�X�e�[�W�}�l�[�W���[���擾
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
    }

    //�R���e�j���[�{�^�����������ꍇ
    public void Continue()
    {
        //�ēx�i�s����
        stageManager.Pose();
    }
}
