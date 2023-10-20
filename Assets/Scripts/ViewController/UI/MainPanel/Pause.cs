using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;
using UnityEngine.SceneManagement;

namespace ShootEditor2D
{
    public class Pause : MonoBehaviour,IController
    {
        public PlayerControl mInput;

        //按钮能否激活
        bool canTrigger = true;
        float animationTime = 0.5f;


        Vector3 originalPosition;
        Vector3 toPosition;
        Vector3 endPosition;

        [SerializeField]
        AnimationCurve curve;

        private void Awake()
        {
            mInput = new PlayerControl();
            mInput.Enable();
            mInput.Player.Pause.started += Pause_started;

            originalPosition = new Vector3(transform.localPosition.x,transform.localPosition.y);
            toPosition = new Vector3(0, 0, 0);
            endPosition = new Vector3(2020,0,0);
            gameObject.SetActive(false);
        }

        private void Pause_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (!gameObject.activeSelf)
            {
                canTrigger = false;

                //暂停状态
                this.GetSystem<IPauseSystem>().StartPauseAnimation(transform, originalPosition, toPosition, animationTime, curve,true, ()=> 
                {
                    canTrigger = true;
                });

                gameObject.SetActive(true);
            }
            else
            {
                Continue();
            }
        }

        public void Continue()
        {
            if (gameObject.activeSelf && canTrigger)
            {
                //解除暂停状态

                this.GetSystem<IPauseSystem>().StartPauseAnimation(transform, toPosition, endPosition, animationTime, curve, false,() =>
                {
                    transform.localPosition = originalPosition;
                    gameObject.SetActive(false);
                    canTrigger = true;
                });

                canTrigger = false;
            }
        }

        public void Exit()
        {

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//如果是在unity编译器中
#else
        Application.Quit();
#endif

        }

        public void Restart()
        {
            Player.Instance.gameObject.SetActive(false);
            //重置playerDate
            var playerDate = this.GetSystem<ISaveLoadSystem>().LoadFromJson<SaveDate>(APPConst.InitPlayerDate);
            this.GetSystem<ISaveLoadSystem>().SaveByJson(APPConst.PlayerDate, playerDate);
            this.GetModel<IPlayerModel>().LoadDate(playerDate);


            //重置levelDate
            var levelDate = this.GetSystem<ISaveLoadSystem>().LoadFromJson<Serialization<LevelDate>>(APPConst.InitLevelDate);
            this.GetSystem<ISaveLoadSystem>().SaveByJson(APPConst.LevelDate, levelDate);
            //this.GetSystem<ISaveLoadSystem>().DeletSaveFile(APPConst.PlayerDate);
            SceneManager.LoadScene("StartScene");

            transform.parent.GetComponentInChildren<Select>(true).Show();

        }

        public IIOC GetIOC()
        {
            return ShootEditor2D.Interface;
        }
    }
}