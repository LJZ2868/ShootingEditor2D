using UnityEngine;
using FrameworkDesign;
using UnityEngine.UI;
using System.Xml;

namespace ShootEditor2D
{
    public class HPController : BaseUIPanel,IController
    {
        public static Transform mObjs;

        public IIOC GetIOC()
        {
            return ShootEditor2D.Interface;
        }

        private void Awake()
        {

            var currentHP = this.GetModel<IPlayerModel>().HP.Value;
            var maxHp = this.GetModel<IPlayerModel>().MaxHP.Value;
            bool add = false ;

            //��ȡ�ӽڵ�
            mObjs = GetComponentInChildren<Transform>(true);

            //��¼Ѫ���仯ʱ��XML
            var doc = new XmlDocument();
            doc.Load(APPConst.PlayerInfoXML);
            var PlayerInfoNode = doc.SelectSingleNode("PlayerInfo");

            //���Ѫ���仯ʱ������ͼ
            this.GetModel<IPlayerModel>().MaxHP.RegisterOnValueChange(newValue =>
            {
                
                if (newValue >= maxHp)
                    add = true;
                else
                    add = false;
                maxHp = newValue;
                
                //������ͼ
                foreach (Transform item in mObjs)
                {
                    if (newValue > 0)
                        item.gameObject.SetActive(true);
                    else
                        item.gameObject.SetActive(false);
                    newValue--;
                        //Debug.Log(item.gameObject.name);
                 }

                //�������ֵ����ʱ��ǰ����ֵ����
                if (add)
                    this.GetModel<IPlayerModel>().HP.Value ++;

                //��ǰ�������ֵ������Ѫ�����ʱ����ֵ����
                if (!add&&maxHp < currentHP)
                {
                    this.GetModel<IPlayerModel>().HP.Value=maxHp;
                }

                //��������
                if (maxHp > 10)
                {
                    maxHp = this.GetModel<IPlayerModel>().MaxHP.Value = 10;
                }
                else if (maxHp < 0)
                {
                    maxHp = this.GetModel<IPlayerModel>().MaxHP.Value = 0;
                }
                //����XML����
                foreach (XmlElement item in PlayerInfoNode)
                {
                    if(item.GetAttribute("name") == "_MaxHP")
                    {
                        item.SetAttribute("value", maxHp.ToString());
                    }
                }
                doc.Save(APPConst.PlayerInfoXML);

                Debug.Log("maxhp: " + this.GetModel<IPlayerModel>().MaxHP.Value);
            });

            //��ǰѪ���仯ʱ������ͼ
            this.GetModel<IPlayerModel>().HP.RegisterOnValueChange(newValue =>
            {
                //������ͼ
                currentHP = newValue;
                foreach (Transform item in mObjs)
                {
                    if (newValue >= 0)
                        item.GetComponent<Image>().fillAmount = newValue;
                    newValue--;
                    //Debug.Log(item.gameObject.name);
                }

                //��������
                if (currentHP > maxHp)
                    currentHP = this.GetModel<IPlayerModel>().HP.Value = maxHp;
                else if (currentHP < 0)
                    currentHP = this.GetModel<IPlayerModel>().HP.Value = 0;
                //����XML
                foreach (XmlElement item in PlayerInfoNode) 
                {
                    if (item.GetAttribute("name") == "_HP")
                    {
                        item.SetAttribute("value", currentHP.ToString());
                    }
                }
                doc.Save(APPConst.PlayerInfoXML);

                Debug.Log("hp: " + this.GetModel<IPlayerModel>().HP.Value);
            });

            //��ʼ����ʾ���Ѫ��
            foreach (Transform item in mObjs)
            {
                if (maxHp > 0)
                {
                    item.gameObject.SetActive(true);
                    item.GetComponent<Image>().fillAmount = currentHP;
                }
                else
                    item.gameObject.SetActive(false);
                maxHp--;
                currentHP--;

                //item.GetComponent<Image>().fillAmount=
                //Debug.Log(item.gameObject.name);
            }
            currentHP = this.GetModel<IPlayerModel>().HP.Value;
            maxHp = this.GetModel<IPlayerModel>().MaxHP.Value;
        }

        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.A))
            //{
            //    this.GetModel<IPlayerModel>().HP.Value += 0.5f;
            //}

            //if (Input.GetKeyDown(KeyCode.B))
            //{
            //    this.GetModel<IPlayerModel>().HP.Value -= 0.5f;
            //}

            //if (Input.GetKeyDown(KeyCode.C))
            //{
            //    this.GetModel<IPlayerModel>().MaxHp.Value++;
            //}

            //if (Input.GetKeyDown(KeyCode.D))
            //{
            //    this.GetModel<IPlayerModel>().MaxHp.Value--;
            //}
        }


    }
}