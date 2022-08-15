using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonScript : MonoBehaviour
{
    public Color regularColor;
    public Color selectedColor;
    [SerializeField] private UnityEvent clickEvent;
    public List<GameObject> ToolTips; // things that show only when this button is hovered

    private Rect box;
    public Menu Group { get; set; } // must not be null

    // Start is called before the first frame update
    void Start()
    {
        SetBox();
    }

    public void Select() {
        gameObject.GetComponent<SpriteRenderer>().color = selectedColor;
        foreach(GameObject tooltip in ToolTips) {
            tooltip.SetActive(true);
        }
    }

    public void Deselect() {
        gameObject.GetComponent<SpriteRenderer>().color = regularColor;
        foreach(GameObject tooltip in ToolTips) {
            tooltip.SetActive(false);
        }
    }

    public void Click() {
        clickEvent.Invoke();
    }

    // finds the best button to go to based on the input direction. loops around the screen if necessary
    public ButtonScript GetClosestNeighbor(Direction direction, List<ButtonScript> buttons = null) {
        // check if this has been calculated before
        ButtonScript closestOption = this;
        Vector2 screenDims = new Vector2(18, 10); // estimate
        float closestDistance = (direction == Direction.Left || direction == Direction.Right ? screenDims.x : screenDims.y);

        if(buttons == null) {
            buttons = Group.Buttons;
        }
        foreach(ButtonScript other in buttons) {
            if(other == this) {
                continue;
            }

            Vector2 comparePosition = other.gameObject.transform.position;
            switch(direction) {
                case Direction.Up:
                    // if below, "shift" to be above
                    if(other.box.yMin < this.box.yMax) {
                        comparePosition.y += screenDims.y;
                    }
                    break;
                case Direction.Down:
                    // if above, "shift" to be below
                    if(other.box.yMax > this.box.yMin) {
                        comparePosition.y -= screenDims.y;
                    }
                    break;
                case Direction.Left:
                    // if to the right, "shift" to be left
                    if(other.box.xMax > this.box.xMin) {
                        comparePosition.x -= screenDims.x;
                    }
                    break;
                case Direction.Right:
                    // if to the left, "shift" to be right
                    if(other.box.xMin < this.box.xMax) {
                        comparePosition.x += screenDims.x;
                    }
                    break;
            }

            float distance = Vector2.Distance(comparePosition, gameObject.transform.position);
            if(distance < closestDistance) {
                closestOption = other;
                closestDistance = distance;
            }
        }

        return closestOption;
    }

    // saves a rectangle that matches the button's shape
    public void SetBox() {
        Vector3 pos = gameObject.transform.position;
        Vector3 scale = gameObject.transform.localScale;
        box = new Rect(pos.x - scale.x / 2, pos.y - scale.y / 2, scale.x, scale.y);
    }
}
