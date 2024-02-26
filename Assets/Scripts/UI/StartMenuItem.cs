using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuItem : MonoBehaviour {
	[SerializeField]
	TMP_Text _text;

	[SerializeField]
	private Image _image;

	[SerializeField]
	private Button _button;

	public event EventHandler<int> Selected;

	public int Index { get; set; }

	public bool Interactable {
		get => _button.interactable;
		set {
			_button.interactable = value;
		}
	}

	public string Name {
		get => _text.text;
		set => _text.text = value;
	}

	public Sprite Icon {
		get => _image.sprite;
		set => _image.sprite = value;
	}

	public Color Color {
		get => _text.color;
		set => _text.color = value;
	}

	public void Select() {
		Selected?.Invoke(this, Index);
	}
}