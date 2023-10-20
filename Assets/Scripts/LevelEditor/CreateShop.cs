using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;
using System.Xml;

namespace ShootEditor2D
{
    public class CreateShop : MonoBehaviour,IController
    {
        static int i;
        //public bool isClear;
        List<string> Props = new List<string>();

        private void Awake()
        {
            Props.Clear();
            Props.Add(APPConst.Shop1);
            i = 0;
        }

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
                this.GetSystem<ITimeSystem>().AddDelayTask(0.3f, () =>
                {
                    var doc = new XmlDocument();
                    //(随机)读取LevelItem配置
                    doc.Load(Props[i]);

                    var levelNode = doc.SelectSingleNode("Shop");
                    foreach (XmlElement item in levelNode.ChildNodes)
                    {
                        var name = item.Attributes["name"].Value;
                        var posX = item.Attributes["x"].Value;
                        var posY = item.Attributes["y"].Value;
                        var tag = item.GetAttribute("tag");
                        if (tag == "Prop")
                        {
                            //实例化并创建相应的对象池
                            PoolManager.Instance.GetObj(name, new Vector3(float.Parse(posX) + Camera.main.transform.position.x, float.Parse(posY) + Camera.main.transform.position.y, 0));
                        }
                    }
                    i++;
                });
                gameObject.SetActive(false);
                transform.parent.GetComponent<LevelDateItem>().Date.IsRoomClear = true;
            }
        }
    }

}