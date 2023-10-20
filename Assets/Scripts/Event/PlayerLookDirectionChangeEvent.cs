using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShootEditor2D
{
    public class PlayerLookDirectionChangeEvent
    {
        public Vector2 lookDirection;
        public PlayerLookDirectionChangeEvent(Vector2 vector)
        {
            lookDirection = vector;
        }
    }
}