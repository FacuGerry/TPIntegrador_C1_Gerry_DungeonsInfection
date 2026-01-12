using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "GameData/KeyBindings")]
public class KeyBindingsSO : ScriptableObject
{
    [Header("Move")]
    public KeyCode up;
    public KeyCode left;
    public KeyCode down;
    public KeyCode right;

    public KeyCode up2;
    public KeyCode left2;
    public KeyCode down2;
    public KeyCode right2;

    [Header("Open Map")]
    public KeyCode openMap;
    public KeyCode openMap2;

    [Header("Interact / Use")]
    public KeyCode interact;
    public KeyCode interact2;

    [Header("Pause")]
    public KeyCode pause;
}
