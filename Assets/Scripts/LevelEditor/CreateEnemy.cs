using FrameworkDesign;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace ShootEditor2D
{
    public class CreateEnemy : MonoBehaviour,IController
    {




        private void Start()
        {
            if (transform.parent.GetComponent<LevelDateItem>().Date.IsRoomClear == true)
            {
                gameObject.SetActive(false);
            }
        }

        public IIOC GetIOC()
        {
            return ShootEditor2D.Interface;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                //Debug.Log("create");
                //collision.GetComponent<Player>().isDodgeCD = true;

                //延迟0.3s生成enemy
                this.GetSystem<ITimeSystem>().AddDelayTask(0.3f, () => 
                {
                    //collision.GetComponent<Player>().isDodgeCD = false;
                    //加载下一个关卡(敌人及道具生成)配置文件
                    var doc = new XmlDocument();
                    //(随机)读取LevelItem配置
                    doc.Load(APPConst.Level + transform.parent.name+".xml");

                    var levelNode = doc.SelectSingleNode("Level");
                    foreach (XmlElement item in levelNode.ChildNodes)
                    {
                        var name = item.Attributes["name"].Value;
                        var posX = item.Attributes["x"].Value;
                        var posY = item.Attributes["y"].Value;
                        var tag = item.GetAttribute("tag");
                        //实例化并创建相应的对象池
                        PoolManager.Instance.GetObj(name, new Vector3(float.Parse(posX) + Camera.main.transform.position.x, float.Parse(posY) + Camera.main.transform.position.y, 0));
                        if (tag == "Enemy" || tag == "Boss")
                        {
                            this.SendCommand(new CreateEnemyCommand(transform.parent.name));
                        }
                    }
                });
                gameObject.SetActive(false);
            }
        }
    }
}