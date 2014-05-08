using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {
	public tk2dSprite sprite;
	public Collider2D hitBox;
	public PressableBehaviour action;
	public AudioSource clickUp;
	public AudioSource clickDown;

	private bool buttonEnabled = true;
	enum ButtonState { UP, DOWN };
	ButtonState currentState = ButtonState.UP;

	public void Enable() {
		buttonEnabled = true;
	}

	public void Disable() {
		buttonEnabled = false;
	}
		
	void Update () {
		if (Input.GetMouseButton(0)) {
			if (hitBox.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition))) {
				sprite.SetSprite("down");
				if (currentState == ButtonState.UP) {
					clickDown.Play();
				}
				currentState = ButtonState.DOWN;
			}
		} else {
			sprite.SetSprite("up");
			if (currentState == ButtonState.DOWN) {
				if (buttonEnabled && hitBox.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition))) {
					action.OnPress();
				}
				currentState = ButtonState.UP;
				clickUp.Play ();
			}
		}
	}
}

