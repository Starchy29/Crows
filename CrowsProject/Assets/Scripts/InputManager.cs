using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction {
    None,
    Up,
    Down,
    Left,
    Right
}

public class InputManager : MonoBehaviour
{
    private Direction joystickDir;
    private Direction lastJoystickDir;

    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update()
    {
        // manage gamepad
        Vector2 joystick = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector2 dPad = new Vector2(Input.GetAxis("DPad X"), Input.GetAxis("DPad Y"));

        Vector2 currentInput = joystick;
        if(dPad != Vector2.zero) { // dPad accurately returns to (0,0), joystick does not
            // prioritize dPad input
            currentInput = dPad;
        }

        lastJoystickDir = joystickDir;
        if(currentInput.sqrMagnitude > 0.7 * 0.7) {
            if(Mathf.Abs(currentInput.x) > Mathf.Abs(currentInput.y)) { // horizontal
                if(currentInput.x > 0) {
                    joystickDir = Direction.Right;
                } else {
                    joystickDir = Direction.Left;
                }
            } else { // vertical
                if(currentInput.y > 0) {
                    joystickDir = Direction.Up;
                } else {
                    joystickDir = Direction.Down;
                }
            }
        }
        else if(currentInput.sqrMagnitude < 0.3 * 0.3) { // dead zone
            joystickDir = Direction.None;
        }
        // else joystick stays the same (0.3-0.7)
    }

    public bool JustPressed(Direction direction) {
        return GamePadJustPressed(direction) || KeyboardJustPressed(direction);
    }

    public bool ConfirmJustPressed() {
        return Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.JoystickButton2);
    }

    public bool CancelJustPressed() {
        return Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKeyDown(KeyCode.JoystickButton3);
    }

    private bool GamePadJustPressed(Direction direction) {
        return lastJoystickDir == Direction.None && joystickDir == direction;
    }
    
    private bool KeyboardJustPressed(Direction direction) {
        switch (direction) {
            case Direction.Up:
                return Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W);

            case Direction.Down:
                return Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S);

            case Direction.Left:
                return Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A);

            case Direction.Right:
                return Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D);

            default:
                return false;
        }
    }
}
