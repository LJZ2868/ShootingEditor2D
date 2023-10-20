using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;

namespace ShootEditor2D
{
    public class RoomDate : MonoBehaviour,IController
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
            //        Path = "Game/Level/" + name,
            //        Position = transform.position,
            //        Tag = "Room",
            //        isRoomClear = false
            //    };
            //    this.GetModel<ILevelModel>().SaveDate(date);
            //});

            //this.RegisterEvent<SaveLevelDateEvent>(e => 
            //{
            //    UpdateLevelDate();
            //});

        }


        //void UpdateLevelDate()
        //{
        //    date.Position = transform.position;
        //    date.IsRoomClear = transform.GetComponentInChildren<Door>().isClear;
        //    //this.GetModel<ILevelModel>().SaveDate(date);
        //}
    }
}