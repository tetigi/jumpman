using UnityEngine;

public class ActionStartGame : PressableBehaviour {
	public override void OnPress() {
		if (PlayerPrefs.HasKey("score")) {
			Camera.main.SendMessage("FadeOut", 1);
			Application.LoadLevel ("jumpman_loadscreen");
		} else {
			Camera.main.SendMessage("FadeOut", 1);
			Application.LoadLevel("jumpman_intro");
		}
	}
}
