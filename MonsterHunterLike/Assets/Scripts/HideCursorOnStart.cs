using UnityEngine;

public class HideCursorOnStart : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = false;  // �}�E�X�J�[�\�����\���ɂ���
        Cursor.lockState = CursorLockMode.Locked;  // �J�[�\���̈ʒu���Œ�i�K�v�ɉ����āj
    }
}
