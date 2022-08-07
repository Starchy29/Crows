using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] private UnityEvent clickEvent;
    private Rect box;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = gameObject.transform.position;
        Vector3 scale = gameObject.transform.localScale;
        box = new Rect(pos.x - scale.x / 2, pos.y - scale.y / 2, scale.x, scale.y);
    }

    // Update is called once per frame
    void Update()
    {
        bool hovered = box.Contains(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if(hovered) {
            // highlight color
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0.1f, 0.1f, 0.5f);
        } else {
            // regular color
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
        }

        if(Input.GetMouseButtonDown(0) && hovered) {
            //click
            clickEvent.Invoke();
        }
    }
}
