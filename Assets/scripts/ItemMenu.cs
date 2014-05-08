using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemMenu : MonoBehaviour {

	public Collider2D hitBox;

	private List<ItemMenuItem> entries = new List<ItemMenuItem>();

	private enum MouseState {UP, MISSED, DRAGGING};
	private MouseState state = MouseState.UP;
	private float lastMouseY = 0;

	private object syncLock = new object();
	public void RegisterItem(ItemMenuItem item) {
		lock(syncLock) {
			for (int i = 0; i < entries.Count; i ++) {
				if (entries[i].index > item.index) {
					entries.Insert(i, item);
					return;
				}
			}

			entries.Add(item);
		}
	}

	private void MoveMenu(float dist) {
		foreach (ItemMenuItem item in entries) {
			Vector3 pos = item.transform.position;
			item.transform.position = new Vector3(pos.x, pos.y + dist, pos.z);
		}
	}

	void Update () {
		if (Input.GetMouseButton(0)) {
			if (state == MouseState.DRAGGING) {
				float y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
				MoveMenu(y - lastMouseY);
				lastMouseY = y;
			} else if (hitBox.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition))) {
				if (state == MouseState.UP) {
					state = MouseState.DRAGGING;
					lastMouseY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
				}
			} else {
				state = MouseState.MISSED;
			}
		} else {
			state = MouseState.UP;
		}
	}
}
