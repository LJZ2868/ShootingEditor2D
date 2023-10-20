using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;
using UnityEditor;

namespace ShootEditor2D
{
    public class PhysicsCheck :MonoBehaviour
    {
        //�Ƿ�Ӵ�����
        public bool isGround;
        //�Ƿ�Ӵ�ǽ��
        public bool isWall;
        //�Ƿ�Ӵ�ƽ̨
        public bool isPlatform;

        public bool IsAttackTirgger;
        //����Ƿ�Ӵ�����ľ��ε�λ��
        public Vector2 drawGroundPosition;
        //���εĳ���
        public Vector2 checkBoxSize;
        //����Ƿ�Ӵ�ǽ���Բ��λ��
        public Vector2 drawWallPosition;
        //����Բ�İ뾶
        public float circleRadius;

        //������Χ
        public float attackCircleRadius;
        public LayerMask Enemylayer;

        private void FixedUpdate()
        {
            Check();
        }

        

        void Check()
        {
            //Բ�μ���Ƿ�Ӵ�����(Բ��,�뾶,���ͼ��)
            //isGround = Physics2D.OverlapCircle((Vector2)transform.position + drawGroundPosition, groundCheckRadius, layer);
            isGround = Physics2D.OverlapBox((Vector2)transform.position + drawGroundPosition, checkBoxSize, 0, LayerMask.GetMask("Ground")) ;

            isPlatform= Physics2D.OverlapBox((Vector2)transform.position + drawGroundPosition, checkBoxSize, 0, LayerMask.GetMask("Platform"));

            isWall = Physics2D.OverlapCircle((Vector2)transform.position + drawWallPosition, circleRadius, LayerMask.GetMask("Ground")) ;

            IsAttackTirgger = Physics2D.OverlapCircle(transform.position, attackCircleRadius, Enemylayer);
        }

        private void OnDrawGizmosSelected()
        {
            //���Բ�ο���,���޸Ļ�
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
                checkGround = EditorGUILayout.Toggle("�Ƿ������",checkGround);


                if (checkGround)
                {
                    check.isGround = EditorGUILayout.Toggle(nameof(check.isGround), check.isGround);
                    check.drawGroundPosition = EditorGUILayout.Vector2Field("�������",check.drawGroundPosition);
                    check.checkBoxSize = EditorGUILayout.Vector2Field("��ⳤ��",check.checkBoxSize);
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    checkPlatform = EditorGUILayout.Toggle("�Ƿ���ƽ̨", checkPlatform);
                }


                if (checkPlatform)
                {
                    check.isPlatform = EditorGUILayout.Toggle(nameof(check.isPlatform), check.isPlatform);
                }

                EditorGUILayout.Space();
                EditorGUILayout.Space();
                checkWall = EditorGUILayout.Toggle("�Ƿ���ǽ��", checkWall);
                if (checkWall)
                {
                    check.isWall = EditorGUILayout.Toggle(nameof(check.isWall), check.isWall);
                    check.drawWallPosition = EditorGUILayout.Vector2Field("�������", check.drawWallPosition);
                    check.circleRadius = EditorGUILayout.FloatField("���뾶", check.circleRadius);
                }
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                checkAttackTirgger = EditorGUILayout.Toggle("�Ƿ����ײ����", checkAttackTirgger);
                if (checkAttackTirgger)
                {
                    check.IsAttackTirgger = EditorGUILayout.Toggle(nameof(check.IsAttackTirgger), check.IsAttackTirgger);
                    check.attackCircleRadius = EditorGUILayout.FloatField("���뾶", check.attackCircleRadius);
                    check.Enemylayer = EditorGUILayout.LayerField("���ͼ��", check.Enemylayer);
                }
                if (!EditorUtility.IsDirty(target))
                    EditorUtility.SetDirty(target);
            }


        }

    }
}