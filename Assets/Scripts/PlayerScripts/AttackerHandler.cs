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
        print("Step 2");
        if (_currentMagazineSize > 0)
        {
            print("Step 3");
            if (_currentDelayTime <= 0)
            {
                print("Step 4");
                var bullet = _bulletPool.GetBullet();
                bullet.gameObject.transform.position = _firePosition.position;
                //bullet.gameObject.transform.localRotation = Quaternion.identity;
                bullet.transform.parent = null;
                bullet.SetActive(true);
                bullet.GetComponent<BulletBehaviour>().FireBullet(_firePosition);

                _currentDelayTime = _shootDelay;
                _currentMagazineSize--;
            }
        }
        
    }

    public void Shoot(bool isShootButtonPressed)
    {
        print("Step 1");
        if (isShootButtonPressed)
        {
            ExecuteShooting();
        }
    }
}
