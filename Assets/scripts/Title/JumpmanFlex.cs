using UnityEngine;
using System.Collections;

public class JumpmanFlex : MonoBehaviour {

	public tk2dSprite sprite;

	private string[] poses = {	"flex_both",
								"flex_left",
								"flex_right",
								"man" };
	private bool posing = false;

	void Update () {
		if (!posing) {
			posing = true;
			StartCoroutine(StrikeNewPose());
		}
	}

	IEnumerator StrikeNewPose() {
		int poseIndex = (int) (Random.value * 4);
		sprite.SetSprite(poses[poseIndex]);
		yield return new WaitForSeconds(2);
		posing = false;
	}
}
