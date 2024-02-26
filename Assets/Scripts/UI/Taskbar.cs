using UnityEngine;
using UnityEngine.UI;

public class Taskbar : MonoBehaviour {
	[SerializeField]
	private Image _image;

	[SerializeField]
	private Sprite _openSprite;

	private Sprite _closeSprite;


	void Awake() {
		_closeSprite = _image.sprite;
	}

	public void Close() {
		_image.sprite = _closeSprite;
	}

	public void Open() {
		_image.sprite = _openSprite;
	}
}