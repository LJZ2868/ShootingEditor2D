using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;

namespace ShootEditor2D
{
    public class ShowPropInfoCommand : AbstractICommand
    {
        public string name;
        public string info;
        public bool isShow;

        public ShowPropInfoCommand(string name, string info,bool isShow)
        {
            this.name = name;
            this.info = info;
            this.isShow = isShow;
        }

        protected override void OnExecute()
        {
            this.SendEvent(new ShowPropInfoEvent(name, info,isShow));
        }
    }
}