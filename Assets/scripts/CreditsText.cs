using UnityEngine;
using System.Collections;

public class CreditsText : MonoBehaviour {
	string msg = "Finally, after many trials, he had done it; he, Jump Man, had jumped higher than any other.\n\n" +
		"Indeed, his was a jump from which there would be no landing, for how can one land without ground?\n\n" +
			"His quest complete, Jump Man glided onwards, towards the stars...\n" +
			"                   \nLegends say, he is still out there, jumping. When will he return? We may never know." +
			"\n               \n..unless we make a sequel." +
			"\n\nThanks for playing!\n-The Jumpman team" +
			"\n\nMusic:\nA Safe Place to Sleep - Fastfall - Lifeformed";

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
	}
}

