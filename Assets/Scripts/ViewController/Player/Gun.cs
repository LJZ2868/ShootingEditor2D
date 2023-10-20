using FrameworkDesign;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ShootEditor2D
{
    public class Gun : MonoBehaviour,IController
    {
        private Bullet mBullet;
        public static Laser mLaser;

        private GunInfo mGunInfo;

        public PlayerControl mInput;

        //射击方向
        public static Vector2 direction;

        //是否转向
        bool isTurn;

        private void Awake()
        {
            mInput = new PlayerControl();
            mBullet = transform.Find("Bullet").GetComponent<Bullet>();
            mLaser = transform.Find("Laser").GetComponent<Laser>();
            mInput.Player.ChangeGun.started += ChangeGun;
            mInput.Player.Reload.started += Reload;

            mGunInfo = this.GetSystem<IGunSystem>().CurrentGun;

            this.RegisterEvent<PlayerLookDirectionChangeEvent>(e =>
            {
                isTurn = e.lookDirection.x <= 0;
            });
           
        }

        private void Reload(InputAction.CallbackContext obj)
        {
            var gunItem = this.GetModel<IGunConfigModel>();
            //idel状态且子弹不满才能装弹
            if (mGunInfo.GunState.Value == GunState.Idle&&
                this.SendQuery(new BulletCountQuery(mGunInfo.Name.Value))!=gunItem.GetItemByName(mGunInfo.Name.Value).MaxClip)
            {
                this.SendCommand<ReloadingCommand>();
            }
        }

        private void ChangeGun(InputAction.CallbackContext obj)
        {
            //idel状态才能换枪
            if (mGunInfo.GunState.Value == GunState.Idle)
            {
                if (this.GetSystem<IGunSystem>().Guns[int.Parse(obj.control.name) - 1] != null)
                {
                    mGunInfo = this.GetSystem<IGunSystem>().CurrentGun = this.GetSystem<IGunSystem>().Guns[int.Parse(obj.control.name) - 1];

                    this.SendCommand<ChangeGunCommand>();
                }
            }
        }

        public void Shoot()
        {
            //idle状态或Hold状态且枪内有子弹才能射击
            if ((mGunInfo.GunState.Value == GunState.Idle || 
                mGunInfo.GunState.Value == GunState.Hold) &&
                this.SendQuery(new BulletCountQuery(mGunInfo.Name.Value)) > 0)
            {
                if (mGunInfo.GunType == GunType.Bullet)
                {
                    //实例化对象路径,实例化对象
                    PoolManager.Instance.GetObj("Game/Player/Bullet", mBullet.gameObject);
                    this.SendCommand<ShootingCommand>();
                    //this.SendCommand(new ShootingCommand(mGunInfo));
                }
                else if (mGunInfo.GunType == GunType.Laser)
                {
                    PoolManager.Instance.GetObj("Game/Player/Laser", mLaser.gameObject);
                    this.SendCommand<ShootingCommand>();
                }
            }
            //自动填弹
            else if (mGunInfo.GunState.Value == GunState.Idle && 
                this.SendQuery(new BulletCountQuery(mGunInfo.Name.Value)) == 0)
            {
                this.SendCommand<ReloadingCommand>();
            }
        }

        private void OnDestroy()
        {
            mGunInfo = null;
        }


        private void FixedUpdate()
        {
            //激光形武器发射时不能转向
            if (mGunInfo.GunState.Value != GunState.Shooting || mGunInfo.GunType != GunType.Laser)
            {
                //枪跟随鼠标旋转
                Vector3 mousePos = Input.mousePosition;
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, -Camera.main.transform.position.z));
                direction = (worldPos - transform.position).normalized;
                transform.rotation = isTurn ? Quaternion.FromToRotation(transform.localPosition, -direction) : Quaternion.FromToRotation(transform.localPosition, direction);
            }
        }

        public IIOC GetIOC()
        {
            return ShootEditor2D.Interface;
        }

        private void OnEnable()
        {
            mInput.Enable();
        }

        private void OnDisable()
        {
            mInput.Disable();
        }
    }
}