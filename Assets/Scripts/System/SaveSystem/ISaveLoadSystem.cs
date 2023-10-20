using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;
using System.IO;

namespace ShootEditor2D
{
    public interface ISaveLoadSystem : ISystem
    {
        void SaveByJson(string saveFileName,object date);
        T LoadFromJson<T>(string saveFileName);
        void DeletSaveFile(string saveFileName);
    }

    public class SaveLoadSystem : AbstractISystem, ISaveLoadSystem
    {
        public void DeletSaveFile(string saveFileName)
        {
            var path = saveFileName;
            File.Delete(path);

        }

        public T LoadFromJson<T>(string saveFileName)
        {
            var path = saveFileName;

            var json = File.ReadAllText(path);

            var date = JsonUtility.FromJson<T>(json);
            return date;
        }

        public void SaveByJson(string saveFileName, object date)
        {
            //将数据转换成json格式
            var json = JsonUtility.ToJson(date,true);
            //存档路径
            var path = saveFileName;

            File.WriteAllText(path, json);

            Debug.Log(json);
            Debug.Log(path);
        }



        protected override void OnInit()
        {
            
        }
    }
}