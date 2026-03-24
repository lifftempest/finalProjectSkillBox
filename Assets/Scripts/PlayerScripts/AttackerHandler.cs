using UnityEngine;

public class AttackerHandler: MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private BulletPool _bulletPool;
    [SerializeField] private Transform _firePosition;
    [Header("Parameters")]
    [SerializeField] private float _shootDelay;
    [SerializeField] private int _magazineSize;

    private float _currentDelayTime;
    private int _currentMagazineSize;

    private void Awake()
    {
        _currentMagazineSize = _magazineSize;
    }

    private void Update()
    {
        if (_currentDelayTime > 0)
        {
            _currentDelayTime -= Time.deltaTime;
        }
    }

    private void ExecuteShooting()
    {
        if (_currentMagazineSize > 0)
        {
            if (_currentDelayTime <= 0)
            {
                var bullet = _bulletPool.GetBullet();
                bullet.gameObject.transform.position = _firePosition.position;
                bullet.transform.parent = null;
                bullet.SetActive(true);
                bullet.GetComponent<BulletBehaviour>().FireBullet(_firePosition);

                _currentDelayTime = _shootDelay;
                _currentMagazineSize--;
            }
        }
        
    }

    public void Shoot(bool isShootButtonPressed, out bool isShooting)
    {
        if (isShootButtonPressed)
        {
            ExecuteShooting();
            isShooting = true;
        }
        else
        {
            isShooting = false;
        }
    }
}
