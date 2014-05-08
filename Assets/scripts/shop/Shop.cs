using UnityEngine;
using System.Collections;
using System.Reflection;

public class Shop : MonoBehaviour {
	public ShopItem jetpack;
	//public ShopItem boots;
	public ShopItem trampoline;
	public ShopItem jumpPotion;
	public ShopItem crouch;
	public ShopItem doubleJump;
	public ShopItem hat;
	public ShopItem saiyanPotion;
	public ShopItem stunt;
	public ShopItem grenade;
	public ShopItem dynamite;
	public TextMesh messageBox;
	public tk2dSprite buyButton;
	public AudioSource chaching;

	public ShopItem [] items;

	private ShopItem selectedItem = null;

	IEnumerator Start () {
		yield return new WaitForSeconds(0.1f);
		items = new ShopItem[10];
		items[0] = jetpack;
		items[1] = trampoline;
		items[2] = jumpPotion;
		items[3] = crouch;
		items[4] = doubleJump;
		items[5] = hat;
		items[6] = saiyanPotion;
		items[7] = stunt;
		items[8] = grenade;
		items[9] = dynamite;
		//items[1] = boots;

		if (PlayerPrefs.HasKey("visited_shop")) {
			System.Type type = typeof(Strings);
			string staticName = "WELCOME_MESSAGE_" + ((int) (1 + (Random.value * 3))).ToString();
			FieldInfo field = type.GetField(staticName);
			StartCoroutine(PutMessage(field.GetValue(new Strings()).ToString()));
		} else { //  first time here
			StartCoroutine(PutMessage ("Hey, you're new around these parts! Have a jump potion on the house."));
			PlayerPrefs.SetInt("visited_shop", 0);
			jumpPotion.Buy();
		}

		buyButton.renderer.enabled = false;
	}

	private bool messagePrinting = false;
	private bool messageWaiting = false;

	public IEnumerator PutMessage(string msg) {
		string curMsg = "";
		int counter = 0;

		if (messagePrinting) {
			messageWaiting = true;
			while (messageWaiting) {
				yield return new WaitForSeconds(0.01f);
			}
		}
		messagePrinting = true;
		for (int i = 0; i < msg.Length; i ++) {
			if (counter >= 27) {
				if (msg[i] != ' ') {
					int j = curMsg.Length-1;
					while (curMsg[j] != ' ') {
						j --;
					}

					curMsg = curMsg.Substring(0, j) + "\n" + curMsg.Substring(j+1);

				} else {
					curMsg += "\n";
				}
				counter = 0;
			}
			curMsg += msg[i];
			counter ++;
			if (msg[i] == '\n') {
				counter = 0;
			}
			messageBox.text = curMsg;

			if (messageWaiting) {
				messageWaiting = false;
				break;
			}

			yield return new WaitForSeconds(0.02f);
		}
		messagePrinting = false;
	}

	public void PurchaseSelected() {
		if (selectedItem != null) {
			if (selectedItem.Cost () <= ApplicationModel.score) {
				selectedItem.Deselect();
				StartCoroutine(PutMessage(Strings.THANKYOU_MESSAGE));
				ApplicationModel.score -= selectedItem.Cost();
				selectedItem.Buy();
				selectedItem = null;
				chaching.Play();
			} else {
				StartCoroutine(PutMessage(Strings.NOT_ENOUGH_CREDIT));
			}
		}
	}

	void Update () {
		foreach (ShopItem item in items) {
			if (item.InStock() && item.Cost() > ApplicationModel.score) {
				Color c = item.sprite.color;
				c.a = 0.7f;
				c.r *= 0.7f;
				c.b *= 0.7f;
				c.g *= 0.7f;
				item.sprite.color = c;
			}
		}
		if (selectedItem != null) {
			buyButton.renderer.enabled = true;
		}
		if (Input.GetButtonDown("Fire1")) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit = new RaycastHit();

			if (Physics.Raycast(ray, out hit)) {
				// check which one got hit
				foreach (ShopItem item in items) {
					if (item.collider.transform == hit.transform) {
						if (selectedItem != item) {
							audio.Play();
							item.OnSelect();
							if (selectedItem != null) {
								selectedItem.Deselect();
							}
							selectedItem = item;
							StartCoroutine(PutMessage(selectedItem.GetMessage()));
						}
						break;
					}
				}
			}
		} else if (Input.GetKeyDown(KeyCode.Escape)) {
			PlayerPrefs.Save();
			Application.LoadLevel("jumpman_main");
		}
	}
}