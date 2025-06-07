using UnityEngine;

public class HideCursorOnStart : MonoBehaviour
{
    void Awake()
    {
        //マウスカーソルを非表示にする
        Cursor.visible = false;

        //カーソルの位置を固定（必要に応じて）
        Cursor.lockState = CursorLockMode.Locked;
    }
}
