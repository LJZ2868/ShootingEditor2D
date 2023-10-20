using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;

namespace ShootEditor2D
{
    public class CreateEnemyCommand : AbstractICommand
    {
        public string name;

        public CreateEnemyCommand(string name)
        {
            this.name = name;
        }

        protected override void OnExecute()
        {
            this.SendEvent(new CreateEnemyEvent(name));
        }
    }
}