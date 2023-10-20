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

                //�ӳ�0.3s����enemy
                this.GetSystem<ITimeSystem>().AddDelayTask(0.3f, () => 
                {
                    //collision.GetComponent<Player>().isDodgeCD = false;
                    //������һ���ؿ�(���˼���������)�����ļ�
                    var doc = new XmlDocument();
                    //(���)��ȡLevelItem����
                    doc.Load(APPConst.Level + transform.parent.name+".xml");

                    var levelNode = doc.SelectSingleNode("Level");
                    foreach (XmlElement item in levelNode.ChildNodes)
                    {
                        var name = item.Attributes["name"].Value;
                        var posX = item.Attributes["x"].Value;
                        var posY = item.Attributes["y"].Value;
                        var tag = item.GetAttribute("tag");
                        //ʵ������������Ӧ�Ķ����
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