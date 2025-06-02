using UnityEngine;

public class HideCursorOnStart : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = false;  // マウスカーソルを非表示にする
        Cursor.lockState = CursorLockMode.Locked;  // カーソルの位置を固定（必要に応じて）
    }
}
