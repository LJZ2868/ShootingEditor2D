using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;

namespace ShootEditor2D
{
    public class ShootingCommand : AbstractICommand
    {
        protected override void OnExecute()
        {
            var gunSystem = this.GetSystem<IGunSystem>();
            gunSystem.CurrentGun.GunState.Value = GunState.Shooting;

            //查询枪械信息
            var gunItem = this.GetModel<IGunConfigModel>().GetItemByName(gunSystem.CurrentGun.Name.Value);
            gunItem.BulletCountInGun--;

            //更新ui视图
            this.SendEvent<ShootingEvent>();

            this.GetSystem<ITimeSystem>().AddDelayTask(gunItem.FiringRate, () =>
            {
                 gunSystem.CurrentGun.GunState.Value = GunState.Idle;
                 this.SendEvent<ShootingEvent>();
            });
        }

    }
}