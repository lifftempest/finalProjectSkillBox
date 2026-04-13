using System.Collections;
using UnityEngine;

public class Fire_Behaviour : MonoBehaviour
{
    [SerializeField] private AudioClip _fireDoDamageClip;
    [SerializeField] private float _damage;
    [SerializeField] private float _launchPower;
    [SerializeField] private float _launchAngle;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponentInParent<HealthComponent>().TakeDamage(_damage);
            StartCoroutine(collision.transform.parent.GetComponent<PlayerController>().ExecutePushPlayer(transform, _launchAngle, _launchPower));
            AudioHandler.Instance.PlaySfx(_fireDoDamageClip);
        }
    }
}
