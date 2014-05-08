using UnityEngine;
using System.Collections;

public class ChargeBar : MonoBehaviour {
	public GameObject indicator;
	private float top = 0.16f;
	private float bottom = -0.16f;
	
	public void SetPower(int power) {
		float dist = (top - bottom)/100;
		ChangeY(bottom + (dist * power));
	}

	public void Show() {
		renderer.enabled = true;
		indicator.renderer.enabled = true;
	}

	public void Hide() {
		renderer.enabled = false;
		indicator.renderer.enabled = false;
	}

	private void ChangeY(float y) {
		Vector3 pos = indicator.transform.localPosition;
		indicator.transform.localPosition = new Vector3(pos.x, y, pos.z);
	}
}