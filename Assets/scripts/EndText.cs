using UnityEngine;
using System.Collections;

public class EndText : MonoBehaviour {
	private float alpha = 0;
	private float dAlpha = 0;
	public TextMesh text;

	void Update() {
		alpha += dAlpha;
		Color c = text.color;
		c.a = Mathf.Clamp01(alpha);
		text.color = c;
	}

	public void FadeIn() {
		dAlpha = 0.001f;
	}
}