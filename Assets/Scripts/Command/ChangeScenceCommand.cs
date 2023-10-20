using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;

namespace ShootEditor2D
{
    public class ChangeScenceCommand : AbstractICommand
    {
        protected override void OnExecute()
        {
            //触发改变场景事件
            PoolManager.Instance.PoolClear();
            this.SendEvent<ChangeScenceEvent>();
        }
    }
}