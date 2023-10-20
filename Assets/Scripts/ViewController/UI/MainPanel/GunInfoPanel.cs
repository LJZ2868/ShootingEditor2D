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

            //��ʼֵ
            var mGun = this.GetSystem<IGunSystem>().CurrentGun;
            var gunItem = this.GetModel<IGunConfigModel>().GetItemByName(mGun.Name.Value);
            state.text = mGun.GunState.Value.ToString();
            gun.text = mGun.Name.Value;
            bulletCount.text = gunItem.BulletCountInGun.ToString();


            //��ǹʱ״̬��ǹ���ӵ��仯
            this.RegisterEvent<ShootingEvent>(e =>
            {
                state.text= this.GetSystem<IGunSystem>().CurrentGun.GunState.Value.ToString();
                bulletCount.text = this.SendQuery(new BulletCountQuery(this.GetSystem<IGunSystem>().CurrentGun.Name.Value)).ToString();
                //bulletCount.text = this.GetModel<IGunConfigModel>().GetItemByName(this.GetSystem<IGunSystem>().CurrentGun.Name.Value).BulletCountInGun.ToString();
            }).UnRegisterWhenGameObjectOnDestroy(gameObject);

            //��ǹʱǹ����ͬǹ�ӵ������仯
            this.RegisterEvent<ChangeGunEvent>(e =>
            {                
                gun.text = this.GetSystem<IGunSystem>().CurrentGun.Name.Value;
                bulletCount.text = this.SendQuery(new BulletCountQuery(this.GetSystem<IGunSystem>().CurrentGun.Name.Value)).ToString();
            }).UnRegisterWhenGameObjectOnDestroy(gameObject);

            //�ʱ״̬��ǹ���ӵ������仯
            this.RegisterEvent<ReloadingEvent>(e =>
            {
                state.text = this.GetSystem<IGunSystem>().CurrentGun.GunState.Value.ToString();
                bulletCount.text = this.SendQuery(new BulletCountQuery(this.GetSystem<IGunSystem>().CurrentGun.Name.Value)).ToString();
            }).UnRegisterWhenGameObjectOnDestroy(gameObject);
        }

    }
}
