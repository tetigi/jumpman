using UnityEngine;

public class ActionNewGame : PressableBehaviour {
	public override void OnPress() {
		PlayerPrefs.DeleteAll();
		PlayerPrefs.Save();
		Camera.main.SendMessage("FadeOut", 1);
		Application.LoadLevel("jumpman_intro");
	}
}
