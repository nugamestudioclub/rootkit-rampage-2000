using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuItem : MonoBehaviour {
	[SerializeField]
	TMP_Text _text;

	[SerializeField]
	private Image _image;

	public event EventHandler<int> Selected;

	public int Index { get; set; }

	public string Name {
		get => _text.text;
		set => _text.text = value;
	}

	public Sprite Icon {
		get => _image.sprite;
		set => _image.sprite = value;
	}

	public void Select() {
		Selected?.Invoke(this, Index);
	}
}