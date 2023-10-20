using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;
using UnityEngine.SceneManagement;

namespace ShootEditor2D
{
    public class StartPanel : BaseUIPanel,IController
    {
        public void NewGame()
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
        }

        public void Continue()
        {
            //加载场景
            SceneManager.LoadScene("StartScene");
        }

        public void Exit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//如果是在unity编译器中
#else
        Application.Quit();
#endif
        }

        public IIOC GetIOC()
        {
            return ShootEditor2D.Interface;
        }
    }
}