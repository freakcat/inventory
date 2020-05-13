using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DrowTest
{

    public class ItemView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerEnterHandler,IPointerExitHandler
    {
        private Text _amount;
        private Text _name;
        private Image _icon;
        private RectTransform rectTransform;
        private CanvasGroup _canvasGroup;

        public Item item;

        private void Awake()
        {
            _amount = transform.Find("count").GetComponent<Text>();
            _name = transform.Find("name").GetComponent<Text>();
            _icon = GetComponent<Image>();
            rectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        void Start()
        {
            
        }
        public void ClearnData()
        {
            item.ctrl.tip.localScale = Vector3.zero;
            item.ctrl = null;
            item = null;
        }

        public void UpdateView(string count, string name, Sprite icon)
        {
            _amount.text = count;
            _name.text = name;
            _icon.sprite = icon;
        }

        public void SetSolt()
        {
            if (!Inventory.Instance.slots[item.SlotID].HasChild())
                transform.SetParent(Inventory.Instance.slots[item.SlotID].transform);
            rectTransform.localPosition = Vector3.zero;
        }
 
        void Update()
        {

        }
        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = false;
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            rectTransform.position = eventData.position;
            transform.SetParent(GameObject.Find("Canvas").transform);

        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;
            SetSolt();
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            item.ctrl.tipText.text = item.ctrl.Desc();
            item.ctrl.tip.localScale = Vector3.one;
            item.ctrl.tip.position = eventData.position;
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            item.ctrl.tip.localScale = Vector3.zero;
        }

 
    }

}
