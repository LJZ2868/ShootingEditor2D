using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;

namespace ShootEditor2D
{
    public class PlayerLookDirectionChangeCommand : AbstractICommand
    {
        Vector2 lookDirection;

        public PlayerLookDirectionChangeCommand(Vector2 vector)
        {
            lookDirection = vector;
        }

        protected override void OnExecute()
        {
            this.SendEvent(new PlayerLookDirectionChangeEvent(lookDirection));
        }

    }
}