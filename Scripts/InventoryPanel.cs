using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DrowTest {
    public class InventoryPanel : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
    {
        private Image _backbround;
        private Transform _inventoryView;
        public Transform slotParent => _inventoryView;
        private Image _theme;
        private Text _themeName;

        public Transform Tip =>_tip;
        private Transform _tip;
        public Text TipText =>_tipText;
        private Text _tipText;
 
        public GameObject slotPrefab;
        public GameObject itemPrefab;

        public Sprite[] sprites;

        private bool canDrag;

        
        private RectTransform _rectTransform;
        private void Awake()
        {
            _backbround = transform.Find("Bg").GetComponent<Image>();
            _inventoryView = transform.Find("inventoryView");
            _theme = transform.Find("Image").GetComponent<Image>();
            _themeName = transform.Find("Image/Text").GetComponent<Text>();
            _tip = transform.Find("Tip");
            _tipText = transform.Find("Tip/Text").GetComponent<Text>();

            _rectTransform = GetComponent<RectTransform>();
        }


        
        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            //Vector3 pos;
            //if(RectTransformUtility.ScreenPointToWorldPointInRectangle(GetComponent<RectTransform>(),eventData.position,null,out pos))
            //{
            //    GetComponent<RectTransform>().position =Vector3.one+ pos;
            //}
            if(canDrag)
            transform.position = eventData.position;
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            
            Vector2 point = Vector2.zero;
            point.x = eventData.position.x - (int)((transform.position.x - _rectTransform.sizeDelta.x / 2) * 100) * 0.01f;
            point.y = eventData.position.y - (int)((transform.position.y - _rectTransform.sizeDelta.y) * 100) * 0.01f;
            
            //限制可拖动位置
            if (point.y > 410f) canDrag = true;
 
             //屏幕坐标转视图坐标
            //GetComponent<RectTransform>().pivot = new Vector2(
            //    point.x / GetComponent<RectTransform>().sizeDelta.x,
            //    point.y / GetComponent<RectTransform>().sizeDelta.y);
        }
 
        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            canDrag = false;
        }
    }

}

