using UnityEngine;

// todo: remove: this is currently useless
public interface IBody
{
    public void Velocity(Vector2 value);
    public void VelocityX(float value);
    public void VelocityY(float value);

    public void Move(Vector2 position);
    public void StopVertical();
    public void ResetVertical();
    public void ToggleRotationFreeze(bool value);
}

