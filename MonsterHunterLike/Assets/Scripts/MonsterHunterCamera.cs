using UnityEngine;
using UnityEngine.InputSystem;

public class MonsterHunterCamera : MonoBehaviour
{
    [SerializeField]
    private Transform _cameraTarget;         // プレイヤー
    [SerializeField]
    private Transform cameraTransform; // カメラ

    [SerializeField]
    private float distance = 5f;       // カメラ距離
    [SerializeField]
    private float mouseSensitivity = 3f;
    [SerializeField]
    private float pitchMin = -20f;
    [SerializeField]
    private float pitchMax = 60f;

    [SerializeField]
    private float smoothSpeed = 5f;   // カメラ追従速度
    [SerializeField]
    private LayerMask obstacleMask;   // 障害物判定レイヤー

    private float yaw = 0f;
    private float pitch = 10f;

    private Vector3 currentVelocity;

    void LateUpdate()
    {
        // マウス入力を取得
        Vector2 lookInput = InputSystem.actions["Look"].ReadValue<Vector2>();

        // 1フレームの入力値の上限を制限
        lookInput = Vector2.ClampMagnitude(lookInput, 50f); 

        // マウス感度を掛けて角度更新
        yaw += lookInput.x * mouseSensitivity * Time.deltaTime;
        pitch -= lookInput.y * mouseSensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        // カメラの理想位置を計算
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 idealPos = _cameraTarget.position + Vector3.up * 1.5f + rotation * new Vector3(0, 0, -distance);

        // 障害物にぶつかっていれば距離を縮める
        RaycastHit hit;
        if (Physics.Linecast(_cameraTarget.position + Vector3.up * 1.5f, idealPos, out hit, obstacleMask))
        {
            idealPos = hit.point;
        }

        // カメラを滑らかに移動させる
        cameraTransform.position = Vector3.SmoothDamp(cameraTransform.position, idealPos, ref currentVelocity, 1f / smoothSpeed);

        // プレイヤーを注視
        cameraTransform.LookAt(_cameraTarget.position + Vector3.up * 1.5f);
    }
}
