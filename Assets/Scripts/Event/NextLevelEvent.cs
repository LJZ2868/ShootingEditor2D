using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShootEditor2D
{
    public class NextLevelEvent 
    {
        public GameObject obj;
        public NextLevelEvent(GameObject obj)
        {
            this.obj = obj;
        }
    }
}