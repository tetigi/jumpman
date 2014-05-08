using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour {

	private float screenX;
	private float cloudSpeed;
	private float x;
	private float startX = -0.4f;

	public tk2dSprite sprite;

	void Start () {
		screenX = Camera.main.pixelWidth;
		x = transform.position.x;

		cloudSpeed = 0.01f + (Random.value * 0.02f);

		int cloudNum = (int) (1 + (Random.value * 7));
		sprite.SetSprite("cloud" + cloudNum.ToString());
	}

	void Update() {
		float dist = cloudSpeed * Time.deltaTime;
		x += dist;
		UpdatePosition();

		// Cloud has floated away
		if (Camera.main.WorldToScreenPoint(transform.position).x > screenX) {
			x = startX;
			int cloudNum = (int) (1 + (Random.value * 7));
			sprite.SetSprite("cloud" + cloudNum.ToString());
		}
  	}

	void UpdatePosition () {
		transform.position = new Vector3(x, transform.position.y, transform.position.z);
	}
}
