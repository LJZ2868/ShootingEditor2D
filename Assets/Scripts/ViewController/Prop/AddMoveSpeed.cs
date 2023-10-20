using FrameworkDesign;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShootEditor2D
{
    public class AddMoveSpeed : AbstractPropItem
    {
        public override int Level => 2;

        public override string Name => nameof(AddMoveSpeed);

        public override bool IsHave { get ; set ; }

        public override string Info => "Speed + 10%";

        public override void Effect()
        {
            this.GetModel<IPlayerModel>().Speed.Value += 30;
        }

    }
}