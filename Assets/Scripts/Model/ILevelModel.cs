using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;
using Cinemachine;

namespace ShootEditor2D
{
    public interface ILevelModel : IModel
    {
        GameObject player { get; set; }
        //[SerializeField]
        //List<LevelDate> levelDates { get; set;  }

        Serialization<LevelDate> LevelDates { get; set; }

        void LoadDate(LevelDate date);
        List<LevelDate> SaveDate(LevelDate levelDate);
    }

    [System.Serializable]
    public class Serialization<T>
    {
        [SerializeField]
        List<T> target;
        public List<T> ToList() { return target; }

        public Serialization(List<T> target)
        {
            this.target = target;
        }
    }

    [System.Serializable]
    public class LevelDate
    {
        public string Name;
        //资源名(路径)
        public string Path;
        //位置
        public Vector3 Position;

        public string Tag;

        public bool IsRoomClear;
    }

    public class LevelDateItem : MonoBehaviour, IController
    {
        public IIOC GetIOC()
        {
            return ShootEditor2D.Interface;
        }

        public LevelDate Date=new LevelDate();

        private void Awake()
        {
            this.GetSystem<ITimeSystem>().AddDelayTask(1, () =>
            {
                UpdateLevelDate();
                this.GetModel<ILevelModel>().SaveDate(Date);
            });

            this.GetSystem<ITimeSystem>().AddDelayTask(1, () => 
            {
                this.RegisterEvent<SaveLevelDateEvent>(e =>
                {
                    UpdateLevelDate();
                    if (e.Name == Date.Name)
                    {
                        Date.IsRoomClear = transform.GetComponentInChildren<Door>(true).isClear;
                    }
                    this.GetModel<ILevelModel>().SaveDate(Date);
                }).UnRegisterWhenGameObjectOnDestroy(gameObject);
            });

            
        }

        void UpdateLevelDate()
        {
            Date.Position = transform.position;
        }

        //public LevelDate InitLevelDate(LevelDate date)
        //{
        //    Date = new LevelDate()
        //    {
        //        Name = date.Name,
        //        Path = date.Path,
        //        Position = date.Position,
        //        Tag = date.Tag,
        //        IsRoomClear = date.IsRoomClear
        //    };
        //    return Date;
        //}

    }

    public class LevelModel : AbstractIModel, ILevelModel
    {
        public GameObject player { get; set; }
        public Serialization<LevelDate> LevelDates { get; set; } = new Serialization<LevelDate>(new List<LevelDate>());

        public void LoadDate(LevelDate date)
        {
            if (date.Tag != "Player")
            {
                ResManager.Instance.LoadAysnc<GameObject>(date.Path, obj =>
                {
                    //obj.name = date.path;
                    //Debug.Log(obj.name);
                    obj.transform.position = date.Position;
                    if (!obj.TryGetComponent(out LevelDateItem Date))
                    {
                        obj.AddComponent<LevelDateItem>().Date = date;
                    }
                });
            }
            else
            {
                if (player == null)
                {
                    ResManager.Instance.LoadAysnc<GameObject>(date.Path, obj =>
                    {
                        obj.transform.position = new Vector3(date.Position.x, date.Position.y+3, date.Position.z);
                        obj.AddComponent<LevelDateItem>().Date = date;
                        //obj.name = date.path;
                        //obj.SetActive(false);
                        player = obj;
                    });
                }
                else
                {
                    player.transform.position = date.Position;
                    player.SetActive(true);
                    Debug.Log("reload");
                }
            }
        }

        public List<LevelDate> SaveDate(LevelDate levelDate)
        {
            //transform.GetComponent<LevelDate>().position = transform.position;
            //如果不存在数据则添加
            if (!LevelDates.ToList().Contains(levelDate))
            {
                LevelDates.ToList().Add(levelDate);
                //Debug.Log("不存在");
            }
            if (levelDate.Tag == "IsHave")
            {
                LevelDates.ToList().Remove(levelDate);
            }
            //更新
            //else
            //{
            //    LevelDates.ToList().Remove(levelDate);
            //    LevelDates.ToList().Add(levelDate);
            //    Debug.Log("存在");
            //}
            return LevelDates.ToList();
        }

        protected override void OnInit()
        {
            
        }
    }
}