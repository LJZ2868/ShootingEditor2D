using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShootEditor2D
{
    public class SaveLevelDateEvent 
    {
        public string Name;

        public SaveLevelDateEvent(string name)
        {
            Name = name;
        }
    }
}