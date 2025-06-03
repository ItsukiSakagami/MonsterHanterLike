using UnityEngine;

public class StickEnemy : MonoBehaviour
{
    private int _hp = 5;

    [SerializeField]
    private Color _newColor = Color.red;
    private MeshRenderer _renderer;
    private Color _defaultColor;

    [SerializeField]
    private float _timer = 1.0f;
    private float _firstTimer;

    private bool _isHit = false;



    void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _firstTimer = _timer;
        _defaultColor = _renderer.material.color;
    }

    void Update()
    {
        if (_isHit)
        {
            _timer -= Time.deltaTime;
        }
        if (_timer <= 0.0f)
        {
            _isHit = false;
            _renderer.material.color = _defaultColor;
            _timer = _firstTimer;
        }

        if (_hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Attack"))
        {
            Debug.Log("Hit");
            _renderer.material.color = _newColor;
            _hp--;
            _isHit = true;
        }
    }
}
