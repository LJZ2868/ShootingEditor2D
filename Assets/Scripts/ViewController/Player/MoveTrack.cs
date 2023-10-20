using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShootEditor2D
{
    [RequireComponent(typeof(LineRenderer))]
    public class MoveTrack : MonoBehaviour
    {
        LineRenderer lineRenderer;

        // ʹ��List����ǰ����λ�õ������
        List<Vector3> pointList = new List<Vector3>();

        // ��Ҫ��¼������������
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

        //��ʼ�������б�
        void InitPointList()
        {
            pointList.Clear();
            for (int i = 0; i < pointSize; i++)
            {
                pointList.Add(transform.position);
            }
            
        }

        //���������б�
        void UpdatePointList()
        {
            //�Ƴ����һ�������
            pointList.RemoveAt(pointSize - 1);
            //����µ������
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