using UnityEngine;
using UnityEngine.InputSystem;

public class MonsterHunterCamera : MonoBehaviour
{
    [Header("�^�[�Q�b�g�ݒ�")]
    [SerializeField]
    private Transform _cameraTarget;
    [SerializeField]
    private Transform _cameraTransform;  //���ۂ̃J����Transform
    [SerializeField]

    [Header("�J���������ݒ�")]
    private float _defaultDistance = 5f;
    [SerializeField]
    private float _minDistance = 1.5f;  //�ڕW�ɍł��߂Â��鋗���iRaycast�Փˎ��̍ŏ������j
    private float _currentDistance; //���݂̎��ۂ̃J��������
    [SerializeField]
    private float _zoomOutSpeed = 2f; // �Y�[���A�E�g���x

    [Header("�}�E�X���x�ݒ�")]
    [SerializeField]
    private float mouseSensitivity = 3f;
    [SerializeField]
    private float _pitchMin = -20f; // �J�����̋p�̍ŏ��l�i�^�������j
    [SerializeField]
    private float _pitchMax = 60f;  // �J�����̋p�̍ő�l�i�^������j

    [Header("�J�����Ǐ]�ݒ�")]
    [SerializeField]
    private float _smoothSpeed = 5f;
    [SerializeField]
    private LayerMask _obstacleMask;    // Raycast�p�F�J�����Ƃ̊Ԃɂ����Q����Layer
    private float _yaw = 0f;    // �J�����̐��������̉�]�p�i���E�j
    private float _pitch = 10f; // �J�����̐��������̉�]�p�i�㉺�j
    private Vector3 _currentVelocity;   // SmoothDamp�Ŏg�p����錻�݂̑��x�i�����I�ȕ�ԏ�ԁj

    void Start()
    {
        _currentDistance = _defaultDistance;
    }

    void LateUpdate()
    {
        // �J�����̉�]�ƈʒu���X�V
        UpdateCameraRotation();

        // Raycast�ŏ�Q�������o���A�ڕW�Ƃ���J�����������v�Z
        float targetDistance = CalculateTargetDistance();

        // ���ۂ̃J����������ڕW�����Ɍ����Ċ��炩�ɕ��
        UpdateCameraDistance(targetDistance);

        // �J�����̈ʒu���X�V���A�v���C���[�𒍎�������
        UpdateCameraPositionAndLook();
    }

    /// <summary>
    /// �}�E�X�̓��͂ɉ����ăJ�����̉�]�p�iyaw��pitch�j���X�V����
    /// </summary>
    void UpdateCameraRotation()
    {
        // �}�E�X���͂��擾
        Vector2 lookInput = InputSystem.actions["Look"].ReadValue<Vector2>();
        // �ُ�ȓ��͂𐧌�
        lookInput = Vector2.ClampMagnitude(lookInput, 50f);

        // �J�����̉�]�p���X�V
        // ���[�i���E�̊p�x�j���X�V
        _yaw += lookInput.x * mouseSensitivity * Time.deltaTime;
        // �s�b�`�i�㉺�̊p�x�j���X�V���A������������
        _pitch -= lookInput.y * mouseSensitivity * Time.deltaTime;
        // �s�b�`�̐�����K�p
        _pitch = Mathf.Clamp(_pitch, _pitchMin, _pitchMax);
    }

    /// <summary>
    /// Raycast�ŏ�Q�������o���A�ڕW�Ƃ���J����������Ԃ�
    /// </summary>
    float CalculateTargetDistance()
    {
        // �J�����̉�]���v�Z
        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0);
        // �J�����̈ʒu���v�Z
        Vector3 origin = _cameraTarget.position + Vector3.up * 1.5f;
        // �J�����̕������v�Z
        Vector3 direction = rotation * Vector3.back;

        // Raycast���g�p���ď�Q�������o
        if (Physics.Raycast(origin, direction, out RaycastHit hit, _defaultDistance, _obstacleMask))
        {
            // ��Q�������������ꍇ�A�q�b�g����������Ԃ�
            return Mathf.Clamp(hit.distance, _minDistance, _defaultDistance);
        }

        // ��Q�����Ȃ��ꍇ�̓f�t�H���g�̋�����Ԃ�
        return _defaultDistance;
    }

    /// <summary>
    /// ���ۂ̃J����������ڕW�����Ɍ����Ċ��炩�ɕ�Ԃ���
    /// </summary>
    void UpdateCameraDistance(float targetDistance)
    {
        // ���݂̋�����ڕW�����Ɍ����ĕ��
        _currentDistance = Mathf.Lerp(_currentDistance, targetDistance, Time.deltaTime * _zoomOutSpeed);
    }

    /// <summary>
    /// �J�����̈ʒu���X�V���A�v���C���[�𒍎�������
    /// </summary>
    void UpdateCameraPositionAndLook()
    {
        // �J�����̉�]���v�Z
        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0);
        Vector3 origin = _cameraTarget.position + Vector3.up * 1.5f;
        // �J�����̈ʒu���v�Z
        Vector3 direction = rotation * Vector3.back;
        // �ڕW�ʒu���v�Z
        Vector3 desiredPosition = origin + direction * _currentDistance;

        // Raycast�ŏ�Q�������o���A�J�����̈ʒu�𒲐�
        _cameraTransform.position = Vector3.SmoothDamp(_cameraTransform.position, desiredPosition, ref _currentVelocity, 1f / _smoothSpeed);
        // �J�����̌������v���C���[�Ɍ�����
        _cameraTransform.LookAt(origin);
    }
}
