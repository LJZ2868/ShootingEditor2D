using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShootEditor2D
{
    public class EnemyPhysicsCheck : MonoBehaviour
    {

        //是否接触地面
        public bool isGround;
        //修改检测是否接触地面的圆心的位置
        public Vector2 drawGroundPosition;
        //检测半径
        public float groundCheckRadius;
        //需要检测的图层
        public LayerMask layer;



        private void FixedUpdate()
        {
            Check();
        }

        void Check()
        {
            //圆形检测是否接触地面(圆心,半径,检测图层)
            isGround = Physics2D.OverlapCircle((Vector2)transform.position + drawGroundPosition, groundCheckRadius, layer);
        }

        private void OnDrawGizmosSelected()
        {
            //检测圆形可视,可修改化
            Gizmos.DrawWireSphere((Vector2)transform.position + drawGroundPosition, groundCheckRadius);
        }
    }
}