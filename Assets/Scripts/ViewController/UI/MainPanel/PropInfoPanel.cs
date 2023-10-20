using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using FrameworkDesign;
using DG.Tweening;

namespace ShootEditor2D
{
    public class PropInfoPanel : MonoBehaviour,IController
    {

        public IIOC GetIOC()
        {
            return ShootEditor2D.Interface;
        }

        private void Start()
        {
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            var name = transform.Find("PropName").GetComponent<TextMeshProUGUI>();
            var info = transform.Find("Info").GetComponent<TextMeshProUGUI>();

            this.RegisterEvent<ShowPropInfoEvent>(e =>
            {
                name.text = e.name;
                info.text = e.info;
                if (e.isShow)
                {
                    gameObject.SetActive(true);
                    transform.DOScale(new Vector3(1, 1, 1), 0.5f).SetEase(Ease.OutBack).OnComplete(()=> 
                    {
                        gameObject.SetActive(true);
                    });
                }
                else
                {
                    transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.InBack).OnComplete(()=> 
                    {
                        gameObject.SetActive(false);
                    });
                }
            });
        }

    }
}