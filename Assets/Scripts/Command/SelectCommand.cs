using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;

namespace ShootEditor2D
{
    public class SelectCommand : AbstractICommand
    {

        protected override void OnExecute()
        {
            this.SendEvent<SelectEvent>();
        }
    }
}