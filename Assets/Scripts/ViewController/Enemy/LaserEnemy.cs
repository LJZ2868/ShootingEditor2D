using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;
using BehaviorDesigner.Runtime;

namespace ShootEditor2D
{

    public class LaserEnemy : EnemyProperty
    {
        protected override float Speed { get; set; }
        protected override string Name => "LaserEnemy";

        protected override void Awake()
        {
            Speed = 80f;

            base.Awake();
        }

        protected override void Attack()
        {
            var isFind =(SharedBool)bt.GetVariable("IsFind");
            if (isFind.Value &&!EnemyItem.IsAttack && (mCheck.isGround||mCheck.isPlatform))
            {
                EnemyItem.IsAttack = true;
                //产生子弹
                CreateFollowBullet();
                //攻击时不能移动
                IsCanMove = false;
                //延迟发射
                this.GetSystem<ITimeSystem>().AddDelayTask(EnemyItem.DelayeTime + EnemyItem.Flight, () =>
                {
                    IsCanMove = true;
                });

                this.GetSystem<ITimeSystem>().AddDelayTask(EnemyItem.FiringRate, () =>
                {
                    EnemyItem.IsAttack = false;
                });
            }
        }

    }
}
