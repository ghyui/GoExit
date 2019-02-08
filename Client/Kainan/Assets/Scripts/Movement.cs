using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Flags]
public enum MoveDirection
{
    NONE,
    UP,
    RIGHT,
    LEFT = 0x0004,
    DOWN = 0x0008,

    VERTICAL = UP | DOWN,
    HORIZONTAL = LEFT | RIGHT,

    ALL = VERTICAL | HORIZONTAL,
}

public class Movement
{
    public MoveDirection Direction { get; private set; }

    public void AddDirection(MoveDirection dir)
    {
        Direction |= dir;
    }
    public void RemoveDirection(MoveDirection dir)
    {
        Direction &= ~dir;
    }
    public bool HasDirection(MoveDirection dir)
    {
        return (Direction & dir) > 0;
    }
}