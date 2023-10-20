using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;

namespace ShootEditor2D
{
    public abstract class BulletProperty : MonoBehaviour,IController
    {
        protected Rigidbody2D mRigidbody2d;
        //�ӵ���Ϣ
        protected BulletInfo bullet;

        protected virtual void Awake()
        {
            mRigidbody2d = GetComponent<Rigidbody2D>();
        }

        protected virtual void OnEnable()
        {
            bullet = this.GetSystem<IGunSystem>().CurrentGun.Bullet;

            //�����ٶ�
            mRigidbody2d.velocity = Gun.direction * bullet.Speed;

            Invoke(nameof(OnPush), bullet.Flight);
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                //û�д�͸Ч��
                if (this.GetSystem<IGunSystem>().CurrentGun.GunType == GunType.Bullet)
                    gameObject.SetActive(false);

                collision.transform.GetComponent<EnemyProperty>().EnemyItem.HP.Value -= bullet.Damage;

                if (collision.transform.GetComponent<EnemyProperty>().EnemyItem.HP.Value <= 0)
                {
                    //���ͻ�ɱ����command(�����¼�)
                    //collision.gameObject.SetActive(false);
                    //��������
                    PoolManager.Instance.PushObj(collision.name,collision.gameObject);

                    this.SendCommand<KillEnemyCommand>();
                }
            }
        }

        //��������
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