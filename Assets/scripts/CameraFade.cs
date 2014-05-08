using UnityEngine;
using System;

public class CameraFade : MonoBehaviour {
	public Texture2D fadeOutTexture;
	public float fadeSpeed;

	int drawDepth = -10;
	private float alpha = 1.0f;
	private int fadeDir = -1;

	void OnGUI() {
		alpha += fadeDir * fadeSpeed * Time.deltaTime;
		alpha = Mathf.Clamp01(alpha);

		Color c = GUI.color;
		c.a = alpha;
		GUI.color = c;
		GUI.depth = drawDepth;
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
	}

	public void FadeIn() {
		fadeDir = -1;
	}

	void FadeOut(float fadeSpeed) {
		this.fadeSpeed = fadeSpeed;
		fadeDir = 1;
	}


	void Start() {
		alpha = 1;
		FadeIn();
	}
}