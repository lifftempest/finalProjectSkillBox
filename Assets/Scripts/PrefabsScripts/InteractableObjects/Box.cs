using UnityEngine;

public class Box : InteractableObject
{
    protected override void ExecuteInteraction()
    {
        Debug.Log("INTERACTION BOX");
    }
}
