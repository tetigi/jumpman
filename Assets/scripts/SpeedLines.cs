using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpeedLines : MonoBehaviour {

	public tk2dSprite speedLine;
	public Jumpman jumpman;
	public Collider2D bounds;

	private List<tk2dSprite> lines;

	bool allActive = false;

	void Start () {
		speedLine.renderer.enabled = false;
		lines = new List<tk2dSprite>();

		for (int i = 0; i < 5; i++) {
			tk2dSprite line = (tk2dSprite) Instantiate(speedLine);
			line.renderer.enabled = false;
			Vector3 oldPos = line.transform.position;
			line.transform.position = new Vector3(0.2f + (i * 0.1f), oldPos.y, oldPos.z);
			lines.Add(line);
		}
	}
	
	void Update () {
		if (! allActive && Mathf.Abs(jumpman.velocity) > 5) {
			SetAllLines(true);
			allActive = true;
		} else if (allActive && Mathf.Abs(jumpman.velocity) <= 5) {
			SetAllLines(false);
			allActive = false;
		}

		if (allActive) {
			MoveLines();
		}
	}

	// TODO actually implement this
	void MoveLines () {
		//foreach (tk2dSprite line in lines) {
		//}
	}

	Vector3 CalcNewPos(tk2dSprite line, float dist) {
		Vector3 oldPos = line.transform.position;
		return new Vector3(oldPos.x, oldPos.y + dist, oldPos.z);
	}

	void SetAllLines (bool value) {
		foreach (tk2dSprite line in lines) {
			line.renderer.enabled = value;
		}
	}
}