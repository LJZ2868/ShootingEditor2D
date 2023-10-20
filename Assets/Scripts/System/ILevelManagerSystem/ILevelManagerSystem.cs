using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using FrameworkDesign;
using UnityEngine.SceneManagement;

namespace ShootEditor2D
{
    public interface ILevelManagerSystem : ISystem
    { 
    }


    public class LevelManager : MonoSingleton<LevelManager>,IController
    {
        public IIOC GetIOC()
        {
            return ShootEditor2D.Interface;
        }

        protected override void Awake()
        {
            //加载角色数据
            var date = this.GetSystem<ISaveLoadSystem>().LoadFromJson<SaveDate>(APPConst.PlayerDate);
            this.GetModel<IPlayerModel>().LoadDate(date);

            this.RegisterEvent<ChangeScenceEvent>(e =>
            {
                this.GetSystem<IObjectUIFollowSystem>().mUIInfos.Clear();
                PoolManager.Instance.PoolClear();
                //Player.Instance.gameObject.SetActive(false);
                //Debug.Log(SceneManager.GetActiveScene().name);
                if (SceneManager.GetActiveScene().name == "StartScene")
                {
                    //加载主ui
                    UIPanelManager.Instance.ShowUIPanel("MainPanel");
                    //var list = this.GetModel<ILevelModel>().LevelDates;
                    this.SendCommand<SelectCommand>();

                    //读取关卡信息
                    var dateList = this.GetSystem<ISaveLoadSystem>().LoadFromJson<Serialization<LevelDate>>(APPConst.LevelDate).ToList();
                    foreach (var item in dateList)
                    {
                        this.GetModel<ILevelModel>().LoadDate(item);
                    }
                    
                    //this.GetModel<ILevelModel>().player.SetActive(true);
                    //Player.Instance.gameObject.SetActive(true);
                    //xml加载
                    #region
                    ////加载第一个场景配置文件
                    //var doc = new XmlDocument();
                    ////读取LevelItem配置
                    //doc.Load(APPConst.LevelXML);

                    //var levelNode = doc.SelectSingleNode("Level");
                    //foreach (XmlElement item in levelNode.ChildNodes)
                    //{
                    //    var name = item.Attributes["name"].Value;
                    //    var posX = item.Attributes["x"].Value;
                    //    var posY = item.Attributes["y"].Value;

                    //    //var obj = GameObject.Instantiate(Resources.Load<GameObject>(name));
                    //    //obj.transform.position = new Vector3(float.Parse(posX), float.Parse(posY), 0);
                    //    if (item.GetAttribute("tag") != "Player")
                    //    {
                    //        ResManager.Instance.LoadAysnc<GameObject>(name, obj =>
                    //        {
                    //            obj.name = name;
                    //            obj.transform.position = new Vector3(float.Parse(posX), float.Parse(posY), 0);
                    //        });
                    //        //player.transform.position = new Vector3(float.Parse(posX), float.Parse(posY), 0);
                    //    }
                    //    else 
                    //    {
                    //        if (player == null)
                    //        {
                    //            ResManager.Instance.LoadAysnc<GameObject>(name, obj =>
                    //            {
                    //                obj.transform.position = new Vector3(float.Parse(posX), float.Parse(posY), 0);
                    //                obj.name = name;
                    //                player = obj;
                    //            });
                    //        }
                    //        else
                    //        {
                    //            player.transform.position = new Vector3(float.Parse(posX), float.Parse(posY), 0);
                    //            player.SetActive(true);
                    //        }
                    //    }
                    //}
                    //GameObject.Instantiate(Resources.Load<GameObject>("Game/Level/Manager"));
                    #endregion

                }
                else if (SceneManager.GetActiveScene().name == "SampleScene")
                {
                    //player?.SetActive(true);
                    var doc = new XmlDocument();
                    //读取LevelItem配置
                    doc.Load(APPConst.LevelXML);

                    var levelNode = doc.SelectSingleNode("Level");

                    foreach (XmlElement item in levelNode.ChildNodes)
                    {
                        var name = item.Attributes["name"].Value;
                        var posX = item.Attributes["x"].Value;
                        var posY = item.Attributes["y"].Value;

                        //if (name == "Game/Player" && GameObject.FindGameObjectWithTag("Player") != null)
                        //{
                        //    GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(float.Parse(posX), float.Parse(posY), 0);
                        //    continue;
                        //}


                        Debug.Log(name + posX + posY);
                        var obj = GameObject.Instantiate(Resources.Load<GameObject>(name));
                        obj.transform.position = new Vector3(float.Parse(posX), float.Parse(posY), 0);

                        //if (name == "Game/Player")
                        //    player = GameObject.FindGameObjectWithTag("Player");
                    }
                }
                else if (SceneManager.GetActiveScene().name == "GamePass")
                {
                    Player.Instance.gameObject.SetActive(false);
                    Debug.Log("end");
                    //this.SendCommand<ChangeScenceCommand>();
                    //GameObject.Instantiate(Resources.Load<GameObject>("Game/Level/Manager"));
                }
            });

            //this.SendCommand<ChangeScenceCommand>();
            base.Awake();
        }
    }

    //加载关卡资源
    public class LevelManagerSystem : AbstractISystem,ILevelManagerSystem
    {

        protected override void OnInit()
        {
            var mLevelManager = new GameObject(nameof(LevelManager));
            mLevelManager.AddComponent<LevelManager>();
        }
    }
}