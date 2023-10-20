using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;

namespace ShootEditor2D
{

    public interface IEnemyModel : IModel
    {
        //Dictionary<string,EnemyModelItem> EnemyModelItems { get; }
        EnemyModelItem GetEnemyModelByName(string name);
    }

    public class EnemyModelItem
    {
        //µ±Ç°ÑªÁ¿
        public BindableProperty<float> HP { get; set; } = new BindableProperty<float>();
        //×Óµ¯Ãû³Æ
        public string BulletName;
        //ÉäËÙ
        public float BulletSpeed;
        //¹¥»÷¾àÀë
        public float Flight;
        //¹¥»÷¼ä¸ô
        public float FiringRate;
        //¹¥»÷ÑÓÊ±
        public float DelayeTime;

        public bool IsAttack;

        public EnemyModelItem(EnemyModelItem item)
        {
            HP = new BindableProperty<float>() { Value = item.HP.Value };
            BulletName = item.BulletName;
            BulletSpeed = item.BulletSpeed;
            Flight = item.Flight;
            FiringRate = item.FiringRate;
            DelayeTime = item.DelayeTime;
            IsAttack = item.IsAttack;
        }
        public EnemyModelItem() { }
    }

    public class EnemyModel : AbstractIModel, IEnemyModel
    {
        private Dictionary<string,EnemyModelItem> EnemyModelItems { get; }=new Dictionary<string, EnemyModelItem>();

        public EnemyModelItem GetEnemyModelByName(string name)
        {
            return EnemyModelItems[name];
        }

        protected override void OnInit()
        {
            //NormalEnemy
            EnemyModelItems.Add(nameof(NormalEnemy), new EnemyModelItem()
            {
                HP = { Value=10},
                BulletName = "EBullet",
                BulletSpeed = 5f,
                Flight = 2.5f,
                FiringRate = 2f,
                DelayeTime = 0.2f,
                IsAttack = false
            });
            //LaserEnemy
            EnemyModelItems.Add(nameof(LaserEnemy), new EnemyModelItem()
            {
                HP = { Value = 12 },
                BulletName = "ELaser",
                BulletSpeed = 0f,
                Flight = 0.5f,
                FiringRate = 5f,
                DelayeTime = 2f,
                IsAttack = false
            });
            //CrossEnemy
            EnemyModelItems.Add(nameof(CrossEnemy), new EnemyModelItem()
            {
                HP = { Value = 8 },
                BulletName = "EBullet",
                BulletSpeed = 3f,
                Flight = 1.5f,
                FiringRate = 1.5f,
                DelayeTime = 0.2f,
                IsAttack = false
            });
            EnemyModelItems.Add(nameof(Boss1), new EnemyModelItem()
            {
                HP = new BindableProperty<float>() { Value=50},
                BulletName = "EBullet",
                BulletSpeed = 16f,
                Flight = 5f,
                FiringRate = 4f,
                DelayeTime = 0.1f,
                IsAttack = false
            });
            //EnemyModelItems.Add(new EnemyModelItem(12f));
            //EnemyModelItems.Add(new EnemyModelItem(8f));
            //EnemyModelItems.Add()
        }
    }
}
