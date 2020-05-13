using DrowTest;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
using haha = DrowTest.Inventory;
public class InventoryTest : MonoBehaviour
{
    public InventoryControll ctrl;
   
    // Start is called before the first frame update
    void Start()
    {
       
        ctrl = new InventoryControll(haha.Instance, GameObject.Find("InventoryPanel(Clone)").GetComponent<InventoryPanel>());
        ctrl.CreateSlot();
 
        ctrl.AddItem(2);
        ctrl.AddItem(2);
        ctrl.AddItem(2);
        ctrl.AddItem(2);
        ctrl.AddItem(2);
        ctrl.AddItem(2);
        ctrl.AddItem(2);
        ctrl.AddItem(1);
        ctrl.AddItem(1);
        ctrl.AddItem(1);
        ctrl.AddItem(1);
        ctrl.AddItem(1);
        ctrl.AddItem(1);
        ctrl.AddItem(1);
        ctrl.AddItem(1);
        ctrl.AddItem(1);
        ctrl.AddItem(1);
        ctrl.AddItem(1);
        ctrl.AddItem(1);
        ctrl.AddItem(1);
        ctrl.AddItem(1);
        ctrl.AddItem(0);


         
        StartCoroutine(Test());
    }
    [SerializeField]
    public InventoryControll.SortBy sortBy;
    IEnumerator Test()
    {
        yield return new WaitForSeconds(1f);
       // ctrl.RemoveItem(2);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("s"))
        {
            ctrl.sortBy = sortBy;
            
            ctrl.Sort();
        }
    }
}
