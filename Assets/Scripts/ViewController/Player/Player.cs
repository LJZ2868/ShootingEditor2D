using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using FrameworkDesign;
using UnityEngine.UI;

namespace ShootEditor2D
{
    public class Player : MonoSingleton<Player>, IController
    {
        Rigidbody2D mRigidbody2d;
        PhysicsCheck mCheck;
        
        Gun mGun;

        //输入系统
        public PlayerControl mInput;
        //移动方向
        public Vector2 InputDirection 
        { 
            get 
            { 
                return mInput.Player.Move.ReadValue<Vector2>(); 
            } 
        }

        //面朝方向
        public static Vector2 LookDirection;

        //跳跃力
        private float mJumpForce = 15f;
        //蹬墙跳时间
        private float wallJumpTime=0.2f;
        //能否控制
        public bool isCanControl = true;
        //能否蓄力
        private bool isCanHold = false;
        //当前跳跃力
        public float CurrentJumpForce 
        {
            get 
            {
                return mJumpForce;
            }
        }

        //当前速度
        public float CurrentMoveSpeed
        {
            get 
            {
                return this.GetModel<IPlayerModel>().Speed.Value;
            }
        }


        public bool isDodgeCD = false;
        public bool isDodge = false;

        public BindableProperty<float> holdTime = new BindableProperty<float>() { Value = 0f };
        public BindableProperty<float> dodgeCD = new BindableProperty<float>() { Value = 5f };


        //LevelDate date;

        protected override void Awake()
        {

            base.Awake();
            mRigidbody2d = GetComponent<Rigidbody2D>();
            mCheck = GetComponent<PhysicsCheck>();
            mGun = transform.Find("Gun").GetComponent<Gun>();
            mInput = new PlayerControl();

            this.GetModel<IPlayerModel>().IsInvincible = false;

            mInput.Player.Jump.started += Jump;
            mInput.Player.Move.performed += Move_performed;

            mInput.Player.Hold.performed += Hold_performed;
            mInput.Player.Hold.canceled += Hold_canceled;
            mInput.Player.Hold.started += Hold_started;

            mInput.Player.Dodge.started += Dodge_started;
            //CheckCharacter();


            //初始化保存关卡数据
            //this.GetSystem<ITimeSystem>().AddDelayTask(1, () =>
            //{
            //    date = new LevelDate()
            //    {
            //        Name = name,
            //        Path = "Game/Player/Player",
            //        Position = transform.position,
            //        Tag = "Player"
            //    };
            //    this.GetModel<ILevelModel>().SaveDate(date);
            //});

            //this.RegisterEvent<SaveLevelDateEvent>(e =>
            //{
            //    UpdateLevelDate();
            //});
        }

        //void UpdateLevelDate()
        //{
        //    date.Position = transform.position;
        //    //Debug.Log(name);
        //    //this.GetModel<ILevelModel>().SaveDate(date);
        //}

        private void Dodge_started(InputAction.CallbackContext obj)
        {
            if (this.GetModel<IPlayerModel>().IsCanDodge.Value && 
                !isDodgeCD && 
                !this.GetSystem<IPauseSystem>().IsPause) 
            {

                dodgeCD.Value = 0;
                //闪避时不能控制
                isCanControl = false;

                this.SendCommand<DodgeCommand>();

                //闪避时间
                isDodge = true;
                this.GetSystem<ITimeSystem>().AddDelayTask(0.3f, () =>
                {
                    isDodge = false;
                    mRigidbody2d.velocity = Vector2.zero;
                    isCanControl = true;
                    this.GetModel<IPlayerModel>().IsInvincible = false;
                    LookDirectionChange();
                });
                //cd计算
                isDodgeCD = true;
                this.GetSystem<ITimeSystem>().AddDelayTask(this.GetModel<IPlayerModel>().MaxDodgeCD.Value,() =>
                {
                    isDodgeCD = false;
                });

                transform.Find("Canvas").Find("DodgeCD").GetComponent<Image>().color = Color.black;
            }
        }

        private void Hold_started(InputAction.CallbackContext obj)
        {
            var mGunInfo = this.GetSystem<IGunSystem>().CurrentGun;
            //当前枪为激光,有子弹且处于Idle状态时才能蓄力
            if (mGunInfo.GunType == GunType.Laser &&
                this.SendQuery(new BulletCountQuery(mGunInfo.Name.Value)) > 0 &&
                mGunInfo.GunState.Value == GunState.Idle)
            {
                this.GetSystem<IGunSystem>().CurrentGun.GunState.Value = GunState.Hold;
                isCanHold = true;
            }
            else
            {
                isCanHold = false;
            }
        }

