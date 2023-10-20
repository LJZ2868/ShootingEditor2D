using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;
using System;

namespace ShootEditor2D
{
    public interface IObjectUIFollowSystem : ISystem
    {
        LinkedList<UIInfo> mUIInfos { get; set; }
        UIInfo UIFollowWithObject(Transform transform,Transform target, Vector3 offsetPos);
        void RemoveUIFollow(UIInfo uiInfo);
    }

    public class UIInfo
    {
        public Transform Transf { get; set; }
        public Vector3 OffsetPos { get; set; }
        public Transform Target { get; set; }
    }

    public class ObjectUIFollowSystem : AbstractISystem, IObjectUIFollowSystem
    {
        public class ObjectPositionUpdate : MonoBehaviour
        {
            public Action OnUpdate;

            private void Update()
            {
                OnUpdate?.Invoke();
            }
        }

        protected override void OnInit()
        {
            //创建一个GameObject管理
            var mObjectPositionUpdate = new GameObject(nameof(ObjectPositionUpdate));
            var updateBehaviour = mObjectPositionUpdate.AddComponent<ObjectPositionUpdate>();
            UnityEngine.Object.DontDestroyOnLoad(mObjectPositionUpdate);

            updateBehaviour.OnUpdate += OnUpdate;
        }

        public LinkedList<UIInfo> mUIInfos { get; set; } = new LinkedList<UIInfo>();

        private void OnUpdate()
        {
            if (mUIInfos.Count > 0)
            {

                //有任务
                var currentNode = mUIInfos.First;
                while (currentNode != null)
                {
                    var ui = currentNode.Value;
                    //记录下个节点
                    var nextNode = currentNode.Next;


                    if (!ui.Transf.gameObject.activeSelf || !ui.Target.gameObject.activeSelf)
                    {
                        //Debug.Log(mUIInfos.Count);
                        //Debug.Log(ui.Transf.name);
                        mUIInfos.Remove(currentNode);
                        //Debug.Log("remove");
                    }
                    else
                    {
                        var pos = Camera.main.WorldToScreenPoint(ui.Target.position + ui.OffsetPos);
                        //分辨率调整
                        float scale = 1920f / Screen.width;
                        //Debug.Log("pos"+ pos);
                        Vector3 targetPosition = new Vector3((pos.x - Screen.width / 2) * scale, (pos.y - Screen.height / 2) * scale, pos.z);
                        ui.Transf.localPosition = targetPosition;

                        //Debug.Log(targetPosition);
                    }
                    currentNode = nextNode;
                }
            }
        }

        public UIInfo UIFollowWithObject(Transform transform, Transform target, Vector3 offsetPos)
        {
            var ui = new UIInfo
            {
                Transf = transform,
                OffsetPos = offsetPos,
                Target = target
            };

            mUIInfos.AddLast(ui);

            return ui;
        }

        public void RemoveUIFollow(UIInfo uiInfo)
        {
            mUIInfos.Remove(uiInfo);
        }
    }
}