using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "GameData/KeyBindings")]
public class KeyBindingsSO : ScriptableObject
{
    [Header("Move")]
    public KeyCode up;
    public KeyCode left;
    public KeyCode down;
    public KeyCode right;

    [Header("Interact / Use")]
    public KeyCode interact;

    [Header("Pause")]
    public KeyCode pause;
}
