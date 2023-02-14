using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//このスクリプトはフリスビーがゴールに衝突した際に使うカメラのスクリプトです
public class FinishCamera : MonoBehaviour
{
    //ゴールのゲームオブジェクト
    [SerializeField] private GameObject target;

    //カメラの回転速度
    [SerializeField, Range(0.1f, 1.0f)] private float _rotationLerpSpeed = 0.1f;

    //このカメラ
    private Camera finishCamera;

    private void Start()
    {
        finishCamera = this.GetComponent<Camera>();
        this.gameObject.SetActive(false);
    }
    private void Update()
    {
        float rotSpeed = _rotationLerpSpeed;

        //常にゴールのゲームオブジェクトの方を向くようにする
        Quaternion rotTo = Quaternion.LookRotation(target.transform.position - this.transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotTo, rotSpeed);
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

        float elapsed = 0f; // 経過時間

        //振動時間が経過時間以下であれば
        while (elapsed < duration)
        {
            //xyそれぞれランダムに振動
            //1-(elapsed / duration)で時間経過するほど振動が小さくなる
            var x = pos.x + Random.Range(-2f, 2f) * magnitude * (1 - (elapsed / duration));
            var y = pos.y + Random.Range(-2f, 2f) * magnitude * (1 - (elapsed / duration));

            //それぞれ位置を適用
            transform.localPosition = new Vector3(x, y, pos.z);
            elapsed += Time.unscaledDeltaTime;

            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }

        //振動が終了したら、カメラ位置を元に戻す
        transform.localPosition = pos;

        //振動終了後、描画対象を増加させる
        ChangeCallingMask();
    }

    // フィニッシュ演出では、衝突直後、専用背景、およびゴールのゲームオブジェクトしか映らない
    // フィニッシュ演出終了後に専用背景を非表示、ほかのオブジェクトを表示する
    private void ChangeCallingMask()
    {
        //レイヤー12はエフェクト
        //レイヤー14はフィニッシュレイヤーです

        //全てのレイヤーを表示
        finishCamera.cullingMask = -1;

        //エフェクトレイヤーの非表示
        finishCamera.cullingMask &= ~(1 << 12);

        //フィニッシュレイヤーの非表示
        finishCamera.cullingMask &= ~(1 << 14);
    }
}
