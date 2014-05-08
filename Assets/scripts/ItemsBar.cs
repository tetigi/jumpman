using UnityEngine;
using System.Collections;

public class ItemsBar : MonoBehaviour {

	public GameObject dynamite;
	public GameObject jetpack;
	public GameObject trampoline;
	public GameObject actualTrampoline;
	public GameObject grenade;
	public Jumpman jumpman;
	public AudioSource grenadeSound;
	public AudioSource dynamiteSound;
	public AudioSource jetpackSound;

	private GameObject [] items;

	void Start() {
		items = new GameObject[4];
		items[0] = dynamite;
		items[1] = jetpack;
		items[2] = trampoline;
		items[3] = grenade;

		foreach (GameObject item in items) {
			if (!ApplicationModel.HasBoughtItem(item.name)) {
				item.renderer.enabled = false;
			}
		}
	}

	public bool CheckPress() {
		if ((jumpman.state == Jumpman.State.JUMP || jumpman.state == Jumpman.State.DOUBLE_JUMP) && Input.GetButtonDown("Fire1")) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit = new RaycastHit();

			if (Physics.Raycast(ray, out hit)) {
				// check which one got hit
				foreach (GameObject item in items) {
					if (item.renderer.enabled && item.collider.transform == hit.transform) {
						if (item.name == Items.TRAMPOLINE) {
							actualTrampoline.renderer.enabled = true;
              				ApplicationModel.RemoveItem(Items.TRAMPOLINE);
						} else if (item.name == Items.DYNAMITE) {
							jumpman.velocity = 10f;
							dynamiteSound.Play();
              				ApplicationModel.RemoveItem(Items.DYNAMITE);
						} else if (item.name == Items.GRENADE) {
							jumpman.velocity = 5f;
							grenadeSound.Play();
              				ApplicationModel.RemoveItem(Items.GRENADE);
						} else if (item.name == Items.JETPACK) {
							StartCoroutine(jumpman.Jetpack(jetpackSound));
						}
						item.renderer.enabled = false;
						return true;
					}
				}
			}
		}
		return false;
	}
}
