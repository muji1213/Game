using System.Collections;
using UnityEngine;

//カメラ追従用のスクリプトです
//このスクリプトはカメラの追従、および対象の切り替え、画面振動、カメラの切り替えを行う
public class CameraFollower : MonoBehaviour
{
    private enum State
    {
        RunPhase,
        FrisbeePhase,
        Stop
    }

    //ステート
    private State state;

    //振動中かどうか
    [HideInInspector]public bool isShake = false;

    //メインカメラ
    [SerializeField] GameObject MainCamera;

    //フリスビーがゴールに衝突した際に切り替えるカメラ
    [SerializeField] GameObject finishCamera;

    //ゴールのゲームオブジェクト
    [SerializeField]private GameObject player;

    //対象に近づくまでの時間
    [SerializeField, Range(0.01f, 1.0f)] private float _positionLerpSpeed = 0.1f;

    //指定の角度まで回る速度
    [SerializeField, Range(0.1f, 1.0f)] private float _rotationLerpSpeed = 0.1f;

    //カメラの位置調整
    //対象のどれくらいまえにつくか
    [SerializeField] private float _distanceForwards = 5f;

    //対象のどれくらい上につくか
    [SerializeField] private float _distanceUpwards = 1f;

    //カメラの向く方向
    [SerializeField] private Vector3 LookTo;

    private void Start()
    {
        state = State.RunPhase;
        //メインカメラはOn
        MainCamera.SetActive(true);
    }

    private void Update()
    {
        //フリスビーが開始位置についたときにカメラを固定する
        if (state == State.Stop)
        {
            return;
        }

        //プレイヤーが穴に落ちた際、もしくはフリスビーが投げられた際にカメラの位置を移動させる
        if (state == State.RunPhase)
        {
            //対象の追いつく速度
            float posSpeed = _positionLerpSpeed;

            //対象の位置への移動
            //posTo：追従の対象
            Vector3 posTo = player.transform.position + transform.up * _distanceUpwards + transform.forward * _distanceForwards;

            //現在位置からposToへ近づく
            transform.position = Vector3.Slerp(transform.position, posTo, posSpeed);

            //カメラの回転
            float rotSpeed = _rotationLerpSpeed;

            //カメラを回転させる
            //LookToはほかのクラスから呼び出される際に引数として渡される
            Quaternion rotTo = Quaternion.LookRotation(LookTo);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotTo, rotSpeed);
        }
        else
        {
            //フリスビーが開始位置についたら位置を固定する
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, player.transform.position.z + _distanceForwards - 3);
        }
    }

    //カメラの追従対象をかえる
    //最初はUnityちゃんに追従し、フリスビーを投げた瞬間に、フリスビーにターゲットを変更
    public void ChangeTarget(GameObject newTarget)
    {
        player = newTarget;
    }

    //カメラの位置を対象へ移動する
    //distansForward:対象からどれくらいまえか
    //distansUpward:対象からどれくらい上か
    //カメラの向く方向
    public void ZoomToTarget(float distansForward, float distanceUpward, Vector3 LookTo)
    {
        this._distanceForwards = distansForward;
        this._distanceUpwards = distanceUpward;
        this.LookTo = LookTo;
    }

    //フリスビーが開始位置についたらカメラを固定する
    public void FixCamera(GameObject StartPos)
    {
        this.transform.position = new Vector3(StartPos.transform.position.x, StartPos.transform.position.y,
                                                player.transform.position.z + _distanceForwards - 3);

        //開始ステートに
        state = State.FrisbeePhase;
    }

    //フリスビーが死亡時、追従を停止させる（フリスビーが下へ落ちていくため）
    public void StopCamera()
    {
        state = State.Stop;
    }

    //カメラ振動メソッド
    //duration: 振動時間
    //magnitude: 振動の大きさ
    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(TheShake(duration, magnitude));
    }

    private IEnumerator TheShake(float duration, float magnitude)
    {
        //振動開始地点を記録
        Vector3 pos = transform.localPosition;
        isShake = true;

        float elapsed = 0f; // 経過時間

        //振動時間が経過時間以下であれば
        while (elapsed < duration)
        {
            //xyそれぞれランダムに振動
            //1-(elapsed / duration)で時間経過するほど振動が小さくなる
            var x = pos.x + Random.Range(-2f, 2f) * magnitude * (1 - (elapsed / duration));
            var y = pos.y + Random.Range(-2f, 2f) * magnitude * (1 - (elapsed / duration));

            //それぞれ位置を適用
            transform.localPosition = new Vector3(x, y, player.transform.position.z + _distanceForwards - 3);
            elapsed += Time.deltaTime;

            yield return new WaitForSeconds(Time.deltaTime / 60);
        }

        //振動が終了したら、カメラ位置を元に戻す
        isShake = false;
        transform.localPosition = pos;
    }

    //フリスビーがゴールに衝突したらフィニッシュカメラへ切り替える（フィニッシュカメラの方が深度が高い）
    public void ChangeFinishCamera()
    {
        finishCamera.SetActive(true);
    }

    //メインカメラに戻す
    public void ChangeNormalCamera()
    {
        finishCamera.SetActive(false);
    }
}
