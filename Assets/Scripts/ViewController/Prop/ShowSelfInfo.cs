using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;
using DG.Tweening;

namespace ShootEditor2D
{
    public class ShowSelfInfo : MonoBehaviour,IController
    {
        public float checkRadius;

        public LayerMask checkLayer;

        public BindableProperty<bool> isTrigger=new BindableProperty<bool>();

        public IIOC GetIOC()
        {
            return ShootEditor2D.Interface;
        }

        private void Awake()
        {
            var name = GetComponent<AbstractPropItem>().Name;
            isTrigger.RegisterOnValueChange(newValue =>
            {
                if (newValue)
                    this.SendCommand(new ShowPropInfoCommand(name, this.GetSystem<IPropSystem>().PropItems[name].Info,newValue));
                else
                    this.SendCommand(new ShowPropInfoCommand(name, this.GetSystem<IPropSystem>().PropItems[name].Info, newValue));
            });
        }

        private void FixedUpdate()
        {
            isTrigger.Value = Physics2D.OverlapCircle(transform.position, checkRadius, checkLayer);
        }


        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, checkRadius);
        }
    }
}