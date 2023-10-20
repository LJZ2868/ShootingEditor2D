using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FrameworkDesign;
using DG.Tweening;
using TMPro;
using System.Linq;

namespace ShootEditor2D
{
    public class Select : BaseUIPanel,IController
    {
        [SerializeField]
        AnimationCurve curve;

        Button[] buttons;

        float animationTime = 0.5f;

        Vector3 originalPosition;
        Vector3 toPosition;


        //Dictionary<string,ICharacterInfo> characters;

        List<int> randomList=new List<int>();

        private void Awake()
        {
            

        }

        private void Start()
        {
            originalPosition = new Vector3 (transform.position.x-960,transform.position.y-540,transform.position.x);
            toPosition = new Vector3(0, 150, 0);

            
            //gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            if (this.GetModel<IPlayerModel>().CharacterSelectCount.Value > 0)
            {
                Hide();
            }
            else
            {
                this.GetSystem<ITimeSystem>().AddDelayTask(1f, () =>
                {
                    ShowPanel();
                //Debug.Log("show");
                });
                CharacterSelect();
                this.RegisterEvent<SelectEvent>(e =>
                {
                    gameObject.SetActive(true);
                });
            }
        }

        void CharacterSelect()
        {
            var characters = this.GetSystem<ICharacterSystem>().characterInfos;

            //产生随机数
            while (randomList.Count < 3)
            {
                int n = Random.Range(0, characters.Count);
                if (!randomList.Contains(n))
                    randomList.Add(n);
            }

            int i = 0;
            buttons = transform.GetComponentsInChildren<Button>();
            foreach (var item in buttons)
            {
                foreach (var charat in characters)
                {
                    if (randomList[i] == charat.Value.ID)
                    {
                        item.GetComponentInChildren<TextMeshProUGUI>().text = charat.Value.Name;
                    }
                }

                item.onClick.AddListener(() =>
                {
                    //解除暂停状态
                    this.GetSystem<IPauseSystem>().StartPauseAnimation(transform, toPosition, originalPosition, animationTime, curve, false, () =>
                    {
                        gameObject.SetActive(false);
                        randomList.Clear();
                        
                    });

                    Debug.Log(item.name);

                    this.SendCommand(new GetCharacterCommand(item.GetComponentInChildren<TextMeshProUGUI>().text));

                    //保存角色数据
                    var date = this.GetModel<IPlayerModel>().SaveDate();
                    this.GetSystem<ISaveLoadSystem>().SaveByJson(APPConst.PlayerDate, date);

                    foreach (var item in buttons)
                    {
                        item.onClick.RemoveAllListeners();
                    }

                });
                //Debug.Log(characters[i].Name);
                i = i + 1 < characters.Count ? i + 1 : i;
            }
        }

        void ShowPanel()
        {
            //暂停状态
            //gameObject.SetActive(true);
            //移动动画计时
            //timer = Time.realtimeSinceStartup;
            this.GetSystem<IPauseSystem>().StartPauseAnimation(transform,originalPosition,toPosition,animationTime,curve,true,null);
            
        }

        public IIOC GetIOC()
        {
            return ShootEditor2D.Interface;
        }

    }
}