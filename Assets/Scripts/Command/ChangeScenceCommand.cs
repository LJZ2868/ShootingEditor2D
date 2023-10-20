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
            //�����ı䳡���¼�
            PoolManager.Instance.PoolClear();
            this.SendEvent<ChangeScenceEvent>();
        }
    }
}