using UnityEngine;

public class PlayerController : Character
{
    protected override InputDataDto GetInputParameters()
    {
        float moveHorizontal;
        float moveVertical;
        var vectorDelta = InputManager.instance.joystick.delta;
        if (vectorDelta == Vector2.zero)
        {
            moveHorizontal = Input.GetAxisRaw("Horizontal");
            moveVertical = Input.GetAxisRaw("Vertical");
        }
        else
        {
            moveHorizontal = vectorDelta.x;
            moveVertical = vectorDelta.y;
        }

        return new InputDataDto(moveHorizontal, moveVertical, false);
    }
}