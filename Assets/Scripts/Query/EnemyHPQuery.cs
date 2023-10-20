using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;

namespace ShootEditor2D
{
    public class EnemyHPQuery : AbstractQuery<BindableProperty<float>>
    {
        private EnemyProperty enemy;

        public EnemyHPQuery(EnemyProperty enemy)
        {
            this.enemy = enemy;
        }

        protected override BindableProperty<float> OnDo()
        {
            return enemy.EnemyItem.HP;
        }
    }
}
