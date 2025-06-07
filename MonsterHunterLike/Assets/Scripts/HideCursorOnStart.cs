using UnityEngine;

public class HideCursorOnStart : MonoBehaviour
{
    void Awake()
    {
        //�}�E�X�J�[�\�����\���ɂ���
        Cursor.visible = false;

        //�J�[�\���̈ʒu���Œ�i�K�v�ɉ����āj
        Cursor.lockState = CursorLockMode.Locked;
    }
}
