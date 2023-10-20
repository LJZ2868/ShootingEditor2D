using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;

namespace ShootEditor2D
{

    public class AddMaxHP : AbstractPropItem
    {
        public override int Level { get { return 1; } }

        public override string Name { get { return nameof(AddMaxHP); } }

        public override bool IsHave { get ; set ; }

        public override string Info { get { return "MaxHp + 1"; } }

        public override void Effect()
        {
            this.GetModel<IPlayerModel>().MaxHP.Value++;
            Debug.Log(nameof(AddMaxHP));
        }

    }   
}
