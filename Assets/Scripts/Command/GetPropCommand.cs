using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;

namespace ShootEditor2D
{

    public class GetPropCommand : AbstractICommand
    {
        private string name;

        public GetPropCommand(string name)
        {
            this.name = name;
        }

        protected override void OnExecute()
        {

        }
    }
}
