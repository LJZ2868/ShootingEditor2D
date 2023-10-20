using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;

namespace ShootEditor2D
{
    public class GetNewGunCommand : AbstractICommand
    {
        private string name;

        public GetNewGunCommand(string name)
        {
            this.name = name;
        }

        protected override void OnExecute()
        {
            this.GetSystem<IGunSystem>().GetNewGun(name);
            this.SendEvent<ChangeGunEvent>();
        }
    }
}
