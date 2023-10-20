using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;

namespace ShootEditor2D
{
    public class NormalEnemy : EnemyProperty
    {
        protected override float Speed { get; set; }
        //ÀàÐÍ
        protected override string Name => "NormalEnemy";

        protected override void Awake()
        {
            Speed = 100f;

            base.Awake();
        }


    }
}
