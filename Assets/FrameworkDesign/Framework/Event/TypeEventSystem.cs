using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameworkDesign
{

    public interface ITypeEventSystem 
    {
        void Send<T>() where T : new();
        void Send<T>(T e);

        IUnRegister Register<T>(Action<T> action);
        void UnRegister<T>(Action<T> action);
    }

    //ע��ʱ���صĶ�������� ����ע��
    public interface IUnRegister
    {
        void UnRegister();
    }
    public class TypeEventSystemUnRegister<T> : IUnRegister
    {
        public ITypeEventSystem typeEventSystem;
        public Action<T> action;

        public void UnRegister()
        {
            typeEventSystem.UnRegister<T>(action);

            typeEventSystem = null;
            action = null;
        }
    }


    //ע���¼��Ĵ�����(����ʱ �������ٻ��Զ�ע��ע����¼�) 
    public class UnRegisterOnDestroyTrigger : MonoBehaviour
    {
        HashSet<IUnRegister> unRegisters = new HashSet<IUnRegister>();

        public void AddUnRegister(IUnRegister unRegister)
        {
            unRegisters.Add(unRegister);
        }

        private void OnDestroy()
        {
            foreach (var item in unRegisters)
            {
                item.UnRegister();
            }

            unRegisters.Clear();
        }
    }

    public static class UnRegisterExtension
    {
        public static void UnRegisterWhenGameObjectOnDestroy(this IUnRegister unRegister,GameObject gameObject)
        {
            //��ȡ�Ƿ���ش������ű�
            var trigger = gameObject.GetComponent<UnRegisterOnDestroyTrigger>();
            //û�оͼ���
            if (!trigger)
            {
                trigger = gameObject.AddComponent<UnRegisterOnDestroyTrigger>();
            }

            trigger.AddUnRegister(unRegister);
        }
    }


    public class TypeEventSystem : MonoBehaviour,ITypeEventSystem
    {
        public interface IRegistration
        {
        }

        public class Registration<T> : IRegistration
        {
            public Action<T> action = e => { };
        }

        //����ע����¼�
        Dictionary<Type, IRegistration> mResgistration = new Dictionary<Type, IRegistration>();

        public void Send<T>() where T : new()
        {
            T e = new T();
            Send<T>(e);
        }

        public void Send<T>(T e)
        {
            Type t = typeof(T);
            IRegistration registration;

            //����¼��Ѿ�ע�� registration��ֱ�ӻ�ȡ������
            if (mResgistration.TryGetValue(t, out registration))
            {
                (registration as Registration<T>).action(e);
            }
        }

        public IUnRegister Register<T>(Action<T> action)
        {
            Type t = typeof(T);
            IRegistration registration;

            //����¼��Ѿ�ע�� registration��ֱ�ӻ�ȡ
            if (mResgistration.TryGetValue(t, out registration))
            {
            }
            else
            {
                //ûע����½�һ������ӵ��ֵ���
                registration = new Registration<T>();
                mResgistration.Add(t, registration);
            }

            //����¼�
            (registration as Registration<T>).action += action;

            return new TypeEventSystemUnRegister<T>()
            {
                typeEventSystem = this,
                action = action 
            };


        }

        public void UnRegister<T>(Action<T> action)
        {
            Type t = typeof(T);
            IRegistration registration;

            //����¼��Ѿ�ע�� registration��ֱ�ӻ�ȡ��ע��
            if (mResgistration.TryGetValue(t, out registration))
            {
                (registration as Registration<T>).action -= action;
            }
        }
    }
}
