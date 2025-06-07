using UnityEngine;
using UnityEngine.InputSystem;

public class MonsterHunterCamera : MonoBehaviour
{
    [SerializeField]
    private Transform _cameraTarget;         // �v���C���[
    [SerializeField]
    private Transform cameraTransform; // �J����

    [SerializeField]
    private float distance = 5f;       // �J��������
    [SerializeField]
    private float mouseSensitivity = 3f;
    [SerializeField]
    private float pitchMin = -20f;
    [SerializeField]
    private float pitchMax = 60f;

    [SerializeField]
    private float smoothSpeed = 5f;   // �J�����Ǐ]���x
    [SerializeField]
    private LayerMask obstacleMask;   // ��Q�����背�C���[

    private float yaw = 0f;
    private float pitch = 10f;

    private Vector3 currentVelocity;

    void LateUpdate()
    {
        // �}�E�X���͂��擾
        Vector2 lookInput = InputSystem.actions["Look"].ReadValue<Vector2>();

        // 1�t���[���̓��͒l�̏���𐧌�
        lookInput = Vector2.ClampMagnitude(lookInput, 50f); 

        // �}�E�X���x���|���Ċp�x�X�V
        yaw += lookInput.x * mouseSensitivity * Time.deltaTime;
        pitch -= lookInput.y * mouseSensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        // �J�����̗��z�ʒu���v�Z
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 idealPos = _cameraTarget.position + Vector3.up * 1.5f + rotation * new Vector3(0, 0, -distance);

        // ��Q���ɂԂ����Ă���΋������k�߂�
        RaycastHit hit;
        if (Physics.Linecast(_cameraTarget.position + Vector3.up * 1.5f, idealPos, out hit, obstacleMask))
        {
            idealPos = hit.point;
        }

        // �J���������炩�Ɉړ�������
        cameraTransform.position = Vector3.SmoothDamp(cameraTransform.position, idealPos, ref currentVelocity, 1f / smoothSpeed);

        // �v���C���[�𒍎�
        cameraTransform.LookAt(_cameraTarget.position + Vector3.up * 1.5f);
    }
}
