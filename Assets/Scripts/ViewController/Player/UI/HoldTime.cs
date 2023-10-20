using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;
using UnityEngine.UI;

namespace ShootEditor2D
{
    public class HoldTime : MonoBehaviour, IController
    {
        //Ŀ������
        Transform target;

        //ƫ��λ��
        Vector3 offsetPos;

        private void Awake()
        {
            target = transform.parent.parent;
            var size = target.GetComponent<Renderer>().bounds.size;
            offsetPos = new Vector3(0, size.y * 2 / 3, 0);

            target.GetComponent<Player>().holdTime.RegisterOnValueChange(newValue =>
            {
                GetComponent<Image>().fillAmount = newValue;
                if (newValue >= 1f)
                {
                    GetComponent<Image>().color = Color.red;
                }
            }).UnRegisterWhenGameObjectOnDestroy(gameObject);
        }

        private void OnEnable()
        {
            this.GetSystem<IObjectUIFollowSystem>().UIFollowWithObject(transform,target,offsetPos);
        }

        public IIOC GetIOC()
        {
            return ShootEditor2D.Interface;
        }
    }
}