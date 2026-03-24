using UnityEngine;
using static UnityEngine.UI.Image;

public class BulletBehaviour : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Sprite[] _bulletSprites;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Rigidbody2D _rigidBody;
    [Space(5), Header("Parameters")]
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _bulletDamage;
    [SerializeField] private float _cameraBufferValue;

    private BulletPool _bulletPool;
    private Camera _mainCamera;

    private void Start()
    {
        _bulletPool = FindAnyObjectByType<BulletPool>();
        SetUpBulletSprite();
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if(this.enabled)
        CheckScreenPosition();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent<HealthComponent>(out var health))
            {
                health.TakeDamage(_bulletDamage);
                Debug.Log("Shot " + health.CurrentHealth);
            }

            ReturnToPool();
        }
    }

    public void FireBullet(Transform transform)
    {
        _rigidBody.linearVelocity = transform.lossyScale.x * _bulletSpeed * transform.right;

    }

    private void CheckScreenPosition()
    {
        Vector3 screenPosition = _mainCamera.WorldToViewportPoint(transform.position);
        if (screenPosition.x < 0 - _cameraBufferValue || screenPosition.x > 1 + _cameraBufferValue ||
            screenPosition.y < 0 - _cameraBufferValue || screenPosition.y > 1 + _cameraBufferValue)
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        _bulletPool.ReturnBulletToPool(gameObject);
    }

    #region SetupSprite
    private void SetUpBulletSprite()
    {
        var sprite = GetRandomSprite();
        _spriteRenderer.sprite = sprite;
        switch (sprite.name)
        {
            case "Shot_8":
                _spriteRenderer.gameObject.transform.localScale = new Vector3(4, 4, 4);
                break;
            case "Shot_3":
                _spriteRenderer.gameObject.transform.localScale = new Vector3(5, 5, 5);
                _spriteRenderer.gameObject.transform.localRotation = Quaternion.Euler(0, 0, -45);
                break;
            case "Shot_5":
                _spriteRenderer.gameObject.transform.localScale = new Vector3(6, 6, 6);
                _spriteRenderer.gameObject.transform.localRotation = Quaternion.Euler(0, 0, -45);
                break;
            default: break;
        }            
    }

    private Sprite GetRandomSprite()
    {
        int spriteIndex = Random.Range(0, _bulletSprites.Length);
        return _bulletSprites[spriteIndex];
    }
    #endregion
}
