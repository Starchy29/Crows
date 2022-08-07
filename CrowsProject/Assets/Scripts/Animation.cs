using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation
{
    private bool complete;
    public bool Complete { get { return complete; } }

    public Animation() {
        complete = false;
    }

    public void Update()
    {
        complete = true;
    }

    public Animation Copy() {
        return this;
    }
}
