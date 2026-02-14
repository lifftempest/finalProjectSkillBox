using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float HorizontalInput { get; private set; }
    public bool JumpPressed { get; private set; }

    private void Update()
    {
        HorizontalInput = Input.GetAxis("Horizontal");
        JumpPressed = Input.GetButtonDown("Jump");
    }
}
