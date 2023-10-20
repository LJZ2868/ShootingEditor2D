using FrameworkDesign;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShootEditor2D
{
    public class AddATK : AbstractPropItem
    {
        public override int Level => 2;

        public override string Name => nameof(AddATK);

        public override bool IsHave { get ; set ; }

        public override string Info => "Damage + 10%";

        public override void Effect()
        {
            this.GetModel<IPlayerModel>().ATK.Value += 0.1f;
        }

    }
}