using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;
using UnityEngine.Tilemaps;

namespace ShootEditor2D
{
    public class Door : MonoBehaviour,IController
    {
        private float timer;

        public bool isClear;

        public IIOC GetIOC()
        {
            return ShootEditor2D.Interface;
        }

        private void Awake()
        {
            isClear = false;
            timer = 0f;
            this.GetSystem<ITimeSystem>().AddDelayTask(1f, () =>
            {
                isClear = true;
                //初始化时没有敌人
                this.GetSystem<ITimeSystem>().AddDelayTask(1f, () =>
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);
                    gameObject.SetActive(false);
                });
            });

            this.RegisterEvent<KillEnemyEvent>(e =>
            {
                //该场景中没敌人时开门
                EnemyClaer();
            }).UnRegisterWhenGameObjectOnDestroy(gameObject);

            //生成敌人时生成门
            this.RegisterEvent<CreateEnemyEvent>(e =>
            {
                
                isClear = false;
                timer = 0;
                Debug.Log(e.name);
                if(e.name==transform.parent.name)
                    gameObject.SetActive(true);

            }).UnRegisterWhenGameObjectOnDestroy(gameObject);
        }

        //门失活且上移
        void EnemyClaer()
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                isClear = true;
                this.GetSystem<ITimeSystem>().AddDelayTask(1f, () =>
                {
                    gameObject.SetActive(false);
                    transform.position = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);
                });

                if (gameObject.activeSelf)
                {
                    //自动保存
                    var levelDates = this.GetModel<ILevelModel>().LevelDates;
                    this.SendCommand(new SaveLevelDateCommand(transform.parent.name));
                    this.GetSystem<ISaveLoadSystem>().SaveByJson(APPConst.LevelDate, levelDates);
                }
            }
        }

        private void FixedUpdate()
        {
            if (isClear)
            {
                timer += Time.deltaTime;
                GetComponent<Tilemap>().color = new Color(1, 1, 1, 1 - timer);
            }
            else
            {
                GetComponent<Tilemap>().color = new Color(1, 1, 1, 1);

                if (transform.position.y > 0)
                {
                    GetComponent<Rigidbody2D>().velocity = Vector2.down * 10f;
                }
                else
                {
                    GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    transform.localPosition = Vector3.zero;
                }
            }
        }
    }
}