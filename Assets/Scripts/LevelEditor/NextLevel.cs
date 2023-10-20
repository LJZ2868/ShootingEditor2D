using FrameworkDesign;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace ShootEditor2D
{
    public class NextLevel : MonoBehaviour,IController
    {
        //GameObject obj=null;
        public IIOC GetIOC()
        {
            return ShootEditor2D.Interface;
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            { 
                //摄像机跟随下一个房间
                this.SendCommand(new NextLevelCommand(transform.parent.gameObject));

                //1s后如果房间没有Enemy则自动保存

                //if (transform.parent.Find("Door").GetComponent<Door>().isClear)
                //{
                //    var date = this.GetModel<ILevelModel>().SaveDate();
                //    this.GetSystem<ISaveLoadSystem>().SaveByJson(APPConst.Date, date);
                //    var levelDates = this.GetModel<ILevelModel>().LevelDates;
                //    this.SendCommand(new SaveLevelDateCommand(transform.parent.name));
                //    this.GetSystem<ISaveLoadSystem>().SaveByJson(APPConst.LevelDate, levelDates);
                //}

            }
        }
    }
}