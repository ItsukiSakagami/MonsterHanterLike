using UnityEngine;
using UnityEngine.InputSystem;

public class MonsterHunterCamera : MonoBehaviour
{
    [Header("ターゲット設定")]
    [SerializeField]
    private Transform _cameraTarget;
    [SerializeField]
    private Transform _cameraTransform;  //実際のカメラTransform
    [SerializeField]

    [Header("カメラ距離設定")]
    private float _defaultDistance = 5f;
    [SerializeField]
    private float _minDistance = 1.5f;  //目標に最も近づける距離（Raycast衝突時の最小距離）
    private float _currentDistance; //現在の実際のカメラ距離
    [SerializeField]
    private float _zoomOutSpeed = 2f; // ズームアウト速度

    [Header("マウス感度設定")]
    [SerializeField]
    private float mouseSensitivity = 3f;
    [SerializeField]
    private float _pitchMin = -20f; // カメラの仰角の最小値（真下方向）
    [SerializeField]
    private float _pitchMax = 60f;  // カメラの仰角の最大値（真上方向）

    [Header("カメラ追従設定")]
    [SerializeField]
    private float _smoothSpeed = 5f;
    [SerializeField]
    private LayerMask _obstacleMask;    // Raycast用：カメラとの間にある障害物のLayer
    private float _yaw = 0f;    // カメラの水平方向の回転角（左右）
    private float _pitch = 10f; // カメラの垂直方向の回転角（上下）
    private Vector3 _currentVelocity;   // SmoothDampで使用される現在の速度（内部的な補間状態）

    void Start()
    {
        _currentDistance = _defaultDistance;
    }

    void LateUpdate()
    {
        // カメラの回転と位置を更新
        UpdateCameraRotation();

        // Raycastで障害物を検出し、目標とするカメラ距離を計算
        float targetDistance = CalculateTargetDistance();

        // 実際のカメラ距離を目標距離に向けて滑らかに補間
        UpdateCameraDistance(targetDistance);

        // カメラの位置を更新し、プレイヤーを注視させる
        UpdateCameraPositionAndLook();
    }

    /// <summary>
    /// マウスの入力に応じてカメラの回転角（yawとpitch）を更新する
    /// </summary>
    void UpdateCameraRotation()
    {
        // マウス入力を取得
        Vector2 lookInput = InputSystem.actions["Look"].ReadValue<Vector2>();
        // 異常な入力を制限
        lookInput = Vector2.ClampMagnitude(lookInput, 50f);

        // カメラの回転角を更新
        // ヨー（左右の角度）を更新
        _yaw += lookInput.x * mouseSensitivity * Time.deltaTime;
        // ピッチ（上下の角度）を更新し、制限をかける
        _pitch -= lookInput.y * mouseSensitivity * Time.deltaTime;
        // ピッチの制限を適用
        _pitch = Mathf.Clamp(_pitch, _pitchMin, _pitchMax);
    }

    /// <summary>
    /// Raycastで障害物を検出し、目標とするカメラ距離を返す
    /// </summary>
    float CalculateTargetDistance()
    {
        // カメラの回転を計算
        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0);
        // カメラの位置を計算
        Vector3 origin = _cameraTarget.position + Vector3.up * 1.5f;
        // カメラの方向を計算
        Vector3 direction = rotation * Vector3.back;

        // Raycastを使用して障害物を検出
        if (Physics.Raycast(origin, direction, out RaycastHit hit, _defaultDistance, _obstacleMask))
        {
            // 障害物が見つかった場合、ヒットした距離を返す
            return Mathf.Clamp(hit.distance, _minDistance, _defaultDistance);
        }

        // 障害物がない場合はデフォルトの距離を返す
        return _defaultDistance;
    }

    /// <summary>
    /// 実際のカメラ距離を目標距離に向けて滑らかに補間する
    /// </summary>
    void UpdateCameraDistance(float targetDistance)
    {
        // 現在の距離を目標距離に向けて補間
        _currentDistance = Mathf.Lerp(_currentDistance, targetDistance, Time.deltaTime * _zoomOutSpeed);
    }

    /// <summary>
    /// カメラの位置を更新し、プレイヤーを注視させる
    /// </summary>
    void UpdateCameraPositionAndLook()
    {
        // カメラの回転を計算
        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0);
        Vector3 origin = _cameraTarget.position + Vector3.up * 1.5f;
        // カメラの位置を計算
        Vector3 direction = rotation * Vector3.back;
        // 目標位置を計算
        Vector3 desiredPosition = origin + direction * _currentDistance;

        // Raycastで障害物を検出し、カメラの位置を調整
        _cameraTransform.position = Vector3.SmoothDamp(_cameraTransform.position, desiredPosition, ref _currentVelocity, 1f / _smoothSpeed);
        // カメラの向きをプレイヤーに向ける
        _cameraTransform.LookAt(origin);
    }
}
