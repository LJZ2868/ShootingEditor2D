using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;
using UnityEngine.UI;

namespace ShootEditor2D
{
    public abstract class EHPBarProperty : MonoBehaviour,IController
    {
        //目标物体
        protected abstract Transform Target { get; }

        //偏移位置
        protected abstract Vector3 OffsetPos { get; set; }
        protected virtual Vector3 Size => Target.GetComponent<Renderer>().bounds.size;

        //血条ui
        protected abstract Transform HPBar { get; }

        protected virtual void Awake()
        {
            //offsetPos = new Vector3(0, size.y * 2 / 3, 0);
            //获取最大血量
            var maxHP = Target.GetComponent<EnemyProperty>().EnemyItem.HP.Value;

            Target.GetComponent<EnemyProperty>().EnemyItem.HP.RegisterOnValueChange(newValue =>
            {
                HPBar.GetComponent<Image>().fillAmount = newValue / maxHP;
                //Debug.Log(this.GetModel<IEnemyModel>().EnemyModelItems[0].HP.Value);

            }).UnRegisterWhenGameObjectOnDestroy(gameObject);
        }

        protected virtual void OnEnable()
        {
            this.GetSystem<IObjectUIFollowSystem>().UIFollowWithObject(transform, Target, -OffsetPos);
        }

        public IIOC GetIOC()
        {
            return ShootEditor2D.Interface;
        }
    }
}