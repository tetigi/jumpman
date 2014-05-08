using UnityEngine;
using System.Collections;

public class ActionPurchaseSelected : PressableBehaviour {
	public Shop shop;
	public override void OnPress () {
		shop.PurchaseSelected();
	}
}
