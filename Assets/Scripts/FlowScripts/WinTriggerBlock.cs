using UnityEngine;

public class WinTriggerBlock : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            EventManager.InvokePlayerWinEvents();
        }
    }
}
