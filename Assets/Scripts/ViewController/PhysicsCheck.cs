using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;
using UnityEditor;

namespace ShootEditor2D
{
    public class PhysicsCheck :MonoBehaviour
    {
        //是否接触地面
        public bool isGround;
        //是否接触墙面
        public bool isWall;
        //是否接触平台
        public bool isPlatform;

        public bool IsAttackTirgger;
        //检测是否接触地面的矩形的位置
        public Vector2 drawGroundPosition;
        //矩形的长宽
        public Vector2 checkBoxSize;
        //检测是否接触墙面的圆心位置
        public Vector2 drawWallPosition;
        //检测的圆的半径
        public float circleRadius;

        //攻击范围
        public float attackCircleRadius;
        public LayerMask Enemylayer;

        private void FixedUpdate()
        {
            Check();
        }

        

        void Check()
        {
            //圆形检测是否接触地面(圆心,半径,检测图层)
            //isGround = Physics2D.OverlapCircle((Vector2)transform.position + drawGroundPosition, groundCheckRadius, layer);
            isGround = Physics2D.OverlapBox((Vector2)transform.position + drawGroundPosition, checkBoxSize, 0, LayerMask.GetMask("Ground")) ;

            isPlatform= Physics2D.OverlapBox((Vector2)transform.position + drawGroundPosition, checkBoxSize, 0, LayerMask.GetMask("Platform"));

            isWall = Physics2D.OverlapCircle((Vector2)transform.position + drawWallPosition, circleRadius, LayerMask.GetMask("Ground")) ;

            IsAttackTirgger = Physics2D.OverlapCircle(transform.position, attackCircleRadius, Enemylayer);
        }

        private void OnDrawGizmosSelected()
        {
            //检测圆形可视,可修改化
            Gizmos.DrawWireCube((Vector2)transform.position + drawGroundPosition, checkBoxSize);

            Gizmos.DrawWireSphere((Vector2)transform.position + drawWallPosition, circleRadius);

            Gizmos.DrawWireSphere((Vector2)transform.position, attackCircleRadius);
            
        }

        [CustomEditor(typeof(PhysicsCheck))]
        public class PhysicsCheckInspector : Editor
        {

            bool checkGround;
            bool checkPlatform;
            bool checkWall;
            bool checkAttackTirgger;

            public override void OnInspectorGUI()
            {
                var check = target as PhysicsCheck;
                //DrawDefaultInspector();
                checkGround = EditorGUILayout.Toggle("是否检测地面",checkGround);


                if (checkGround)
                {
                    check.isGround = EditorGUILayout.Toggle(nameof(check.isGround), check.isGround);
                    check.drawGroundPosition = EditorGUILayout.Vector2Field("检测中心",check.drawGroundPosition);
                    check.checkBoxSize = EditorGUILayout.Vector2Field("检测长宽",check.checkBoxSize);
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    checkPlatform = EditorGUILayout.Toggle("是否检测平台", checkPlatform);
                }


                if (checkPlatform)
                {
                    check.isPlatform = EditorGUILayout.Toggle(nameof(check.isPlatform), check.isPlatform);
                }

                EditorGUILayout.Space();
                EditorGUILayout.Space();
                checkWall = EditorGUILayout.Toggle("是否检测墙壁", checkWall);
                if (checkWall)
                {
                    check.isWall = EditorGUILayout.Toggle(nameof(check.isWall), check.isWall);
                    check.drawWallPosition = EditorGUILayout.Vector2Field("检测中心", check.drawWallPosition);
                    check.circleRadius = EditorGUILayout.FloatField("检测半径", check.circleRadius);
                }
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                checkAttackTirgger = EditorGUILayout.Toggle("是否检碰撞攻击", checkAttackTirgger);
                if (checkAttackTirgger)
                {
                    check.IsAttackTirgger = EditorGUILayout.Toggle(nameof(check.IsAttackTirgger), check.IsAttackTirgger);
                    check.attackCircleRadius = EditorGUILayout.FloatField("检测半径", check.attackCircleRadius);
                    check.Enemylayer = EditorGUILayout.LayerField("检测图层", check.Enemylayer);
                }
                if (!EditorUtility.IsDirty(target))
                    EditorUtility.SetDirty(target);
            }


        }

    }
}