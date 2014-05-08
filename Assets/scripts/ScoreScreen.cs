using UnityEngine;
using System.Collections;

public class ScoreScreen : MonoBehaviour {
	public TextMesh scoreDisplay;
	public TextMesh scoreWorkOutDisplay;
	public TextMesh textDisplay;
	public Jumpman jumpman;
	public Button retry;
	public Button shop;
	public CashMonies cashMonies;

	public TextMesh heightLevel;
	public TextMesh airTimeLevel;
	public TextMesh speedLevel;
	public TextMesh cashLevel;

	private float maxHeight = 0;
	private float airTime = 0;
	private float originalY;

	private float lastRotation = 0;
	private float halfWayDirection = 0;

	void Start () {
		SetRender(false);

		originalY = transform.position.y;
		UpdateYPos(transform.position.y - 1.3f);
	}

	private void SetRender(bool b) {
		renderer.enabled = b;
		retry.renderer.enabled = b;
		shop.renderer.enabled = b;
		scoreDisplay.renderer.enabled = b;
		textDisplay.renderer.enabled = b;
	}

	public IEnumerator SlideIn() {

		UpdateYPos(transform.position.y - 1.3f);
		SetRender(true);
		yield return new WaitForSeconds(0.5f);
		yield return StartCoroutine(jumpman.WalkTo(jumpman.originalX));

		while (transform.position.y < originalY) {
			float ratio = (originalY - transform.position.y)/1.3f;
			UpdateYPos(transform.position.y + Mathf.Max (0.001f, 0.2f * ratio));
			yield return new WaitForSeconds(0.01f);
		}


		int showScore = 0;
		int realScore = CalculateScore();
		ApplicationModel.score += realScore;
		for (int i = 0; i < 100; i ++) {
			showScore += (int) Mathf.Max (1, realScore / 100f);
			showScore = (int) Mathf.Min (showScore, realScore);
			scoreDisplay.text = "$" + showScore.ToString();
			yield return new WaitForSeconds(0.01f);
		}

		scoreDisplay.text = "$" + realScore.ToString();
	}

	private int CalculateScore() {
		scoreWorkOutDisplay.text = "$(" + cashMonies.cashCollected.ToString() + "+" + ((int) airTime).ToString() + ")*" + ((int) maxHeight).ToString();
		return (int) ((cashMonies.cashCollected + airTime) * maxHeight / 10);
	}

	private void UpdateYPos(float y) {
		Vector3 oldPos = transform.position;
		transform.position = new Vector3(oldPos.x, y, oldPos.z);
	}

	private void UpdateLevels() {
		airTimeLevel.text = "Air time: " + System.Math.Round(airTime, 1).ToString() + "s";
		heightLevel.text = "Height: " + ((int) (100*(jumpman.positionY - jumpman.ground))).ToString() + "m";
		speedLevel.text = "Speed: " + System.Math.Round(jumpman.velocity, 1).ToString() + "km/h";
		cashLevel.text = "$" + cashMonies.cashCollected.ToString() + " get!";
	}

	void Update() {
		if (jumpman.positionY > jumpman.ground) {
			airTime += Time.deltaTime;
			maxHeight = Mathf.Max(maxHeight, (jumpman.positionY - jumpman.ground)*100);
		}

		// did user cross 0 or 180?
		if (lastRotation > 0 && jumpman.rotation < 0 && jumpman.rotation > -90) { // crossed zero forwards 
			if (halfWayDirection > 0) { // halfway was forwards too
				FrontFlip();
			} else { // reset halfway
				halfWayDirection = 0;
			}
		} else if (jumpman.rotation > 0 && lastRotation < 0 && lastRotation > -90) { // crossed zero backwards
			if (halfWayDirection < 0) { // halfway was backwards too
				BackFlip();
			} else { // reset halfway
				halfWayDirection = 0;
			}
		} else if (lastRotation < -90 && lastRotation > -180 && jumpman.rotation < -180 && jumpman.rotation > -270) { // crossed halfway forwards
			halfWayDirection = 1;
		} else if (lastRotation < -180 && lastRotation > -270 && jumpman.rotation < -90 && jumpman.rotation > -180) { // crossed halfway backwards
			halfWayDirection = -1;
		}

		lastRotation = jumpman.rotation;

		UpdateLevels();
	}

	private int forwardOrBackward = 1;
	private void FrontFlip() {
		if (forwardOrBackward > 0) {
			cashMonies.cashCollected += 1;
			forwardOrBackward = -1;
		}
		print ("Front flip!");
	}

	private void BackFlip() {
		if (forwardOrBackward < 0) {
			cashMonies.cashCollected += 1;
			forwardOrBackward = 1;
		}
		print ("Back flip!");
	}
}