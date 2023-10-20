using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;

namespace ShootEditor2D
{
    public interface IGunSystem : ISystem
    {
        List<GunInfo> Guns { get; }
        GunInfo CurrentGun { get; set; }
        void GetNewGun(string name);
    }

    public class GunSystem : AbstractISystem, IGunSystem
    {
        public List<GunInfo> Guns { get; } = new List<GunInfo>();

        protected override void OnInit()
        {
            var mBullet = this.GetSystem<IBulletSystem>();

            CurrentGun = new GunInfo()
            {
                Name = new BindableProperty<string>() { Value = "pistol" },
                GunState = new BindableProperty<GunState>() { Value = GunState.Idle },
                Bullet = mBullet.BulletInfos["pistol"],
                GunType = GunType.Bullet
            };
            // ÷«π
            Guns.Add(CurrentGun);
            //≥Â∑Ê«π
            Guns.Add(new GunInfo()
            {
                Name = new BindableProperty<string>() { Value = "submachine" },
                GunState = new BindableProperty<GunState>() { Value = GunState.Idle },
                Bullet = mBullet.BulletInfos["submachine"],
                GunType = GunType.Bullet
            });
            //º§π‚
            Guns.Add(new GunInfo()
            {
                Name = new BindableProperty<string>() { Value = "laser" },
                GunState = new BindableProperty<GunState>() { Value = GunState.Idle },
                Bullet = mBullet.BulletInfos["laser"],
                GunType = GunType.Laser
            });
        }

        public void GetNewGun(string name)
        {
            var mBullet = this.GetSystem<IBulletSystem>();

            Guns.Add(CurrentGun = new GunInfo()
            {
                Name = new BindableProperty<string>() { Value = name },
                GunState = new BindableProperty<GunState>() { Value = GunState.Idle },
                Bullet = mBullet.BulletInfos[name]
            });
        }

        public GunInfo CurrentGun { get; set; } 

        
    }
}