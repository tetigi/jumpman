using UnityEngine;
using System.Collections;

public class CreditView : MonoBehaviour {
	public TextMesh text;

	void Start () {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}

	void Update () {
		text.text = "$" + ApplicationModel.score.ToString();
	}
}