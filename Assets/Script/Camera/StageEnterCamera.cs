using UnityEngine;
using Cinemachine;

public class StageEnterCamera : MonoBehaviour
{
    private CinemachineDollyCart dollyCart;
    [Header("パス")] [SerializeField] private CinemachineSmoothPath path;
    [Header("カメラの速さ")] [SerializeField] [Range(0, 2)] private float speed;

    //ムービーが終わったか
    private bool isFinished = false;

    //ムービーを開始するかどうか
    private bool isStart = false;

    //プレイヤーを追従するスクリプト(初回入場時のみ無効にしておく)
    private CameraFollower cameraFollower;

    //ムービーを切り上げる値(Lerpのためpositionが0にならない)
    [Header("パスを切り上げる距離")] [SerializeField] [Range(0.1f, 10)] private float per;

    private void Awake()
    {
        cameraFollower = GetComponent<CameraFollower>();
        cameraFollower.enabled = false;

        dollyCart = this.GetComponent<CinemachineDollyCart>();

        //初回入場済みか調べる
        if (StageSelectManager.I.isStageEntered(GameManager.I.SelectedStageInfo.StageNum))
        {
            //入場済みならすぐに開始する
            dollyCart.m_Position = 0;

            //パスも設定しない
            dollyCart.m_Path = null;

            //終了判定にする
            isFinished = true;
        }
        else
        {
            //そうでないならムービーを流す
            dollyCart.m_Position = path.PathLength;
        }
    }

    void Update()
    {
        //スタートフラグが降りてないなら何もしない
        if (!isStart)
        {
            return;
        }

        //パスの一定割合まで進んだら切り上げる(Lerpは0にならない)
        if (dollyCart.m_Position <= per)
        {
            dollyCart.m_Path = null;

            //終了フラグを下す
            isFinished = true;

            //初回入場フラグをおろす
            StageSelectManager.I.ActiveStageEnteredFlag(GameManager.I.SelectedStageInfo.StageNum);
            return;
        }
        else if (dollyCart.m_Position <= path.PathLength)
        {
            //パスを辿る（徐々に速度を落とす）
            dollyCart.m_Position = Mathf.Lerp(dollyCart.m_Position, 0, speed * Time.deltaTime);
        }
    }

    //ムービーを開始する
    public void StartMovie()
    {
        if (isStart)
        {
            return;
        }
        isStart = true;
    }


    //終了を通知する
    public bool MovieFinished()
    {
        return isFinished;
    }
}
