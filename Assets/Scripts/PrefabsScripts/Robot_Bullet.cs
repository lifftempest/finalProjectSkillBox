using UnityEngine;

public class Robot_Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidBody;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _bulletDamage;

    private BulletPool _bulletPool;

    private void Start()
    {
        _bulletPool = FindAnyObjectByType<Enemy_Robot>().gameObject.GetComponentInChildren<BulletPool>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponentInParent<HealthComponent>().TakeDamage(_bulletDamage);
            ReturnToPool();
            print("Robot deal damage to Player");
        }
        else
        {
            ReturnToPool();
        }
    }

    public void FireBullet(Transform transform)
    {
        _rigidBody.linearVelocity = transform.lossyScale.x * _bulletSpeed * transform.right;
    }

    private void ReturnToPool()
    {
        _bulletPool.ReturnBulletToPool(gameObject);
    }
}
