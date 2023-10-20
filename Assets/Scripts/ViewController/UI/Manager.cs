using FrameworkDesign;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShootEditor2D
{
    public class Manager : MonoBehaviour,IController
    {
        public IIOC GetIOC()
        {
            return ShootEditor2D.Interface;
        }


        protected  void Awake()
        {
        }


        
    }
}