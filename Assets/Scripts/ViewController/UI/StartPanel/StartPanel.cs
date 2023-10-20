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
            //����playerDate
            var playerDate = this.GetSystem<ISaveLoadSystem>().LoadFromJson<SaveDate>(APPConst.InitPlayerDate);
            this.GetSystem<ISaveLoadSystem>().SaveByJson(APPConst.PlayerDate, playerDate);
            this.GetModel<IPlayerModel>().LoadDate(playerDate);


            //����levelDate
            var levelDate = this.GetSystem<ISaveLoadSystem>().LoadFromJson<Serialization<LevelDate>>(APPConst.InitLevelDate);
            this.GetSystem<ISaveLoadSystem>().SaveByJson(APPConst.LevelDate, levelDate);


            //���س���
            SceneManager.LoadScene("StartScene");
        }

        public void Continue()
        {
            //���س���
            SceneManager.LoadScene("StartScene");
        }

        public void Exit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//�������unity��������
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