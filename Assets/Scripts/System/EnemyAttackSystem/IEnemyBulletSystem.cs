using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;

namespace ShootEditor2D
{
    public interface IEnemyBulletSystem :ISystem
    {

        List<EnemyBulletInfo> EnemyBulletInfos { get; set; }
    }

    public class EnemyBulletInfo
    {
        //����
        public string Name;
        ////�����ٶ�
        //public float Speed;
        ////����ʱ��
        //public float Flight;
        ////�������
        //public float FiringRate;

        //public bool IsAttack;

        public float Damage;

        public EnemyBulletInfo(string name,float damage)
        {
            Name = name;
            //IsAttack = isAttack;
            Damage = damage;
        }
    }

    public class EnemyBulletSystem : AbstractISystem, IEnemyBulletSystem
    {
        public List<EnemyBulletInfo> EnemyBulletInfos { get; set; } = new List<EnemyBulletInfo>();

        protected override void OnInit()
        {

            EnemyBulletInfos.Add(new EnemyBulletInfo("EBullet", 0.5f));
            EnemyBulletInfos.Add(new EnemyBulletInfo("ELaser", 0.5f));
            //EnemyBulletInfos.Add(new EnemyBulletInfo("CEBullet", 0.5f));

        }
    }


}
