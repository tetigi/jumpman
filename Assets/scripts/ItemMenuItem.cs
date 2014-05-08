using UnityEngine;
using System.Collections;

public class ItemMenuItem : MonoBehaviour {
	public string itemName;
	public ItemMenu owner;
	public int index;

	void Start() {
		owner.RegisterItem(this);
	}
}
