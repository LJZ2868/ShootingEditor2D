using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShootEditor2D
{
    [RequireComponent(typeof(LineRenderer))]
    public class MoveTrack : MonoBehaviour
    {
        LineRenderer lineRenderer;

        // 使用List储存前几个位置的坐标点
        List<Vector3> pointList = new List<Vector3>();

        // 需要记录的坐标点的数量
        int pointSize = 20;

        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();

            InitPointList();

            lineRenderer.positionCount = pointSize;
        }

        private void FixedUpdate () 
        {
            if (transform.GetComponent<Player>().isDodge)
            {
                UpdatePointList();
                DrawLine();
            }
            else
            {
                InitPointList();
                DrawLine();
            }
            
                //lineRenderer.
        }

        //初始化坐标列表
        void InitPointList()
        {
            pointList.Clear();
            for (int i = 0; i < pointSize; i++)
            {
                pointList.Add(transform.position);
            }
            
        }

        //更新坐标列表
        void UpdatePointList()
        {
            //移除最后一个坐标点
            pointList.RemoveAt(pointSize - 1);
            //添加新的坐标点
            pointList.Insert(0, transform.position);
        }

        void DrawLine()
        {
            for (int i = 0; i < pointSize; i++)
            {
                lineRenderer.SetPosition(i,pointList[i]);
            }
        }
    }
}