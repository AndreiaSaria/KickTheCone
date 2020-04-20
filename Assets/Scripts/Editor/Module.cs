using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction {
    NONE,
    FORWARD,
    RIGHT,
    LEFT
}

public class Module : MonoBehaviour
{
    public Direction Direction;
    public Transform Start;
    public Transform End;
}
