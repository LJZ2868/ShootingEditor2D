using FrameworkDesign;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShootEditor2D
{
    public class CrossEnemy : EnemyProperty
    {
        protected override float Speed { get ; set ; }
        protected override string Name => "CrossEnemy";

        protected override void Awake()
        {
            Speed = 120f;

            base.Awake();
        }

        protected override void Attack()
        {
            if (!EnemyItem.IsAttack && (mCheck.isGround||mCheck.isPlatform))
            {
                EnemyItem.IsAttack = true;
                //�����ӵ�

                if (gameObject.activeSelf)
                {
                    CreateBulletWithVector(Vector2.up);
                    CreateBulletWithVector(Vector2.down);
                    CreateBulletWithVector(Vector2.left);
                    CreateBulletWithVector(Vector2.right);
                }

                //����ʱ�����ƶ�
                IsCanMove = false;
                //�ӳٷ���
                this.GetSystem<ITimeSystem>().AddDelayTask(EnemyItem.DelayeTime, () =>
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