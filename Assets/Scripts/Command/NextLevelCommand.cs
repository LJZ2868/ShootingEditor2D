using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;

namespace ShootEditor2D
{
    public class NextLevelCommand : AbstractICommand
    {
        private GameObject obj;

        public NextLevelCommand(GameObject obj)
        {
            this.obj = obj;
        }

        protected override void OnExecute()
        {
            this.SendEvent(new NextLevelEvent(obj));
        }
    }
}