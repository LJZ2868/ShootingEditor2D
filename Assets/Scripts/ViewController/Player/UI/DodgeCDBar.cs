using FrameworkDesign;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ShootEditor2D
{
    public class DodgeCDBar : MonoBehaviour, IController
    {
        //Ŀ������
        Transform target;
        //ƫ��λ��
        Vector3 offsetPos;

        UIInfo currentInfo;

        private void Awake()
        {
            target = transform.parent.parent;
            var size = target.GetComponent<Renderer>().bounds.size;
            offsetPos = new Vector3(size.x *9/10, 0, 0);

            float maxDodgeCD = this.GetModel<IPlayerModel>().MaxDodgeCD.Value;

            this.GetModel<IPlayerModel>().MaxDodgeCD.RegisterOnValueChange(newValue=> 
            {
                maxDodgeCD = newValue;
            });

            //cd��ʾ
            target.GetComponent<Player>().dodgeCD.RegisterOnValueChange(newValue =>
            {
                GetComponent<Image>().fillAmount = newValue/maxDodgeCD;
                if (newValue >= maxDodgeCD)
                {
                    GetComponent<Image>().color = Color.red;
                }
            }).UnRegisterWhenGameObjectOnDestroy(gameObject);

            //ui����
            this.RegisterEvent<PlayerLookDirectionChangeEvent>(e => 
            {
                this.GetSystem<IObjectUIFollowSystem>().RemoveUIFollow(currentInfo);
                //isTurn = e.lookDirection.x < 0;
                if (e.lookDirection.x < 0)
                {
                    this.GetSystem<IObjectUIFollowSystem>().UIFollowWithObject(transform, target, offsetPos);
                }
                else
                {
                    this.GetSystem<IObjectUIFollowSystem>().UIFollowWithObject(transform, target, -offsetPos);
                }
            });

            //this.GetModel<IPlayerModel>().IsCanDodge.RegisterOnValueChange(newValue =>
            //{
            //    if (newValue)
            //        gameObject.SetActive(true);
            //});

            //gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            currentInfo = this.GetSystem<IObjectUIFollowSystem>().UIFollowWithObject(transform, target, -offsetPos);
        }

        public IIOC GetIOC()
        {
            return ShootEditor2D.Interface;
        }
    }
}