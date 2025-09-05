using UnityEngine;

public class HideCanvas : MonoBehaviour
{
    // 対象のキャンバスをインスペクタで割り当て
    [SerializeField] private Canvas targetCanvas;

    // ボタンにこのメソッドを割り当てる
    public void Hide()
    {
        if (targetCanvas != null)
        {
            targetCanvas.gameObject.SetActive(false);
        }
    }
}