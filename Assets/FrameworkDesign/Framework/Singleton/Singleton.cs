using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameworkDesign
{

    /// <summary>
    /// һ����ͨ�ĵ���
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Singleton<T> where T : new()
    {
        /// <summary>
        /// ��������
        /// </summary>
        private static T instance;

        /// <summary>
        /// �߳���
        /// </summary>
        private static readonly object locked = typeof(T);

        public static T Instance
        {
            get
            {
                if (instance == null)
                    lock (locked)
                        instance ??= new T();
                return instance;
            }
        }
    }


    /// <summary>
    /// �̳�Mono��һ������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T instance;

        private static readonly object locked = typeof(T);

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locked)
                    {
                        instance ??= FindObjectOfType<T>();

                        instance ??= new GameObject(typeof(T).Name).AddComponent<T>();
                    }
                }
                return instance;
            }
        }

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = (T)this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

        }

    }
}
