using System.Collections.Generic;
using UnityEngine;
namespace FrameworkDesign
{
    public class PoolDate : MonoBehaviour
    {
        public GameObject fatherObj;
        public List<GameObject> poolList;

        //��һ��������ʾҪ���뻺��صĶ��󣬵ڶ���������ʾ�����
        public PoolDate(GameObject obj, GameObject poolObj)
        {
            //�����ǵĳ��� ����һ�������� ���Ұ�����Ϊ����pool(�¹�)�����������
            fatherObj = new GameObject(obj.name);
            fatherObj.transform.parent = poolObj.transform;

            //��ʼ������صĶ���
            poolList = new List<GameObject>();
            //������صĶ�����뻺���
            PushObj(obj);

        }

        public void PushObj(GameObject obj)
        {
            //������
            poolList.Add(obj);
            //����ʱʹ��ʧЧ
            obj.SetActive(false);
            //���ø�����
            obj.transform.parent = fatherObj.transform;

        }

        public GameObject GetObj()
        {
            //ȡ����һ��
            GameObject obj = poolList[0];
            poolList.RemoveAt(0);

            //���ø��ӹ�ϵ
            obj.transform.parent = null;
            //���� ������ʾ
            obj.SetActive(true);


            return obj;
        }
    }

    public class PoolManager : Singleton<PoolManager>
    {

        //�¹�ͳ���
        public Dictionary<string, PoolDate> poolDic = new Dictionary<string, PoolDate>();

        //�����
        private GameObject poolObj;


        //��ȡ��ʵ����GameObject(������ʵ�����Ӷ���ʱ)
        public GameObject GetObj(string name,GameObject @object)
        {
            GameObject gameObj;
            //�г��� ���ҳ������ж���
            if (poolDic.ContainsKey(name) && poolDic[name].poolList.Count > 0)
            {
                gameObj = poolDic[name].GetObj();
                gameObj.transform.position = @object.transform.position;
                gameObj.transform.rotation = @object.transform.rotation;
                gameObj.transform.localScale = @object.transform.lossyScale;
            }
            else
            {
                gameObj = ResManager.Instance.Load<GameObject>(name,obj =>
                {
                    obj.transform.SetPositionAndRotation(@object.transform.position, @object.transform.rotation);
                    obj.transform.localScale = @object.transform.lossyScale;
                    obj.name = name;
                    obj.SetActive(true);
                    return obj;
                });
                //gameObj = ResManager.Instance.Load<GameObject>(name);
                //gameObj.transform.SetPositionAndRotation(@object.transform.position, @object.transform.rotation);
                //gameObj.transform.localScale = @object.transform.lossyScale;
                //gameObj.name = name;
                //gameObj.SetActive(true);
            }
            return gameObj;
        }



        //ʵ����Ԥ����
        public GameObject GetObj(string name, Vector3 vector)
        {
            GameObject gameObj = null;
            //�г��� ���ҳ������ж���
            if (poolDic.ContainsKey(name) && poolDic[name].poolList.Count > 0)
            {
                gameObj = poolDic[name].GetObj();
                gameObj.transform.position = vector;
            }
            else
            {
                ResManager.Instance.LoadAysnc<GameObject>(name, obj =>
                {
                    obj.transform.position = vector;
                    obj.name = name;
                    obj.SetActive(true);
                    gameObj = obj;
                });
                //gameObj = GameObject.Instantiate(Resources.Load<GameObject>(name),@object.transform.position,@object.transform.rotation);
                //gameObj.transform.localScale = @object.transform.lossyScale;
                //gameObj.name = name;
                //gameObj.SetActive(true);
            }
            return gameObj;
        }

        //��ȡGameObject
        public GameObject GetObj(string name)
        {
            GameObject gameObj;
            //�г��� ���ҳ������ж���
            if (poolDic.ContainsKey(name) && poolDic[name].poolList.Count > 0)
            {
                gameObj = poolDic[name].GetObj();
            }
            else
            {
                gameObj = Resources.Load<GameObject>(name);
                gameObj.name = name;
                gameObj.SetActive(true);
            }
            return gameObj;
        }

        public void PushObj(string name, GameObject obj)
        {
            //�Ƿ��л����
            if (poolObj == null)
            {
                poolObj = new GameObject("Pool");
            }


            //�����г���
            if (poolDic.ContainsKey(name))
            {
                poolDic[name].PushObj(obj);
            }
            //����û�г���
            else
            {
                poolDic.Add(name, new PoolDate(obj, poolObj));
            }
        }

        // ��ջ���صķ��� 
        // ��Ҫ���� �����л�ʱ
        public void PoolClear()
        {

            poolDic.Clear();
            poolObj = null;

        }



    }
}