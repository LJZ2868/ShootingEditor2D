using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;

namespace ShootEditor2D
{
    public class ReloadingCommand : AbstractICommand
    {
        protected override void OnExecute()
        {
            var currentGun = this.GetSystem<IGunSystem>().CurrentGun;
            var gunItem = this.GetModel<IGunConfigModel>().GetItemByName(currentGun.Name.Value);

            currentGun.GunState.Value = GunState.Reloading;
            //������ͼ
            this.SendEvent<ReloadingEvent>();

            this.GetSystem<ITimeSystem>().AddDelayTask(gunItem.ReloadSeconds,()=> 
            {
                gunItem.BulletCountInGun = gunItem.MaxClip;

                currentGun.GunState.Value = GunState.Idle;
                //������ͼ
                this.SendEvent<ReloadingEvent>();
            });

            
        }
    }
}
