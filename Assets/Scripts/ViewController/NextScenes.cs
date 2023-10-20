using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FrameworkDesign;

namespace ShootEditor2D
{
    public class NextScenes: MonoBehaviour,IController
    {
        public string mGamePassSceneName;


        public IIOC GetIOC()
        {
            return ShootEditor2D.Interface;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                switch (mGamePassSceneName)
                {
                    case "GamePass":
                        //Òþ²ØUI
                        UIPanelManager.Instance.HideUIPanel("MainPanel");

                        //¼ÓÔØÐÂUI
                        UIPanelManager.Instance.ShowUIPanel("EndPanel");

                        SceneManager.LoadScene(mGamePassSceneName);
                        break;
                    default:
                        break;
                }
                
            }
        }
    }
}