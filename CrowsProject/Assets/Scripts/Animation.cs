using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    public EventType OnComplete { get; set; }

    void Update()
    {
        // end animation
        OnComplete();
        Destroy(this);
    }

    public Animation CopyFrom(Animation other) {
        // to do: make this copy from the other animation
        return this;
    }
}
