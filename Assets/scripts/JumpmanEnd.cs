using UnityEngine;
using System.Collections;

public class JumpmanEnd : MonoBehaviour {
	bool done = false;
	void Start () {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}
	void Update() {
		Vector3 pos = transform.position;
		float rot = transform.eulerAngles.z;

		transform.position = new Vector3(pos.x + 0.001f, pos.y + 0.002f, pos.z);
		transform.eulerAngles = new Vector3(0, 0, rot + 1);

		Vector3 worldPos = Camera.main.WorldToScreenPoint(transform.position);
		if (!done && worldPos.x > Screen.width && worldPos.y > Screen.height) {
			Camera.main.SendMessage("FadeOut", 0.2f);
			done = true;
			StartCoroutine(GoToCredits());
		}
	}

	IEnumerator GoToCredits() {
		yield return new WaitForSeconds(3);
		PlayerPrefs.SetInt("intro", 0);
		Application.LoadLevel("jumpman_credits");
	}
}