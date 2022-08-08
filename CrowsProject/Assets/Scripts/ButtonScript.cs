using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] private Color regularColor;
    [SerializeField] private Color selectedColor;
    [SerializeField] private UnityEvent clickEvent;
    private Rect box;
    public ButtonGroup Group { get; set; } // must not be null

    private Dictionary<Direction, ButtonScript> closestNeighbors;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = gameObject.transform.position;
        Vector3 scale = gameObject.transform.localScale;
        box = new Rect(pos.x - scale.x / 2, pos.y - scale.y / 2, scale.x, scale.y);

        closestNeighbors = new Dictionary<Direction, ButtonScript>();
        closestNeighbors[Direction.Up] = null;
        closestNeighbors[Direction.Down] = null;
        closestNeighbors[Direction.Left] = null;
        closestNeighbors[Direction.Right] = null;
    }

    public void Select() {
        gameObject.GetComponent<SpriteRenderer>().color = selectedColor;
    }

    public void Deselect() {
        gameObject.GetComponent<SpriteRenderer>().color = regularColor;
    }

    public void Click() {
        clickEvent.Invoke();
    }

    // finds the best button to go to based on the input direction. loops around the screen if necessary
    public ButtonScript GetClosestNeighbor(Direction direction) {
        // check if this has been calculated before
        ButtonScript check = closestNeighbors[direction];
        if(check != null) {
            return check;
        }

        ButtonScript closestOption = this;
        Vector2 screenDims = new Vector2(18, 10); // estimate
        float closestDistance = (direction == Direction.Left || direction == Direction.Right ? screenDims.x : screenDims.y);

        foreach(ButtonScript other in Group.Buttons) {
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

        closestNeighbors[direction] = closestOption;
        return closestOption;
    }
}
