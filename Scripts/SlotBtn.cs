using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DrowTest
{
    public class SlotBtn : MonoBehaviour,IDropHandler
    { 
        public Button btn;
        public int id;

        

        public Action<int> OnUse;
        private void Awake()
        {
            btn = GetComponent<Button>();
        }
        // Start is called before the first frame update
        void Start()
        {
            btn.onClick.AddListener(()=> {
                var itemView = GetComponentInChildren<ItemView>();
                if (!itemView)
                {
                    return;
                }   
                OnUse?.Invoke(itemView.item.id);
                print("你使用了该物品");
            });
        }

        public void CleranChild()
        {
            if (transform.childCount > 0)
            {
                GetComponentInChildren<ItemView>().ClearnData();
                Destroy(transform.GetChild(0).gameObject);
            }
            
        } 

        public bool HasChild()
        {
            return transform.childCount > 0;
        }
         
        public void RemoveChild()
        {
            transform.GetChild(0).SetParent(GameObject.Find("Canvas").transform);
        }
        void IDropHandler.OnDrop(PointerEventData eventData)
        {
            var item = eventData.pointerDrag.GetComponent<ItemView>();
            if (!item) return;

            var itemSlotId = item.item.SlotID;
            //先确定格子有没有物品，有的话交换位置 
            var curItem = GetComponentInChildren<ItemView>();
            if (curItem)
            {
                curItem.item.SetSlotID(itemSlotId);
            }
            //没直接把拖拽的物品丢入
            item.item.SetSlotID(id);
        }
    }

}
