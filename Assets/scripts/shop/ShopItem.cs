using UnityEngine;
using System.Collections;
using System.Reflection;
using System;

public class ShopItem : MonoBehaviour {
	public tk2dSprite sprite;
	public int quantity;

	private float originalY;
	private bool selected = false;
	private bool bought = false;

	void Start() {
		originalY = this.transform.position.y;

		if (this.GetLevel() > quantity) {
			bought = true;
			Darken();
		}
	}

	private int GetLevel() {
		return 1 + ApplicationModel.CountItem(name);
	}

	public string GetMessage() {
		Type type = typeof(Strings);
		string staticName = name.ToUpper() + "_DESC";
		if (quantity > 1) {
			staticName += "_" + GetLevel().ToString();
		}
		FieldInfo field = type.GetField(staticName);
		return field.GetValue(new Strings()).ToString();
	}

	public bool InStock() {
		return GetLevel() <= quantity;
	}

	public int Cost() {
		Type type = typeof(Items);
		string staticName = name.ToUpper() + "_COST";
		if (quantity > 1) {
			staticName += "_" + GetLevel().ToString();
		}
		FieldInfo field = type.GetField(staticName);
		return (int) field.GetValue(new Items());
	}

	public void Buy() {
		if (!bought) {
			ApplicationModel.AddItem(name);
			if (this.GetLevel() > quantity) {
				bought = true;
				Darken ();
			}
		}
	}

	private void Darken() {
		sprite.color = Color.clear;	
	}

	public void OnSelect() {
		if (!selected) {
			// float up
			StartCoroutine(FloatTo (originalY + 0.05f));
			// display tooltip
			selected = true;
		} 
	}

	public void Deselect() {
		if (selected) {
			// float down to normal position
			StartCoroutine(FloatTo (originalY));
			// hide tooltip
			selected = false;
		}
	}

	private IEnumerator FloatTo(float y) {
		float speed = 0.004f;
		float originalDist = Mathf.Abs(y - this.transform.position.y);
		if (this.transform.position.y < y) {
			while (this.transform.position.y < y) {
				float dist = y - this.transform.position.y;
				float ratio = dist / originalDist;
				Vector3 pos = this.transform.position;
				this.transform.position = new Vector3(pos.x, pos.y + Mathf.Max (0.001f, speed * ratio), pos.z);
				yield return new WaitForSeconds(0.01f);
				if (!selected) {
					break;
				}
			}
		} else {
			while (this.transform.position.y > y) {
				Vector3 pos = this.transform.position;
				this.transform.position = new Vector3(pos.x, pos.y - 0.002f, pos.z);
				yield return new WaitForSeconds(0.01f);
				if (selected) {
					break;
				}
			}
		}
	}
}