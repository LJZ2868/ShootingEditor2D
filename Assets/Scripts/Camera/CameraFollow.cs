using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;

namespace ShootEditor2D
{
    public class CameraFollow : MonoBehaviour,IController
    {
        public IIOC GetIOC()
        {
            return ShootEditor2D.Interface;
        }

        LevelDate date;

        private void Awake()
        {
            //this.GetSystem<ITimeSystem>().AddDelayTask(1, () =>
            //{
            //    date = new LevelDate()
            //    {
            //        Name = name,
            //        Path = "Game/CM vcam1",
            //        Position = transform.position,
            //        Tag = "Camera",
            //    };
            //    this.GetModel<ILevelModel>().SaveDate(date);
            //});

            //this.RegisterEvent<SaveLevelDateEvent>(e =>
            //{
            //    UpdateLevelDate();
            //});

            //ÉãÏñ»ú¸úËæ
            this.RegisterEvent<NextLevelEvent>(e =>
            {
                GetComponent<CinemachineVirtualCamera>().Follow = e.obj.transform;
                //Debug.Log(e.obj.name);
            }).UnRegisterWhenGameObjectOnDestroy(gameObject); 
        }

        //void UpdateLevelDate()
        //{
        //    date.Position = transform.position;
        //    //this.GetModel<ILevelModel>().SaveDate(date);
        //}
    }
}