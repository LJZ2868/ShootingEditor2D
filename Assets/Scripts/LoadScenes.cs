using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;


namespace ShootEditor2D
{
    public class LoadScenes : MonoBehaviour, IController
    {
        public IIOC GetIOC()
        {
            return ShootEditor2D.Interface;
        }

        private void Start()
        {
            this.SendCommand<ChangeScenceCommand>();
        }
    }
}