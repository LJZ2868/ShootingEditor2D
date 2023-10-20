using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;

namespace ShootEditor2D
{
    public class DodgeCommand : AbstractICommand
    {
        protected override void OnExecute()
        {
            this.GetModel<IPlayerModel>().IsInvincible = true;

            this.SendEvent<DodgeEvent>();
        }
    }
}