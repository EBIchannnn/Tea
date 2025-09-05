using UnityEngine;

public class HideCanvas : MonoBehaviour
{
    // 対象のキャンバスをインスペクタで割り当て
    [SerializeField] private Canvas targetCanvas;

    // キャンバスを表示
    public void Show()
    {
        if (targetCanvas != null)
        {
            targetCanvas.gameObject.SetActive(true);
            CameraController.IsUIActive = true;
        }
    }

    // ボタンにこのメソッドを割り当てる
    public void Hide()
    {
        if (targetCanvas != null)
        {
            targetCanvas.gameObject.SetActive(false);
            CameraController.IsUIActive = false;
        }
    }
}