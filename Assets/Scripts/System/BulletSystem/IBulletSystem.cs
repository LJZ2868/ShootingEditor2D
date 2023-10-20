using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;

namespace ShootEditor2D
{
    public interface IBulletSystem : ISystem
    {
        Dictionary<string,BulletInfo> BulletInfos { get; }
    }

    public class BulletSystem : AbstractISystem, IBulletSystem
    {

        public Dictionary<string, BulletInfo> BulletInfos { get; } = new Dictionary<string, BulletInfo>();

        protected override void OnInit()
        {
            var atk = this.GetModel<IPlayerModel>().ATK.Value;
            //��ǹ�ӵ�
            BulletInfos.Add("pistol", new BulletInfo()
            {
                Name = "pistol",
                originalSpeed = 1f,
                originalFlight = 2f,
                originalDamage = 1 
            });
            //���ǹ�ӵ�
            BulletInfos.Add("submachine", new BulletInfo()
            {
                Name = "submachine",
                originalSpeed = 1.5f,
                originalFlight = 0.6f,
                originalDamage = 0.6f 
            });
            //����ǹ�ӵ�
            BulletInfos.Add("laser", new BulletInfo()
            {
                Name = "laser",
                originalSpeed = 0f,
                originalFlight = 1f,
                originalDamage = 0.5f
            });
        }
    }
}
