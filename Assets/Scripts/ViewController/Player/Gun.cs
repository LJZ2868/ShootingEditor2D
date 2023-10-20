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

        //�������
        public static Vector2 direction;

        //�Ƿ�ת��
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
            //idel״̬���ӵ���������װ��
            if (mGunInfo.GunState.Value == GunState.Idle&&
                this.SendQuery(new BulletCountQuery(mGunInfo.Name.Value))!=gunItem.GetItemByName(mGunInfo.Name.Value).MaxClip)
            {
                this.SendCommand<ReloadingCommand>();
            }
        }

        private void ChangeGun(InputAction.CallbackContext obj)
        {
            //idel״̬���ܻ�ǹ
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
            //idle״̬��Hold״̬��ǹ�����ӵ��������
            if ((mGunInfo.GunState.Value == GunState.Idle || 
                mGunInfo.GunState.Value == GunState.Hold) &&
                this.SendQuery(new BulletCountQuery(mGunInfo.Name.Value)) > 0)
            {
                if (mGunInfo.GunType == GunType.Bullet)
                {
                    //ʵ��������·��,ʵ��������
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
            //�Զ��
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
            //��������������ʱ����ת��
            if (mGunInfo.GunState.Value != GunState.Shooting || mGunInfo.GunType != GunType.Laser)
            {
                //ǹ���������ת
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