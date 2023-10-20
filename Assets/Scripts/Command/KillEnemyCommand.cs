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
            //��ɱ����
            this.GetModel<IPlayerModel>().TotalKill.Value++;

            //ӵ����Ѫ����ʱ����
            if(this.GetSystem<ICharacterSystem>().characterInfos[nameof(LifeStealing)].IsHave)
                this.GetModel<IPlayerModel>().LifeStealingCount.Value++;

            this.SendEvent<KillEnemyEvent>();
            Debug.Log("TotalKill:" + this.GetModel<IPlayerModel>().TotalKill.Value);
        }
    }
}