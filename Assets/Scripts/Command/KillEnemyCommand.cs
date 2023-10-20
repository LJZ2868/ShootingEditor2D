using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;

namespace ShootEditor2D
{
    public class KillEnemyCommand : AbstractICommand
    {
        protected override void OnExecute()
        {
            //击杀敌人
            this.GetModel<IPlayerModel>().TotalKill.Value++;

            //拥有吸血特性时计数
            if(this.GetSystem<ICharacterSystem>().characterInfos[nameof(LifeStealing)].IsHave)
                this.GetModel<IPlayerModel>().LifeStealingCount.Value++;

            this.SendEvent<KillEnemyEvent>();
            Debug.Log("TotalKill:" + this.GetModel<IPlayerModel>().TotalKill.Value);
        }
    }
}