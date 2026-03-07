using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float HorizontalInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool InteractionPressed { get; private set; }

    public bool AttackButtonPressed { get; private set; }

    private void Update()
    {
        HorizontalInput = Input.GetAxis(InputVariables.HORIZONTAL_AXIS);
        JumpPressed = Input.GetButtonDown(InputVariables.JUMP_BUTTON);
        InteractionPressed = Input.GetKeyDown(InputVariables.INTERACTION_BUTTON);
        AttackButtonPressed = Input.GetButton(InputVariables.ATTACK_BUTTON);
    }
}
