using UnityEngine;

public class Fire_Behaviour : MonoBehaviour
{
    [SerializeField] private float _damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponentInParent<HealthComponent>().TakeDamage(_damage);
        }
    }
}
