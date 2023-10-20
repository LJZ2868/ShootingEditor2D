using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;

namespace ShootEditor2D
{

    public class BulletCountQuery : AbstractQuery<int>
    {
        private string name;

        public BulletCountQuery(string name)
        {
            this.name = name;
        }

        protected override int OnDo()
        {
            var gunConfigModel = this.GetModel<IGunConfigModel>();
            var gunConfigItem = gunConfigModel.GetItemByName(name);
            return gunConfigItem.BulletCountInGun;
        }
    }
}
