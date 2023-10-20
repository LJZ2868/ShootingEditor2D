using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace FrameworkDesign
{
    public class ResManager : MonoSingleton<ResManager>
    {
        //ͬ��������Դ
        public T Load<T>(string resName, System.Func<T,T> callback) where T : Object
        {
            T resObj = Resources.Load<T>(resName);
            if (resObj is GameObject)
            {
                return callback(GameObject.Instantiate(resObj));
            }
            else
                return resObj;
        }

        public T Load<T>(string resName) where T : Object
        {
            T resObj = Resources.Load<T>(resName);
            if (resObj is GameObject)
            {
                return GameObject.Instantiate(resObj);
            }
            else
                return resObj;
        }

        //�첽������Դ
        public void LoadAysnc<T>(string resName, UnityAction<T> callback) where T : Object
        {
            //�����첽���ص�Э��
            StartCoroutine(LoadAsyncCor(resName, callback));
        }

        //������Эͬ������  ���� �����첽���ض�Ӧ����Դ
        IEnumerator LoadAsyncCor<T>(string resName, UnityAction<T> callback) where T : Object
        {

            ResourceRequest R = Resources.LoadAsync<T>(resName);
            yield return R;

            if (R.asset is GameObject)
            {
                var obj = GameObject.Instantiate(R.asset);
                obj.name = R.asset.name;
                callback(obj as T);
            }
            else
                callback(R.asset as T);
        }
    }
}