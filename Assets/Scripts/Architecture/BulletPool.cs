using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private int _poolSize;

    private List<GameObject> _bulletPool;

    private void Awake()
    {
        _bulletPool = new List<GameObject>();

        for (int i = 0; i < _poolSize; i++)
        {
            GameObject bullet = Instantiate(_bulletPrefab, transform);
            bullet.SetActive(false);
            _bulletPool.Add(bullet);
        }
    }

    public GameObject GetBullet()
    {
        foreach (var b in _bulletPool)
        {
            if (!b.activeInHierarchy)
            {
                return b;
            }
        }
        return null;
    }

    public void ReturnBulletToPool(GameObject bullet)
    {
        bullet.SetActive(false);
        bullet.transform.SetParent(transform);
    }

}
