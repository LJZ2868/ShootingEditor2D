using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShootEditor2D
{
    public class EnemyPhysicsCheck : MonoBehaviour
    {

        //�Ƿ�Ӵ�����
        public bool isGround;
        //�޸ļ���Ƿ�Ӵ������Բ�ĵ�λ��
        public Vector2 drawGroundPosition;
        //���뾶
        public float groundCheckRadius;
        //��Ҫ����ͼ��
        public LayerMask layer;



        private void FixedUpdate()
        {
            Check();
        }

        void Check()
        {
            //Բ�μ���Ƿ�Ӵ�����(Բ��,�뾶,���ͼ��)
            isGround = Physics2D.OverlapCircle((Vector2)transform.position + drawGroundPosition, groundCheckRadius, layer);
        }

        private void OnDrawGizmosSelected()
        {
            //���Բ�ο���,���޸Ļ�
            Gizmos.DrawWireSphere((Vector2)transform.position + drawGroundPosition, groundCheckRadius);
        }
    }
}