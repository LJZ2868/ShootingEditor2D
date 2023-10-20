using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;

namespace ShootEditor2D
{
    public class ChangeGunCommand : AbstractICommand
    {
        protected override void OnExecute()
        {

            Debug.Log("CurrentGun= " + this.GetSystem<IGunSystem>().CurrentGun.Name.Value);
            this.SendEvent<ChangeGunEvent>();
        }
    }

}