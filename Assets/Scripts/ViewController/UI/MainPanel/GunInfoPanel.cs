using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;
using TMPro;

namespace ShootEditor2D
{
    public class GunInfoPanel : BaseUIPanel,IController
    {
        TextMeshProUGUI state;
        TextMeshProUGUI gun;
        TextMeshProUGUI bulletCount;

        public IIOC GetIOC()
        {
            return ShootEditor2D.Interface;
        }

        private void Awake()
        {
            state = transform.Find("State").GetComponent<TextMeshProUGUI>();
            gun = transform.Find("Gun").GetComponent<TextMeshProUGUI>();
            bulletCount = transform.Find("BulletCount").GetComponent<TextMeshProUGUI>();

            //初始值
            var mGun = this.GetSystem<IGunSystem>().CurrentGun;
            var gunItem = this.GetModel<IGunConfigModel>().GetItemByName(mGun.Name.Value);
            state.text = mGun.GunState.Value.ToString();
            gun.text = mGun.Name.Value;
            bulletCount.text = gunItem.BulletCountInGun.ToString();


            //开枪时状态及枪内子弹变化
            this.RegisterEvent<ShootingEvent>(e =>
            {
                state.text= this.GetSystem<IGunSystem>().CurrentGun.GunState.Value.ToString();
                bulletCount.text = this.SendQuery(new BulletCountQuery(this.GetSystem<IGunSystem>().CurrentGun.Name.Value)).ToString();
                //bulletCount.text = this.GetModel<IGunConfigModel>().GetItemByName(this.GetSystem<IGunSystem>().CurrentGun.Name.Value).BulletCountInGun.ToString();
            }).UnRegisterWhenGameObjectOnDestroy(gameObject);

            //换枪时枪及不同枪子弹数量变化
            this.RegisterEvent<ChangeGunEvent>(e =>
            {                
                gun.text = this.GetSystem<IGunSystem>().CurrentGun.Name.Value;
                bulletCount.text = this.SendQuery(new BulletCountQuery(this.GetSystem<IGunSystem>().CurrentGun.Name.Value)).ToString();
            }).UnRegisterWhenGameObjectOnDestroy(gameObject);

            //填弹时状态及枪内子弹数量变化
            this.RegisterEvent<ReloadingEvent>(e =>
            {
                state.text = this.GetSystem<IGunSystem>().CurrentGun.GunState.Value.ToString();
                bulletCount.text = this.SendQuery(new BulletCountQuery(this.GetSystem<IGunSystem>().CurrentGun.Name.Value)).ToString();
            }).UnRegisterWhenGameObjectOnDestroy(gameObject);
        }

    }
}
