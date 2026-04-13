using System;
using UnityEngine;

public class AttackerHandler: MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private BulletPool _bulletPool;
    [SerializeField] private Transform _firePosition;
    [Header("Parameters")]
    [SerializeField] private float _shootDelay;
    [SerializeField] private int _magazineSize;

    public Action<int> OnShoot;

    private float _currentDelayTime;
    private int _currentMagazineSize;

    public int CurrentMagazine => _currentMagazineSize;
    public int MaxMagazine => _magazineSize;

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

    private void ExecuteShooting(AudioClip shootClip, AudioClip emptyMagazine)
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
                AudioHandler.Instance.PlaySfx(shootClip);

                _currentDelayTime = _shootDelay;
                _currentMagazineSize--;
                OnShoot?.Invoke(_currentMagazineSize);
            }
        }
        if (_currentMagazineSize <= 0 && _currentDelayTime <= 0)
        {
            AudioHandler.Instance.PlaySfx(emptyMagazine);
            _currentDelayTime = _shootDelay;
        }
    }

    public void ReloadMagazine()
    {
        _currentMagazineSize = _magazineSize;
        OnShoot?.Invoke(_currentMagazineSize);
    }

    public void Shoot(bool isShootButtonPressed, out bool isShooting, AudioClip shootClip, AudioClip emptyMagazine)
    {
        if (isShootButtonPressed)
        {
            ExecuteShooting(shootClip, emptyMagazine);
            isShooting = true;
        }
        else
        {
            isShooting = false;
        }
    }
}
