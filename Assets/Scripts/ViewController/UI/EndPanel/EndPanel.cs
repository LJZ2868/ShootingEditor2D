using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FrameworkDesign;

namespace ShootEditor2D
{
    public class EndPanel : BaseUIPanel,IController
    {
        public IIOC GetIOC()
        {
            return ShootEditor2D.Interface;
        }

        private void Awake()
        {
            transform.Find("BackBtn").GetComponent<Button>().onClick.AddListener(() =>
            {

                //重置playerDate
                var playerDate = this.GetSystem<ISaveLoadSystem>().LoadFromJson<SaveDate>(APPConst.InitPlayerDate);
                this.GetSystem<ISaveLoadSystem>().SaveByJson(APPConst.PlayerDate, playerDate);
                this.GetModel<IPlayerModel>().LoadDate(playerDate);

                //重置levelDate
                var levelDate = this.GetSystem<ISaveLoadSystem>().LoadFromJson<Serialization<LevelDate>>(APPConst.InitLevelDate);
                this.GetSystem<ISaveLoadSystem>().SaveByJson(APPConst.LevelDate, levelDate);

                //加载场景
                SceneManager.LoadScene("StartScene");
                
                //隐藏UI
                UIPanelManager.Instance.HideUIPanel("EndPanel");
                //显示加载场景的UI
                UIPanelManager.Instance.ShowUIPanel("MainPanel");
            });
        }
    }
}