using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;
using System;

namespace ShootEditor2D
{
    public interface IPauseSystem : ISystem
    {
        //持续记录当前的时间
        //float CurrentSeconds { get; }
        //持续时间 动画委托
        BindableProperty<int> PauseCount { get; set; }
        bool IsPause { get; }

        void StartPauseAnimation(Transform transform,Vector3 originalPosition,Vector3 toPosition, float seconds,AnimationCurve animationCurve,bool isPause,Action onFinish);
        void StartPauseAnimation(Transform transform, Vector3 originalPosition, Vector3 toPosition, float seconds, bool isPause, Action onFinish);
    }

    public class PauseAnimation
    {
        //动画的物体
        public Transform Transf { get; set; }
        //初始位置
        public Vector3 OriginalPosition { get; set; }
        //终止位置
        public Vector3 ToPosition { get; set; }
        //记录持续时间
        public float Seconds { get; set; }
        //记录开始时间
        public float StartSeconds { get; set; }
        //是否暂停
        public bool IsPause { get; set; }
        //动画结束任务
        public Action OnFinish { get; set; }

        public AnimationCurve Curve { get; set; }
    }

    public class PauseSystem : AbstractISystem, IPauseSystem
    {
        public class PauseSyetemUpdate : MonoBehaviour
        {
            public Action OnUpdate;

            private void Update()
            {
                OnUpdate?.Invoke();
            }
        }

        //存储任务的链表
        private LinkedList<PauseAnimation> mAnimations = new LinkedList<PauseAnimation>();

        //管理暂停数(大于0时暂停)
        public BindableProperty<int> PauseCount { get; set; } = new BindableProperty<int>() { Value = 0 };
        public bool IsPause { get; private set; }

        public void StartPauseAnimation(Transform transform, Vector3 originalPosition, Vector3 toPosition,float seconds,AnimationCurve animationCurve, bool isPause, Action onFinish)
        {
            var animation = new PauseAnimation
            {
                Transf = transform,
                OriginalPosition = originalPosition,
                ToPosition = toPosition,
                Seconds = seconds,
                Curve = animationCurve,
                IsPause = isPause,
                StartSeconds = Time.realtimeSinceStartup,
                OnFinish = onFinish,
            };

            //如果要暂停
            if (animation.IsPause)
            {
                PauseCount.Value++;
            }
            else
            {
                PauseCount.Value--;
                if (PauseCount.Value < 0)
                {
                    PauseCount.Value = 0;
                }
                    
            }
            //加入列表
            mAnimations.AddLast(animation);
        }

        public void StartPauseAnimation(Transform transform, Vector3 originalPosition, Vector3 toPosition, float seconds, bool isPause, Action onFinish)
        {
            var animation = new PauseAnimation
            {
                Transf = transform,
                OriginalPosition = originalPosition,
                ToPosition = toPosition,
                Seconds = seconds,
                IsPause=isPause,
                StartSeconds = Time.realtimeSinceStartup,
                OnFinish = onFinish,
            };
            //加入列表
            mAnimations.AddLast(animation);
        }

        protected override void OnInit()
        {
            //创建一个GameObject记录时间
            var mPauseSyetemUpdate = new GameObject(nameof(PauseSyetemUpdate));
            var updateBehaviour = mPauseSyetemUpdate.AddComponent<PauseSyetemUpdate>();
            UnityEngine.Object.DontDestroyOnLoad(mPauseSyetemUpdate);

            updateBehaviour.OnUpdate += OnUpdate;

            //暂停参数(判断是否暂停)
            PauseCount.RegisterOnValueChange(newValue =>
            {
                //Debug.Log(newValue);
                //等于零时解除暂停
                if (newValue <= 0)
                {
                    IsPause = false;
                    Time.timeScale = 1;
                }
                else
                {
                    IsPause = true;
                    Time.timeScale = 0;
                }
            });
        }

        private void OnUpdate()
        {
            if (mAnimations.Count > 0)
            {
                //有任务
                var currentNode = mAnimations.First;
                while (currentNode != null)
                {
                    var animation = currentNode.Value;

                    //记录下个节点
                    var nextNode = currentNode.Next;

                    //非牵制性的线性差值动画效果
                    if (Time.realtimeSinceStartup - animation.StartSeconds <= animation.Seconds && Time.realtimeSinceStartup - animation.StartSeconds > 0)
                    {
                        if (animation.Curve != null)
                        {
                            animation.Transf.localPosition = Vector3.LerpUnclamped(animation.OriginalPosition, animation.ToPosition, animation.Curve.Evaluate((Time.realtimeSinceStartup - animation.StartSeconds) / animation.Seconds));
                        }
                        else
                        {
                            animation.Transf.localPosition = Vector3.Lerp(animation.OriginalPosition, animation.ToPosition, (Time.realtimeSinceStartup - animation.StartSeconds) / animation.Seconds);
                        }
                        
                    }
                    else
                    {
                        //执行延时任务
                        animation.OnFinish?.Invoke();
                        //滞空
                        animation.OnFinish = null;
                        //删除节点
                        mAnimations.Remove(currentNode);
                    }

                    currentNode = nextNode;
                }
            }
        }

        
    }
}