using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parametric
{
    public delegate float Math(float input);
    private Math xFunc;
    private Math yFunc;

    public Parametric(Math xFunc, Math yFunc) {
        this.xFunc = xFunc;
        this.yFunc = yFunc;
    }

    public Vector2 Calculate(float t) {
        return new Vector2(xFunc(t), yFunc(t));
    }
}
