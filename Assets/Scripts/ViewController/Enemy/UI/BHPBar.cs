using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShootEditor2D
{
    public class BHPBar : EHPBarProperty
    {
        protected override Transform Target => transform.parent.parent;

        protected override Vector3 OffsetPos { get ; set ; }

        protected override Transform HPBar => transform;

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void Awake()
        {
            OffsetPos = Vector3.zero;
            base.Awake();
        }
    }
}