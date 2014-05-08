using UnityEngine;

public class ActionChangeScene : PressableBehaviour {
	public string scene;
	public override void OnPress() {
		PlayerPrefs.SetInt ("score", ApplicationModel.score);
		PlayerPrefs.SetString ("items", ApplicationModel.ItemsAsString());
		PlayerPrefs.Save();
		Camera.main.SendMessage("FadeOut", 1);
		Application.LoadLevel(scene);
	}
}
