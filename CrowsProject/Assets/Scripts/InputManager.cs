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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 joystick = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector2 dPAd = new Vector2(Input.GetAxis("DPad X"), Input.GetAxis("DPad Y"));

        Vector2 currentInput = dPAd;
        if(joystick != Vector2.zero) {
            // prioritize joystick input
            currentInput = joystick;
        }

        Direction currentDir = Direction.None;
        if(currentInput.sqrMagnitude > 0.3 * 0.3) { // dead zone
            if(Mathf.Abs(currentInput.x) > Mathf.Abs(currentInput.y)) { // horizontal
                if(currentInput.x > 0) {
                    currentDir = Direction.Right;
                } else {
                    currentDir = Direction.Left;
                }
            } else { // vertical
                if(currentInput.y > 0) {
                    currentDir = Direction.Up;
                } else {
                    currentDir = Direction.Down;
                }
            }
        }

        lastJoystickDir = joystickDir;
        joystickDir = currentDir;

        // test
        if(InputJustPressed(Direction.Up)) {
            Debug.Log("up");
        }
        if(InputJustPressed(Direction.Down)) {
            Debug.Log("down");
        }
        if(InputJustPressed(Direction.Left)) {
            Debug.Log("left");
        }
        if(InputJustPressed(Direction.Right)) {
            Debug.Log("right");
        }
    }

    public bool InputJustPressed(Direction direction) {
        // check gamepad
        if(GamePadJustPressed(direction)) {
            return true;
        }

        // check keyboard
        switch(direction) {
            case Direction.Up:
                return Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);

            case Direction.Down:
                return Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.DownArrow);

            case Direction.Left:
                return Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);

            case Direction.Right:
                return Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
            
            default:
                return false;
        }
    }

    private bool GamePadJustPressed(Direction direction) {
        return lastJoystickDir == Direction.None && joystickDir == direction;
    }
}
