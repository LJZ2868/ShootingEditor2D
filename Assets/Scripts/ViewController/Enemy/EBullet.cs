using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;

namespace ShootEditor2D
{

    public class EBullet : EnemyBulletProperty
    {

        public override bool IsSpecialAttack { get ; set ; }

        protected override EnemyBulletInfo BulletInfo { get ; set ; }


        public override void Attack()
        {
            base.Attack();
        }

        private void FixedUpdate()
        {
            if (IsSpecialAttack)
            {
                //Debug.Log("special");
                UpdatePointList();
                ShootToPlayer();
            }
        }

        protected override void Awake()
        {
            var info = this.GetSystem<IEnemyBulletSystem>().EnemyBulletInfos[0];
            BulletInfo = new EnemyBulletInfo(info.Name, info.Damage);

            base.Awake();
        }


    }
}
