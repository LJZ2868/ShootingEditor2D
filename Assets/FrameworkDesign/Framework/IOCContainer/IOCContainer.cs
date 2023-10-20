using System;
using System.Collections.Generic;
namespace FrameworkDesign
{
    //IOC����  �����ݷ����������
    public abstract class IOCContainer<T> : IIOC where T:IOCContainer<T>,new()
    {
        private static Dictionary<Type, object> mInstances = new Dictionary<Type, object>();

        private static T mContainer;

        //�¼�ϵͳ
        private ITypeEventSystem mTypeEventSystem = new TypeEventSystem();

        //�Ƿ���ɳ�ʼ��
        private bool mInited = false;

        //��ʼ��Model����
        private List<IModel> mModels = new List<IModel>();

        //��ʼ��System����
        private List<ISystem> mSystems = new List<ISystem>();

        //��ʼ��
        protected abstract void Init();

        public static IIOC Interface
        {
            get
            {
                if (mContainer == null)
                {
                    MakeSureIOC();
                }
                return mContainer;
            }
        }


        //ȷ��������������(����)
        static void MakeSureIOC()
        {
            if (mContainer == null)
            {
                mContainer = new T();

                //������ʼ��
                mContainer.Init();


                //Model������ݳ�ʼ��
                foreach (var item in mContainer.mModels)
                {
                    item.Init();
                }

                //�������
                mContainer.mModels.Clear();
                
                //System��ʼ��(System��Ϊ�ײ��ʼ��ʱ���ܻ����Model�������������Model��ʼ����)
                foreach (var item in mContainer.mSystems)
                {
                    item.Init();
                }
                mContainer.mSystems.Clear();

                mContainer.mInited = true;
            }
        }



        //ע��ʵ��(ģ��),ͬһ�ӿڵ�ʵ�ֻᱻ�滻
        private  void Register<T>(T instance)
        {
            MakeSureIOC();

            Type key = typeof(T);

            if (!mInstances.ContainsKey(key))
                mInstances.Add(key, instance);
            else
                mInstances[key] = instance;
        }
        //ʵ����ʵ��(ģ��)
        private T Get<T>() where T:class
        {
            MakeSureIOC();

            Type key = typeof(T);

            if (mInstances.ContainsKey(key))
                return mInstances[typeof(T)] as T;

            return null;
        }

        public T GetUtility<T>() where T : class,IUtility
        {
            return Get<T>();
        }

        public void RegisterModel<T>(T model) where T : IModel
        {
            //IOC������Model��ע��ǰͨ������IOCContainer��ȡ������ �����������ڳ�ʼ��ǰ����Model�� �����ظ���ʼ��
            model.SetIOC(this);
            mContainer.Register<T>(model);
            
            if (!mInited)
            {
                //��ӵ�Model������ ���ڳ�ʼ��
                mContainer.mModels.Add(model);
            }
            //��ʼ������(��ֹ�ظ���ʼ��)
            else
            {
                model.Init();
            }
        }

        public void RegisterUtility<T>(T utility) where T : IUtility
        {
            mContainer.Register<T>(utility);
        }

        public T GetModel<T>() where T : class,IModel
        {
            return Get<T>();
        }

        public void RegisterSystem<T>(T system) where T : ISystem
        {
            system.SetIOC(this);
            mContainer.Register<T>(system);

            if (!mInited)
            {
                mContainer.mSystems.Add(system);
            }
            else
            {
                system.Init();
            }
        }

        public void SendCommand<T>() where T : ICommand, new()
        {
            var command = new T();
            command.SetIOC(this);
            command.Execute();
        }

        public void SendCommand<T>(T command) where T : ICommand
        {
            command.SetIOC(this);
            command.Execute();
        }

        public T GetSystem<T>() where T : class, ISystem
        {
            return Get<T>();
        }

        public void SendEvent<T>() where T : new()
        {
            mTypeEventSystem.Send<T>();
        }

        public void SendEvent<T>(T e)
        {
            mTypeEventSystem.Send<T>(e);
        }

        public IUnRegister RegisterEvent<T>(Action<T> action)
        {
           return mTypeEventSystem.Register<T>(action);
        }

        public void UnRegisterEvent<T>(Action<T> action)
        {
            mTypeEventSystem.UnRegister<T>(action);
        }

        public TResult SendQuery<TResult>(IQuery<TResult> query)
        {
            query.SetIOC(this);
            return query.Do();
        }
    }
}
