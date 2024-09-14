using UnityEngine;

public static class PlayerInput
{
    public static bool Disabled;
    
    public static float GetAxisRaw(string axis)
    {
        if (Disabled) return 0;
        return Input.GetAxisRaw(axis);
    }
}