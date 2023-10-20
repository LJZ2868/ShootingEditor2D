using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;

namespace ShootEditor2D
{
    public abstract class EnemyBulletProperty : MonoBehaviour,IController
    {

        protected Rigidbody2D rigidbody2d;
        protected GameObject player;
        //子弹信息
        protected abstract EnemyBulletInfo BulletInfo { get; set; }

        public virtual EnemyModelItem enemyModelitem { get; set; }

        //protected abstract string Name { get; }

        // 使用List储存Player前几个位置的坐标点
        List<Vector2> pointList = new List<Vector2>();
        // 需要记录的坐标点的数量
        int pointSize = 40;
        public virtual bool IsSpecialAttack { get; set; }

        protected virtual void Awake()
        { 

            rigidbody2d = GetComponent<Rigidbody2D>();
            player = Player.Instance.gameObject;
        }

        //初始化坐标列表
        protected void InitPointList()
        {
            pointList.Clear();
            for (int i = 0; i < pointSize; i++)
            {
                pointList.Add(Player.Instance.transform.position-transform.position);
            }

        }

        //更新坐标列表
        protected void UpdatePointList()
        {
            //移除第一个坐标点
            pointList.RemoveAt(0);
            //添加新的坐标点
            pointList.Add(Player.Instance.transform.position-transform.position);
        }

        public virtual void Attack()
        {
            this.GetSystem<ITimeSystem>().AddDelayTask(enemyModelitem.DelayeTime, () =>
            {
                Invoke(nameof(OnPush), enemyModelitem.Flight);
            });
        }

        protected virtual void OnEnable()
        {
            gameObject.SetActive(true);
            InitPointList();   
        }

        protected virtual void ShootToPlayer()
        {
            //朝向Player射击
            rigidbody2d.velocity = pointList[0].normalized * enemyModelitem.BulletSpeed;
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {

            if (collision.gameObject.CompareTag("Player"))
            {
                this.SendCommand(new HurtPlayerCommand(BulletInfo.Damage));
                gameObject.SetActive(false);
            }
        }

        protected virtual void OnPush()
        {
            PoolManager.Instance.PushObj(gameObject.name, gameObject);
            IsSpecialAttack = false;
        }

        public IIOC GetIOC()
        {
            return ShootEditor2D.Interface;
        }
    }   
}
