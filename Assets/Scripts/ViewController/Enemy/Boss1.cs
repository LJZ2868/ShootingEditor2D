using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;
using System;

namespace ShootEditor2D
{
    public class Boss1 : EnemyProperty
    {
        protected override float Speed { get ; set ; }

        protected override string Name => "Boss1";

        protected override void Awake()
        {
            Speed = 20f;

            base.Awake();
        }

        protected override void Move()
        {
            var target = Player.Instance.transform;

            //��player�ƶ�
            if (IsCanMove)
            {
                var direction = (target.position - transform.position).magnitude >= 17f ? target.position - transform.position : (target.position - transform.position).normalized * 17;
                rigidbody2d.velocity = direction * CurrentMoveSpeed * Time.deltaTime;
                IsCanMove = false;
                //Debug.Log(direction.magnitude);
            }
            //ײǽ
            if (mCheck.isWall)
            {
                this.GetSystem<ITimeSystem>().AddDelayTask(EnemyItem.DelayeTime, () =>
                {
                    IsCanMove = true;
                });
            }
        }

        protected override void Attack()
        {
            if (!EnemyItem.IsAttack && mCheck.isWall)
            {
                EnemyItem.IsAttack = true;

                var direction = Player.Instance.transform.position-transform.position;


                //������ʽ1
                Attack1(direction);
                this.GetSystem<ITimeSystem>().AddDelayTask(0.2f, () =>
                 {
                     Attack1(direction);
                 });

                //2
                if (EnemyItem.HP.Value <= MaxHP / 2)
                {
                    Attack2();
                }
                //Attack2();
                //�ӳٷ���
                this.GetSystem<ITimeSystem>().AddDelayTask(EnemyItem.FiringRate, () =>
                {
                    EnemyItem.IsAttack = false;
                });
            }
        }

        private void Attack2()
        {
            var bullet = PoolManager.Instance.GetObj(mBulletPath, Bullet.gameObject).GetComponent<EnemyBulletProperty>();
            bullet.enemyModelitem = new EnemyModelItem(EnemyItem); 
            bullet.enemyModelitem.Flight = EnemyItem.Flight * 1.5f;
            bullet.GetComponent<EnemyBulletProperty>().IsSpecialAttack = true;
            bullet.Attack();
        }

        void Attack1(Vector2 position)
        {
            //�����ӵ�(�м�ĳ���player)
            CreateBulletWithVector(position);
            //��������ƫ��25�Ƚǵ��ӵ�
            CreateBulletWithVector(DirectionRotation(position, 25));
            CreateBulletWithVector(DirectionRotation(position, -25));
        }
    }
}