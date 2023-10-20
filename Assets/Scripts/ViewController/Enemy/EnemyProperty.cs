using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;
using BehaviorDesigner.Runtime;

namespace ShootEditor2D
{
    public abstract class EnemyProperty : MonoBehaviour, IController
    {
        //�ӵ�����(λ��)
        protected virtual Transform Bullet { get; set; }
        //��������
        public virtual EnemyModelItem EnemyItem { get; set; }
        //��¼���Ѫ��
        protected float MaxHP;

        protected const string ResourcesPath = "Game/Enemy/";
        protected string mBulletPath;

        protected PhysicsCheck mCheck;
        protected Rigidbody2D rigidbody2d;
        protected BehaviorTree bt;

        protected virtual bool IsCanMove { get; set; }


        //Enemy����
        protected abstract float Speed { get; set; }
        public virtual float CurrentMoveSpeed
        {
            get
            {
                return Speed;
            }
        }

        protected abstract string Name { get; }
        

        protected Matrix4x4 matrix;
        protected virtual void Awake()
        {
            IsCanMove = true;
            //��ȡ�ӵ����ͺ���������

            //����
            var item = this.GetModel<IEnemyModel>().GetEnemyModelByName(Name);
            EnemyItem = new EnemyModelItem(item);

            //�ӵ�
            //var info = this.GetSystem<IEnemyBulletSystem>().EnemyBulletInfos[0];
            //BulletInfo = new EnemyBulletInfo(info.Name, info.IsAttack, info.Damage);
            Bullet = transform.Find(EnemyItem.BulletName);
            mBulletPath = ResourcesPath + EnemyItem.BulletName;


            MaxHP = EnemyItem.HP.Value;
            //BulletItem = this.GetModel<IEnemyGunModel>().GetGunItemByName(BulletInfo.Name);
            //BulletItem=this.GetModel<IEnemyModel>().GetEnemyModelByName(EnemyItem.)

            mCheck = GetComponent<PhysicsCheck>();
            rigidbody2d = GetComponent<Rigidbody2D>();
            bt = GetComponent<BehaviorTree>();

            
        }

        protected Vector2 DirectionRotation(Vector2 direction, float angle)
        {
            //˳ʱ����ת����
            var radian = angle * Mathf.PI / 180;
            matrix.SetRow(0, new Vector2(Mathf.Cos(radian), Mathf.Sin(radian)));
            matrix.SetRow(1, new Vector2(-Mathf.Sin(radian), Mathf.Cos(radian)));
            return (matrix * (direction.normalized)).normalized;
        }

        protected virtual void OnEnable()
        {
            EnemyItem.HP.Value = MaxHP;
            //Debug.Log("maxhp"+MaxHP);
        }

        protected virtual void Move()
        {
            //��¼����
            var mLocalScale = transform.localScale.x;

            //�ƶ�
            if ((mCheck.isGround||mCheck.isPlatform) && IsCanMove)
            {
                rigidbody2d.velocity = new Vector2(CurrentMoveSpeed * transform.localScale.x * Time.deltaTime, rigidbody2d.velocity.y);
            }
            else if(!IsCanMove)
            {
                rigidbody2d.velocity = Vector2.zero;
            }
            if((!mCheck.isGround&&!mCheck.isPlatform)||mCheck.isWall)
            {
                //��ǽ�򵽾�ͷת��(���λ��ȡ��)
                mCheck.drawGroundPosition.x *= -1;
                mCheck.drawWallPosition.x *= -1;
                transform.localScale = new Vector3(-mLocalScale, 1, 1);
            }
        }

        protected void CreateBulletWithVector(Vector2 direction)
        {
            var bullet = PoolManager.Instance.GetObj(mBulletPath, Bullet.gameObject).GetComponent<EnemyBulletProperty>();
            bullet.enemyModelitem = new EnemyModelItem(EnemyItem);
            bullet.GetComponent<Rigidbody2D>().velocity = direction.normalized * EnemyItem.BulletSpeed;
            bullet.Attack();
        }

        protected void CreateFollowBullet()
        {
            //�����ӵ�
            if (gameObject.activeSelf)
            {
                var bullet = PoolManager.Instance.GetObj(mBulletPath, Bullet.gameObject).GetComponent<EnemyBulletProperty>();
                bullet.enemyModelitem = new EnemyModelItem(EnemyItem); 
                bullet.GetComponent<Rigidbody2D>().velocity = (Player.Instance.transform.position - transform.position).normalized * EnemyItem.BulletSpeed;
                //�����ӵ�����
                bullet.Attack();
            }
        }

        protected virtual void Attack()
        {
            var isFind = (SharedBool)bt.GetVariable("IsFind");
            if (isFind.Value&&!EnemyItem.IsAttack && (mCheck.isGround||mCheck.isPlatform))
            {
                EnemyItem.IsAttack = true;

                CreateFollowBullet();

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

        protected virtual void FixedUpdate()
        {
            Attack();
            Move();
        }

        public IIOC GetIOC()
        {
            return ShootEditor2D.Interface;
        }
    }   
}
