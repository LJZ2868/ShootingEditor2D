using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;
using System;

namespace ShootEditor2D
{
    public interface IPauseSystem : ISystem
    {
        //������¼��ǰ��ʱ��
        //float CurrentSeconds { get; }
        //����ʱ�� ����ί��
        BindableProperty<int> PauseCount { get; set; }
        bool IsPause { get; }

        void StartPauseAnimation(Transform transform,Vector3 originalPosition,Vector3 toPosition, float seconds,AnimationCurve animationCurve,bool isPause,Action onFinish);
        void StartPauseAnimation(Transform transform, Vector3 originalPosition, Vector3 toPosition, float seconds, bool isPause, Action onFinish);
    }

    public class PauseAnimation
    {
        //����������
        public Transform Transf { get; set; }
        //��ʼλ��
        public Vector3 OriginalPosition { get; set; }
        //��ֹλ��
        public Vector3 ToPosition { get; set; }
        //��¼����ʱ��
        public float Seconds { get; set; }
        //��¼��ʼʱ��
        public float StartSeconds { get; set; }
        //�Ƿ���ͣ
        public bool IsPause { get; set; }
        //������������
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

        //�洢���������
        private LinkedList<PauseAnimation> mAnimations = new LinkedList<PauseAnimation>();

        //������ͣ��(����0ʱ��ͣ)
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

            //���Ҫ��ͣ
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
            //�����б�
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
            //�����б�
            mAnimations.AddLast(animation);
        }

        protected override void OnInit()
        {
            //����һ��GameObject��¼ʱ��
            var mPauseSyetemUpdate = new GameObject(nameof(PauseSyetemUpdate));
            var updateBehaviour = mPauseSyetemUpdate.AddComponent<PauseSyetemUpdate>();
            UnityEngine.Object.DontDestroyOnLoad(mPauseSyetemUpdate);

            updateBehaviour.OnUpdate += OnUpdate;

            //��ͣ����(�ж��Ƿ���ͣ)
            PauseCount.RegisterOnValueChange(newValue =>
            {
                //Debug.Log(newValue);
                //������ʱ�����ͣ
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
                //������
                var currentNode = mAnimations.First;
                while (currentNode != null)
                {
                    var animation = currentNode.Value;

                    //��¼�¸��ڵ�
                    var nextNode = currentNode.Next;

                    //��ǣ���Ե����Բ�ֵ����Ч��
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
                        //ִ����ʱ����
                        animation.OnFinish?.Invoke();
                        //�Ϳ�
                        animation.OnFinish = null;
                        //ɾ���ڵ�
                        mAnimations.Remove(currentNode);
                    }

                    currentNode = nextNode;
                }
            }
        }

        
    }
}