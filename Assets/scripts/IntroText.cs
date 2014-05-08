using UnityEngine;
using System.Collections;

public class IntroText : MonoBehaviour {
	string msg = "A long time ago, in a much simpler time, there lived a man who longed to jump like no other man.\n\n" +
		"He did not yearn for fame, or for fortune. He simply longed to be able to jump higher than anyone had ever jumped before.\n\n" +
			"This man's name?\n\n";
	string endMsg = "...\n\n\nJUMP\n\nMAN";

	public TextMesh messageBox;

	IEnumerator Start () {
		string curMsg = "";
		int counter = 0;
		
		for (int i = 0; i < msg.Length; i ++) {
			if (counter >= 27) { // e
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
			
			yield return new WaitForSeconds(0.05f);
		}

		for (int i = 0; i < endMsg.Length; i ++) {
			if (counter >= 27) { // e
				if (endMsg[i] != ' ') {
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
			curMsg += endMsg[i];
			counter ++;
			if (endMsg[i] == '\n') {
				counter = 0;
			}
			messageBox.text = curMsg;
			
			yield return new WaitForSeconds(0.4f);
		}
	}
}

