using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;

namespace ShootEditor2D
{
    public class Laser : BulletProperty
    {
        [SerializeField]
        private AnimationCurve animationCurve;

        private bool isCanAttack;
        private float timer;

        private List<Collider2D> colliders;

        Vector3 to1;
        Vector3 to2;

        protected override void Awake()
        {
            colliders = new List<Collider2D>();
            isCanAttack = true;
            timer = 0f;
            base.Awake();
        }

        private void FixedUpdate () 
        {
            transform.position = Gun.mLaser.transform.position;

            //前一半时间内宽度增大
            if (timer < (bullet.Flight) / 2)
                transform.localScale = Vector3.LerpUnclamped(transform.localScale, to1, animationCurve.Evaluate(timer / (bullet.Flight / 2)));
            else
            {
                transform.localScale = Vector3.LerpUnclamped(transform.localScale, to2, animationCurve.Evaluate((timer / (bullet.Flight / 2)) - 1));
            }
            timer += Time.deltaTime;
        }

        protected override void OnEnable()
        {
            //初始大小
            timer = 0;

            if (Player.LookDirection.x < 0)
            {
                to1 = new Vector3(-1.2f, 0.4f, 1);
                to2 = new Vector3(-1.2f, 0, 1);
                transform.localScale = new Vector3(-0.7f, 0.2f, 1);
            }
            else
            {
                to1 = new Vector3(1.2f, 0.4f, 1);
                to2 = new Vector3(1.2f, 0, 1);
                transform.localScale = new Vector3(0.7f, 0.2f, 1);
            }

            base.OnEnable();
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                if(!colliders.Contains(collision))
                    colliders.Add(collision);
                if (isCanAttack)
                {
                    isCanAttack = false;
                    //计算伤害(一次攻击8次伤害)
                    this.GetSystem<ITimeSystem>().AddDelayTask(bullet.Flight / 9, () =>
                    {
                        isCanAttack = true;
                        //Debug.Log(colliders.Count);
                        foreach (var item in colliders)
                        {
                            item.transform.GetComponent<EnemyProperty>().EnemyItem.HP.Value -= bullet.Damage;
                        }
                        colliders.Remove(collision);
                        //Debug.Log("damage");
                    });
                }

                if (collision.transform.GetComponent<EnemyProperty>().EnemyItem.HP.Value <= 0)
                {
                    //发送击杀敌人command(触发事件)
                    collision.gameObject.SetActive(false);
                    this.SendCommand<KillEnemyCommand>();
                }
            }
        }
    }
}