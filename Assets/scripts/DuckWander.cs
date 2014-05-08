using UnityEngine;
using System.Collections;

public class DuckWander : MonoBehaviour {
	public Collider2D pondLimit;

	private float vX = -0.01f;
	private float vY = -0.01f;
	private float x;
	private float y;

	enum Facing { RIGHT, LEFT };
	Facing facing = Facing.LEFT;

	void Start () {
		x = transform.position.x;
		y = transform.position.y;
	}
	
	void Update () {
		x += vX * Time.deltaTime;
		y += vY * Time.deltaTime;

		if (!pondLimit.OverlapPoint(new Vector2(x, y))) {
			vX *= -1;
			vY *= -1;
			return;
		} 

		if (Random.value > 0.99) {
			vY *= -1;
		}

		if (Random.value > 0.999) {
			vX *= -1;
		}

		UpdatePosition();
	}

	void UpdatePosition () {
		transform.position = new Vector3(x, y, this.transform.position.z);
		if (vX > 0 && facing == Facing.LEFT) {
			transform.Rotate(new Vector3(0, 180, 0));
			facing = Facing.RIGHT;
		} else if (vX < 0 && facing == Facing.RIGHT) {
			transform.Rotate(new Vector3(0, 180, 0));
			facing = Facing.LEFT;
		}
	}
		                       	
}
