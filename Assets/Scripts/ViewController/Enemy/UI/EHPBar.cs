using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;
using UnityEngine.UI;

namespace ShootEditor2D
{

    public class EHPBar : EHPBarProperty
    {
        protected override Transform Target => transform.parent.parent;

        protected override Vector3 OffsetPos { get ; set ; }

        protected override Transform HPBar => transform.Find("Value");

        protected override void Awake()
        {
            OffsetPos = new Vector3(0, Size.y * 2 / 3, 0);
            //更新血条视图
            base.Awake();
   
        }
    }   
}
