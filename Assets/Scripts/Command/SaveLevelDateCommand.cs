using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;

namespace ShootEditor2D
{
    public class SaveLevelDateCommand : AbstractICommand
    {
        public string Name;

        public SaveLevelDateCommand(string name)
        {
            Name = name;
        }

        protected override void OnExecute()
        {
            this.SendEvent(new SaveLevelDateEvent(Name));
        }
    }
}