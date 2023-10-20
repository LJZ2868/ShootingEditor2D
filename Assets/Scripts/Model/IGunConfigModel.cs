using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;

namespace ShootEditor2D
{
    public interface IGunConfigModel : IModel
    {
        GunConfigItem GetItemByName(string name);
        public Dictionary<string, GunConfigItem> mItems { get; }
    }

    public class GunConfigItem :IController
    {
        //Ãû³Æ
        public string Name { get; set; }
        //Éä»÷¼ä¸ô
        public float originalFiringRate;
        //Ìîµ¯Ê±¼ä
        public float originalReloadSeconds;
        //Ç¹ÄÚ×Óµ¯
        public int BulletCountInGun { get; set; }
        //µ¯¼ÐÈÝÁ¿
        public int originalMaxClip;

        public float FiringRate => originalFiringRate * this.GetModel<IPlayerModel>().FiringRate.Value;
        public float ReloadSeconds => originalReloadSeconds * this.GetModel<IPlayerModel>().ReloadSeconds.Value;
        public int MaxClip => originalMaxClip * this.GetModel<IPlayerModel>().MaxClip.Value;

        public IIOC GetIOC()
        {
            return ShootEditor2D.Interface;
        }
    }

    public class GunConfigModel : AbstractIModel, IGunConfigModel
    {
        public Dictionary<string, GunConfigItem> mItems { get; } = new Dictionary<string, GunConfigItem>()
        {
            {"pistol",new GunConfigItem(){ Name="pistol",originalFiringRate=0.5f,originalReloadSeconds=1f,BulletCountInGun=10,originalMaxClip=10} },
            { "submachine",new GunConfigItem(){ Name="submachine",originalFiringRate=0.2f,originalReloadSeconds=0.8f,BulletCountInGun=20,originalMaxClip=20} },
            { "laser",new GunConfigItem(){ Name="laser",originalFiringRate=1f,originalReloadSeconds=1.5f,BulletCountInGun=5,originalMaxClip=5} }
        };

        public GunConfigItem GetItemByName(string name)
        {
            return mItems[name];
        }

        protected override void OnInit()
        {
            
        }
    }
}
