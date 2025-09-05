using UnityEngine;

public class MainCamera : MonoBehaviour
{
    // 開始位置を指定
    public Vector3 startPosition = new Vector3(0, 5, -10);
    // 開始時の向きを指定（例：Quaternion.Euler(30, 0, 0) で少し下向き）
    public Vector3 startRotation = new Vector3(30, 0, 0);

    void Start()
    {
        // カメラを開始位置へ移動
        transform.position = startPosition;
        // カメラの向きを設定
        transform.rotation = Quaternion.Euler(startRotation);
    }
}