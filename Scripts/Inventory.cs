using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

namespace DrowTest
{
    public class Inventory  
    {
        public static Inventory Instance { get { if (_instance == null) { _instance = new Inventory(); return _instance; }return _instance; } private set { } }
        private static Inventory _instance;
        //private


        //public
        public int slotCount = 35;
        public readonly List<SlotBtn> slots = new List<SlotBtn>();
        public List<Item> items = new List<Item>();



        private Inventory()
        {
            
        }
        
    }
    public class InventoryControll
    {
         

        public Inventory inventory { get; private set; }
        public InventoryPanel inventoryPanel { get; private set; }
        public InventoryControll(Inventory inventory, InventoryPanel inventoryPanel)
        {
            this.inventory = inventory;
            this.inventoryPanel = inventoryPanel;
            GetItems();
            sortByCustom = (x,y) => { return x.SlotID.CompareTo(y.SlotID); };
        }
 
        public void GetItems()
        {
            var items = inventory.items;
            var id = 0;
            foreach (var sprite in inventoryPanel.sprites)
            {

                Item item = new Item()
                {
                    icon = sprite,
                    id = id,
                    description = $"test:{sprite.name}",
                    name = sprite.name
                };
                item.AddCount(5);
                items.Add(item);
                id++;
            }
            foreach (var item in items)
            {
                Debug.Log($"{item.name}:{item.id}");
            }
        }

        public void CreateSlot()
        {
            
            for (int i = 0; i < inventory.slotCount; i++)
            {
               var slot = GameObject.Instantiate(inventoryPanel.slotPrefab, inventoryPanel.slotParent).GetComponent<SlotBtn>();
               inventory.slots.Add(slot);
            }

            for (int i = 0; i < inventory.slots.Count; i++)
            {
                inventory.slots[i].id = i;
                inventory.slots[i].OnUse = RemoveItem;
            }
        }
 
        public void AddItem(int itemId)
        {
            Item itemToAdd = null;
            //判断所有格子中是否包含了要添加得物品
            if(inventory.slots.Exists((slot) => {
                if (!slot.GetComponentInChildren<ItemView>()) return false;
                
                if(slot.GetComponentInChildren<ItemView>().item.id == itemId)
                {
                    itemToAdd = slot.GetComponentInChildren<ItemView>().item;
                }
 
                return slot.GetComponentInChildren<ItemView>().item.id == itemId;   
            }))
            {

                itemToAdd.AddCount();
            }
 
            if (itemToAdd==null)
            {
                //寻找空格子，并实例化要添加的item
                foreach (var slot in inventory.slots)
                {
                    if (slot.transform.childCount <= 0)
                    {
                        itemToAdd = inventory.items.Find((item) => {return (item.id == itemId); });
                        if (itemToAdd == null) return;
                        var itemview = GameObject.Instantiate(inventoryPanel.itemPrefab, slot.transform).GetComponent<ItemView>();

                        ItemCtrl itemCtrl = new ItemCtrl(itemToAdd, itemview,inventoryPanel.Tip,inventoryPanel.TipText);
                        itemCtrl.SetValue(itemToAdd.name,itemToAdd.id,slot.id,itemToAdd.description,itemToAdd.Count,itemToAdd.icon);
                        break;
                    }
                }
            }
 
        }

        public void RemoveItem(int itemId)
        {
            Item itemToRemove = null;
            SlotBtn slotHasItem = null;
            //判断所有格子中是否包含了要移除的物品
            if (inventory.slots.Exists((slot) => {
                if (!slot.GetComponentInChildren<ItemView>()) return false;

                if (slot.GetComponentInChildren<ItemView>().item.id == itemId)
                {
                    itemToRemove = slot.GetComponentInChildren<ItemView>().item;
                    slotHasItem = slot;
                }

                return slot.GetComponentInChildren<ItemView>().item.id == itemId;
            }))
            {
                itemToRemove.AddCount(-1);
                if (itemToRemove.Count == 0)
                {
                    Debug.Log($"if itrm count equie zero:{itemToRemove.Count},remove from slot");
                    slotHasItem.CleranChild();
                }
            }
        }

        public delegate int ItemSortBy(Item x,Item y);
        //自定义排序
        public ItemSortBy sortByCustom;
        /// <summary>
        /// 物品排序
        /// </summary>
        public void Sort()
        {
            var slots = inventory.slots.FindAll((slot) => { return slot.GetComponentInChildren<ItemView>(); });
            List<Item> listItem = new List<Item>();
            foreach (var slot in slots)
            {
                listItem.Add(slot.GetComponentInChildren<ItemView>().item);
            }

            SortItem(listItem);

            for (int i = 0; i < listItem.Count; i++)
            {
                if (!inventory.slots[i].HasChild())
                {
                    
                      listItem[i].SetSlotID(i);
 
                }
                else
                {
                    var id = listItem[i].SlotID;
                    //先把当前排序的物体移除格子,让给要替换格子的物体
                    //当前排序的物体与要替换格子的物体相同时，不需要移除
                    if(id!=i)
                    inventory.slots[id].RemoveChild();
                    //因为上面把格子里的物体移除了，是空的，所以无法获取物品会报错
                    inventory.slots[i].GetComponentInChildren<ItemView>().item.SetSlotID(id);
                    
                    listItem[i].SetSlotID(i);
  
                }

            }
            
        }


        public enum SortBy { 
        ID,
        COUNT,
        NAME,
        CUSTOM
        }
        public SortBy sortBy;
        private void SortItem(List<Item> items)
        {
            switch (sortBy)
            {
                case SortBy.ID:
                    items.Sort(SortByID);
                    break;
                case SortBy.COUNT:
                    items.Sort(SortByCOUNT);
                    break;
                case SortBy.NAME:
                    items.Sort(SortByNAME);
                    break;
                case SortBy.CUSTOM:
                    items.Sort(SortByCUSTOM);
                    break;
                default:
                    break;
            }
        }

        private int SortByID(Item x, Item y)
        {
            int result = 0;
            if (x.id > y.id) return result = 1;
            if (x.id == y.id) return result = 0;
            if (x.id < y.id) return result = -1;
            return result;
        }
        private int SortByCOUNT(Item x, Item y)
        {
            int result = 0;
            if (x.Count > y.Count) return result = 1;
            if (x.Count == y.Count) return result = 0;
            if (x.Count < y.Count) return result = -1;
            return result;
        }
        private int SortByNAME(Item x, Item y)
        {
            return x.name.CompareTo(y.name);
        }
        private int SortByCUSTOM(Item x, Item y)
        {
            return sortByCustom(x,y);
        }
    }

}