        private void Hold_canceled(InputAction.CallbackContext obj)
        {
            if (this.GetSystem<IGunSystem>().CurrentGun.GunType == GunType.Laser)
            {
                holdTime.Value = 0f;
                this.GetSystem<IGunSystem>().CurrentGun.GunState.Value = GunState.Idle;
                isCanHold = false;
            }
        }

        private void Hold_performed(InputAction.CallbackContext obj)
        {
            if (this.GetSystem<IGunSystem>().CurrentGun.GunType == GunType.Laser&&
                holdTime.Value>=this.GetModel<IPlayerModel>().MaxHoldTime.Value)
            {
                Debug.Log("Hold:" + holdTime.Value);
                //发射激光
                mGun?.Shoot();

                transform.Find("Canvas").Find("HoldTime").GetComponent<Image>().color = Color.black;
                holdTime.Value = 0f;
                isCanHold = false;
            }
        }

        private void Move_performed(InputAction.CallbackContext obj)
        {
            //Debug.Log(obj.control.name);

            //激光形武器发射时不能转向
            var mGunInfo =  this.GetSystem<IGunSystem>().CurrentGun;
            if ((mGunInfo.GunState.Value != GunState.Shooting || mGunInfo.GunType != GunType.Laser)&&
                !this.GetSystem<IPauseSystem>().IsPause)
            {

                LookDirectionChange();
            }
           
        }


        void LookDirectionChange()
        {
            //转向
            if (InputDirection.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
                mCheck.drawWallPosition.x = MathF.Abs(mCheck.drawWallPosition.x);
            }
            else if (InputDirection.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                mCheck.drawWallPosition.x = -MathF.Abs(mCheck.drawWallPosition.x);
            }

            if (LookDirection.x != transform.localScale.x)
            {
                LookDirection = transform.localScale;
                this.SendCommand(new PlayerLookDirectionChangeCommand(LookDirection));
            }
        }

        private void Fire()
        {
            //子弹型攻击方式发射
            if (mInput.Player.Fire.IsPressed()&& this.GetSystem<IGunSystem>().CurrentGun.GunType!=GunType.Laser)
                mGun?.Shoot();
        }

        private void Jump(InputAction.CallbackContext obj)
        {
            if (isCanControl)
            {
                if ((mCheck.isGround || mCheck.isPlatform) && 
                    !this.GetSystem<IPauseSystem>().IsPause)
                {
                    if (InputDirection.y >= 0)
                        mRigidbody2d.AddForce(Vector2.up * CurrentJumpForce, ForceMode2D.Impulse);
                    else
                    {
                        if (mCheck.isPlatform)
                            //设为Trigger(下跳)
                            GetComponent<Collider2D>().isTrigger = true;
                    }
                }
                else if (mCheck.isWall && 
                    InputDirection.y >= 0)
                {
                    //蹬墙跳时间
                    isCanControl = false;
                    if (LookDirection.x > 0)
                        mRigidbody2d.velocity = Vector2.left * 5 + Vector2.up * 15;
                    else
                        mRigidbody2d.velocity = Vector2.right * 5 + Vector2.up * 15;

                    this.GetSystem<ITimeSystem>().AddDelayTask(wallJumpTime, () =>
                    {
                        isCanControl = true;
                    });
                }
            }
        }

        
        void Move()
        {
            if (isCanControl)
                mRigidbody2d.velocity = new Vector2(InputDirection.x * CurrentMoveSpeed * Time.deltaTime, mRigidbody2d.velocity.y);
        }

        void Dodge()
        {
            if (isDodge)
            {
                mRigidbody2d.velocity = new Vector2(LookDirection.x * CurrentMoveSpeed * Time.deltaTime * 6, 0);
            }
        }

        private void FixedUpdate () 
        {
            Fire();
            Move();
            Dodge();

            if (isCanHold)
            {
                holdTime.Value += Time.deltaTime;
            }

            if (dodgeCD.Value < this.GetModel<IPlayerModel>().MaxDodgeCD.Value)
            {
                dodgeCD.Value += Time.deltaTime;
            }
        }

        public IIOC GetIOC()
        {
            return ShootEditor2D.Interface;
        }

        //用inputSystem要写input.Enable()默认为Disable
        private void OnEnable()
        {
            mInput.Enable();
        }

        //private void OnDisable()
        //{
        //    mInput.Disable();
        //}
    }
}