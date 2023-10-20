using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FrameworkDesign
{
    public interface ITimeSystem : ISystem
    {
        //������¼��ǰ��ʱ��
        float CurrentSeconds { get; }
        //��ʱ���ί��
        void AddDelayTask(float seconds, Action onDelayFinish);
    }

    public class TimeSyetem : AbstractISystem, ITimeSystem
    {

        public class TimeSyetemUpdate : MonoBehaviour
        {
            public Action OnUpdate;

            private void Update()
            {
                OnUpdate?.Invoke();
            }
        }

        public float CurrentSeconds { get; private set; }

        //�洢��ʱ�����������
        private LinkedList<DelayTask> mDelayTasks = new LinkedList<DelayTask>();

        //��ʼ��
        public void AddDelayTask(float seconds, Action onDelayFinish)
        {
            var mDelayTask = new DelayTask
            {
                Seconds = seconds,
                OnFinsh = onDelayFinish,
                DelayTaskState = DelayTaskState.NoStart
            };

            //���������б�
            mDelayTasks.AddLast(mDelayTask);
        }

        protected override void OnInit()
        {
            //����һ��GameObject��¼ʱ��
            var mTimeSyetemUpdate = new GameObject(nameof(TimeSyetemUpdate));
            var updateBehaviour = mTimeSyetemUpdate.AddComponent<TimeSyetemUpdate>();
            UnityEngine.Object.DontDestroyOnLoad(mTimeSyetemUpdate);

            updateBehaviour.OnUpdate += OnUpdate;
        }

        void OnUpdate()
        {
            CurrentSeconds += Time.deltaTime;

            if (mDelayTasks.Count > 0)
            {
                //����ʱ����
                var currentNode = mDelayTasks.First;
                while (currentNode != null)
                {
                    //��¼�¸��ڵ�
                    var nextNode = currentNode.Next;
                    var delayTask = currentNode.Value;


                    if (delayTask.DelayTaskState == DelayTaskState.NoStart)
                    {
                        delayTask.DelayTaskState = DelayTaskState.Started;
                        //��¼��ʼʱ��
                        delayTask.StartSeconds = CurrentSeconds;
                        //�������ʱ��
                        delayTask.FinishSeconds = CurrentSeconds + delayTask.Seconds;
                    }
                    else if (delayTask.DelayTaskState == DelayTaskState.Started)
                    {
                        //��ʱ��ɺ�
                        if (delayTask.FinishSeconds <= CurrentSeconds)
                        {
                            delayTask.DelayTaskState = DelayTaskState.Finish;
                            //ִ����ʱ����
                            delayTask.OnFinsh?.Invoke();
                            //�Ϳ�
                            delayTask.OnFinsh = null;
                            //ɾ���ڵ�
                            mDelayTasks.Remove(currentNode);
                        }
                        
                    }
                    //�����¸��ڵ�
                    currentNode = nextNode;
                }
            }
        }
    }

    public class DelayTask
    {
        //��ʱʱ��
        public float Seconds { get; set; }
        //��ʱ����
        public Action OnFinsh { get; set; }

        //��¼��ʼʱ��
        public float StartSeconds { get; set; }
        //��¼���ʱ��
        public float FinishSeconds { get; set; }
        //״̬
        public DelayTaskState DelayTaskState { get; set; }
    }

    public enum DelayTaskState
    {
        NoStart,
        Started,
        Finish
    }
}