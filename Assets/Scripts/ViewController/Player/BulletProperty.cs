using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;

namespace ShootEditor2D
{
    public abstract class BulletProperty : MonoBehaviour,IController
    {
        protected Rigidbody2D mRigidbody2d;
        //子弹信息
        protected BulletInfo bullet;

        protected virtual void Awake()
        {
            mRigidbody2d = GetComponent<Rigidbody2D>();
        }

        protected virtual void OnEnable()
        {
            bullet = this.GetSystem<IGunSystem>().CurrentGun.Bullet;

            //飞行速度
            mRigidbody2d.velocity = Gun.direction * bullet.Speed;

            Invoke(nameof(OnPush), bullet.Flight);
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                //没有穿透效果
                if (this.GetSystem<IGunSystem>().CurrentGun.GunType == GunType.Bullet)
                    gameObject.SetActive(false);

                collision.transform.GetComponent<EnemyProperty>().EnemyItem.HP.Value -= bullet.Damage;

                if (collision.transform.GetComponent<EnemyProperty>().EnemyItem.HP.Value <= 0)
                {
                    //发送击杀敌人command(触发事件)
                    //collision.gameObject.SetActive(false);
                    //存入对象池
                    PoolManager.Instance.PushObj(collision.name,collision.gameObject);

                    this.SendCommand<KillEnemyCommand>();
                }
            }
        }

        //存入对象池
        protected virtual void OnPush()
        {
            PoolManager.Instance.PushObj(gameObject.name, gameObject);
        }

        public IIOC GetIOC()
        {
            return ShootEditor2D.Interface;
        }
    }
}