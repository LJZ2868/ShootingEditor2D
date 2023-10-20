using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;

namespace ShootEditor2D
{
    public class ELaser : EnemyBulletProperty
    {
        [SerializeField]
        private AnimationCurve animationCurve;

        protected override EnemyBulletInfo BulletInfo { get ; set ; }

        //����Ŀ��
        private bool isLocked = false;

        private float timer;

        private bool isCanAttack;

        //RaycastHit2D hit2D;

        protected override void Awake()
        {
            var info = this.GetSystem<IEnemyBulletSystem>().EnemyBulletInfos[1];
            BulletInfo = new EnemyBulletInfo(info.Name,info.Damage);

            base.Awake();
            isCanAttack = false;
            //enemyBulletItem = this.GetModel<IEnemyGunModel>().GetGunItemByName(BulletInfo.Name);
        }

        private void FixedUpdate()
        {
            if (!isLocked)
            {
                //δ����ʱ���ٽ�ɫ
                //���Ŀ�귽��
                Vector3 targetDic = player.transform.position - transform.position;
                //��������y����ת��Ŀ�귽�����ת��Ԫ��
                Quaternion rotation = Quaternion.FromToRotation(transform.up, targetDic);

                //transform.right = Quaternion.Lerp(Quaternion.identity, rotation, 1f) * transform.right;
                //�������y��ָ�� ��ת��Ԫ��Ӧ�õ�y��ķ���
                transform.up = rotation * transform.up;
            }

            if (isCanAttack)
            {
                Vector3 to = new Vector3(0.02f, 10f, 1);
                transform.localScale = Vector3.LerpUnclamped(transform.localScale, to,animationCurve.Evaluate(timer/ enemyModelitem.Flight));
                timer += Time.deltaTime;
                if (timer > enemyModelitem.Flight)
                {
                    //transform.localScale = Vector3.Lerp(transform.localScale,Vector3.zero, animationCurve.Evaluate(timer / enemyBulletItem.Flight));
                    timer = 0;
                    OnPush();
                }
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player")&&isCanAttack)
            {
                this.SendCommand(new HurtPlayerCommand(BulletInfo.Damage));
                
                //gameObject.SetActive(false);
            }
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
        }

        public override void Attack()
        {
            isLocked = false;
            isCanAttack = false;
            
            //һ�������
            this.GetSystem<ITimeSystem>().AddDelayTask(1f, () =>
            {
                isLocked = true;
            });

            //�ǹ���״̬�´�С
            transform.localScale = new Vector3(0.005f, 10f, 1);

            this.GetSystem<ITimeSystem>().AddDelayTask(enemyModelitem.DelayeTime, () =>
            {
                isCanAttack = true;
                //����״̬�´�С
                //����0.5s
                //Invoke(nameof(OnPush), enemyBulletItem.Flight);
            });
        }
    }
}
