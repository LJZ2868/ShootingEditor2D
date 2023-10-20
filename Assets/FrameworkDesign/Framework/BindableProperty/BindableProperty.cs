using System;
namespace FrameworkDesign
{
    //���ݺ����ݱ���¼��ķ��ͺϼ�
    public class BindableProperty<T> 
    {
        private Action<T> onValueChange = e => { };

        private T mValue = default(T);
        public T Value
        {
            get => mValue;
            set
            {
                if (!value.Equals(mValue))
                {
                    mValue = value;
                    onValueChange?.Invoke(value);
                }
            }

        }

        //��װBindableProperty��onValueChange
        public IUnRegister RegisterOnValueChange(Action<T> action)
        {
            onValueChange += action;

            return new BindablePropertyUnRegister<T>()
            {
                BindableProperty = this,
                OnValueChange = onValueChange
            };
        }
        public void UnRegisterOnValueChange(Action<T> action) 
        {
            onValueChange -= action;
        }


        //��BindableProperty��װ�¼�ϵͳ
        public class BindablePropertyUnRegister<T> : IUnRegister 
        {
            public BindableProperty<T> BindableProperty { get; set; }

            public Action<T> OnValueChange { get; set; }

            public void UnRegister()
            {
                BindableProperty.UnRegisterOnValueChange(OnValueChange);
                BindableProperty = null;
                OnValueChange = null;
            }
        }

    }
}
