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

            //获取子节点
            mObjs = GetComponentInChildren<Transform>(true);

            //记录血量变化时的XML
            var doc = new XmlDocument();
            doc.Load(APPConst.PlayerInfoXML);
            var PlayerInfoNode = doc.SelectSingleNode("PlayerInfo");

            //最大血量变化时更新视图
            this.GetModel<IPlayerModel>().MaxHP.RegisterOnValueChange(newValue =>
            {
                
                if (newValue >= maxHp)
                    add = true;
                else
                    add = false;
                maxHp = newValue;
                
                //更新视图
                foreach (Transform item in mObjs)
                {
                    if (newValue > 0)
                        item.gameObject.SetActive(true);
                    else
                        item.gameObject.SetActive(false);
                    newValue--;
                        //Debug.Log(item.gameObject.name);
                 }

                //最大生命值增加时当前生命值增加
                if (add)
                    this.GetModel<IPlayerModel>().HP.Value ++;

                //当前最大生命值减少且血量溢出时生命值减少
                if (!add&&maxHp < currentHP)
                {
                    this.GetModel<IPlayerModel>().HP.Value=maxHp;
                }

                //更新数据
                if (maxHp > 10)
                {
                    maxHp = this.GetModel<IPlayerModel>().MaxHP.Value = 10;
                }
                else if (maxHp < 0)
                {
                    maxHp = this.GetModel<IPlayerModel>().MaxHP.Value = 0;
                }
                //更新XML数据
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

            //当前血量变化时更新视图
            this.GetModel<IPlayerModel>().HP.RegisterOnValueChange(newValue =>
            {
                //更新视图
                currentHP = newValue;
                foreach (Transform item in mObjs)
                {
                    if (newValue >= 0)
                        item.GetComponent<Image>().fillAmount = newValue;
                    newValue--;
                    //Debug.Log(item.gameObject.name);
                }

                //更新数据
                if (currentHP > maxHp)
                    currentHP = this.GetModel<IPlayerModel>().HP.Value = maxHp;
                else if (currentHP < 0)
                    currentHP = this.GetModel<IPlayerModel>().HP.Value = 0;
                //更新XML
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

            //初始化显示最大血量
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