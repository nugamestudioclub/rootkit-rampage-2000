using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField]
    private bool _isOpen;

    public bool IsOpen { get => _isOpen; private set => _isOpen = value; }

    [SerializeField]
    private Transform _list;

    [SerializeField]
    private GameObject _listItem;

    [SerializeField]
    private float _animationLength;

    [SerializeField]
    private Vector3 _offset;

    public event EventHandler<int> ItemSelected;

    [SerializeField]
    private Color _enabledColor = Color.black;

    [SerializeField]
    private Color _disabledColor = Color.red;

    public int Count { get; private set; }

    private void Awake()
    {
        if (!IsOpen)
        {
            transform.position += _offset;
        }
    }

    public void AddItem(string name, Sprite icon, bool interactable)
    {
        int index = Count++;
        var obj = Instantiate(_listItem, _list);
        var item = obj.GetComponent<StartMenuItem>();
        item.Name = name;
        item.Icon = icon;
        item.Index = index;
        item.Selected += Item_Selected;
        item.Interactable = interactable;
        item.Color = interactable ? _enabledColor : _disabledColor;

    }

    public void Clear()
    {
        Count = 0;
        for (int i = 0; i < _list.childCount; ++i)
        {
            var child = _list.GetChild(i).gameObject;
            if (child.TryGetComponent<StartMenuItem>(out var item))
            {
                item.Selected -= Item_Selected;
                Destroy(child);
            }
        }
    }

    public void Close()
    {
        if (IsOpen)
        {
            transform.position += _offset;
            IsOpen = false;
        }

    }

    public void Open()
    {
        if (!IsOpen)
        {
            transform.position -= _offset;
            IsOpen = true;
        }

    }

    public event EventHandler<int> Clicked;

    public void SelectItem(int index)
    {
        ItemSelected?.Invoke(this, index);
    }

    private void Item_Selected(object sender, int index)
    {
        SelectItem(index);
    }
}