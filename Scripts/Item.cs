using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace DrowTest
{
    public class Item 
    {
        public string name;
        public int id;
        public int SlotID { get { return _slotId; } private set { if (_slotId != value ) { _slotId = value; OnChangeSlotId?.Invoke(); } } }
        private int _slotId;
        public string description;
        public int Count { get { return _count; } private set{ if (_count != value&&value>=0) { _count = value; OnChangeCount?.Invoke(); } } }
        private int _count;
        public Sprite icon;
        public ItemCtrl ctrl;

        public Item()
        {
            
        }
        public delegate void Change();
        public event Change OnChangeCount;
        public event Change OnStart;
        public event Change OnChangeSlotId;

        public void AddCount()
        {
            Count++;
        }
        public void AddCount(int count)
        {
            Count += count;
        }
        public void SetSlotID(int id)
        {
            SlotID = id;
        }
        public void Start()
        {
            OnStart?.Invoke();
        }
 
    }


    public class ItemCtrl
    {
        public Item item { get; private set; }
        public ItemView itemView { get; private set; }

        public Transform tip { get; private set; }
        public Text tipText { get; private set; }
        public ItemCtrl(Item item,ItemView itemView,Transform tip,Text tipText)
        {
            this.item = item;
            this.itemView = itemView;
            this.tip = tip;
            this.tipText = tipText;
            this.itemView.item = item;            
            this.item.ctrl = this;
            item.OnChangeCount += OnChangeCount;
            item.OnStart += OnStart;
            item.OnChangeSlotId += OnChangeSlotId;
        }

        private void OnChangeSlotId()
        {
            itemView.SetSolt();
        }

        private void OnChangeCount()
        {
            itemView.UpdateView(item.Count.ToString(),item.name,item.icon);
        }

        private void OnStart()
        {
            itemView.UpdateView(item.Count.ToString(), item.name, item.icon);
        }
 
        public void SetValue(string name, int id, int slotid, string description, int count, Sprite icon)
        {
            item.name = name;
            item.id = id;
            item.SetSlotID(slotid);
            item.description = description;
            item.AddCount();
            item.icon = icon;
            item.Start();
        }

        public string  Desc()
        {
            StringBuilder builder = new StringBuilder();
            //< size = 20 >< color =#FFEC58FF><b>chongwujingpo</b></color>
            //</ size >
            //0
            //6
            //test: chongwujingpo
            // chongwujingpo

            builder.AppendLine($"<size=20><color=#ffec58ff><b>{item.name}</b></color></size>");
            builder.AppendLine($"物品ID：<size=15><color=#ffffffff><b>{item.id.ToString()}</b></color></size>");
            builder.AppendLine($"数量：<size=15><color=#ffffffff><b>{item.Count.ToString()}</b></color></size>");
            builder.AppendLine($"描述：<size=15><color=#ffffffff><b>{item.description}</b></color></size>");
            return builder.ToString();
        }
    }

}
