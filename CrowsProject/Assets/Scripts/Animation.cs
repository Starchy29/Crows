using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    public EventType OnComplete { get; set; }

    public Animation() {
    }

    void Update()
    {
        OnComplete();
        // delete self?
    }

    public Animation Copy() {
        // to do: make this copy
        return this;
    }
}
