using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShootEditor2D
{
    public class ShowPropInfoEvent
    {
        public string name;
        public string info;
        public bool isShow;

        public ShowPropInfoEvent(string name, string info,bool isShow)
        {
            this.name = name;
            this.info = info;
            this.isShow = isShow;
        }
    }
}