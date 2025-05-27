using UnityEngine;

public class CameraTarget : MonoBehaviour
{

    Vector3 firstPos;
    void Start()
    {
        firstPos = transform.position;
    }

    void Update()
    {
        transform.position = new Vector3(
            transform.position.x,
            firstPos.y,
            transform.position.z
            );
    }
}
