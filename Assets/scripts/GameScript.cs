using UnityEngine;
using System.Collections;

public class GameScript : MonoBehaviour {

	public Jumpman jumpman;
	public GameObject reallyFar;
	public GameObject far;
	public GameObject near;
	public ScoreScreen scoreScreen;
	public Obstacles obstacles;
	public Platforms platforms;
	public tk2dSprite stars;
	public Color sky;
	public Color ionosphere;
	public Color space;
	public ItemsBar itemBar;
	public AudioSource hardcore;
	public AudioSource serene;

	private float nearStillPos;
	private float farStillPos;
	private float reallyFarStillPos;

	private bool playing = true;
	private bool inJump = false;

	private const float OBSTACLE_CHANCE = 0.1f;
	private const float OBSTACLE_WAIT = 0.5f;
	private float obstacleCounter = 0;

	private float endGameHeight = 30;

	void Start () {
		TouchSense ts = TouchSense.instance;
		nearStillPos = near.transform.position.y;
		farStillPos = far.transform.position.y;
		reallyFarStillPos = reallyFar.transform.position.y;

		//ApplicationModel.AddItem(Items.DOUBLE_JUMP_MAN);
		scoreScreen.cashMonies.SpawnNCoinsBetween(30, 0.6f, 25f);
		scoreScreen.cashMonies.SpawnCoinAt(0.7f);

		platforms.SpawnNPlatsBetween(15, 0.6f, 15f);

		SetAlpha(stars, 0);
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}

	private void SetAlpha(tk2dSprite sprite, float a) {
		Color c = sprite.color;
		sprite.color = new Color(c.r, c.b, c.g, a);
	}

	private IEnumerator StartEndScene() {
		yield return new WaitForSeconds(2f);
		Camera.main.SendMessage("FadeOut", 0.2f);
		yield return new WaitForSeconds(2f);
		Application.LoadLevel("jumpman_end");
	}

	private bool buttonDown = false;

	void Update () {
		obstacleCounter += Time.deltaTime;

		// Check for end state
		if (jumpman.state != Jumpman.State.CROUCH && inJump && jumpman.positionY <= jumpman.ground) {
			playing = false;
			inJump = false;
			hardcore.Stop();
			serene.Play();
			StartCoroutine(scoreScreen.SlideIn());
			TouchSense.instance.playBuiltinEffect(2);
		}

		// Check for end game
		if (jumpman.gravity != 0 && jumpman.positionY > endGameHeight) {
			jumpman.velocity = 1f;
			jumpman.doubleJumpVelocity = 0f;
			jumpman.gravity = 0f;
			StartCoroutine(StartEndScene());
		}

		// Jumping
		if (playing && Input.GetButtonDown("Fire1")) {
			if (!itemBar.CheckPress()) {
				if (jumpman.state == Jumpman.State.JUMP) {
					StartCoroutine(jumpman.Jump());
				} else if (jumpman.state == Jumpman.State.MAN) {
					StartCoroutine (jumpman.CrouchJump());
					serene.Stop();
					hardcore.Play();
					inJump = true;
				}
			}
		}

		// Move environment based on jump man's position
		MoveGameObject(near, nearStillPos - (jumpman.positionY - jumpman.ground));
		MoveGameObject(far, farStillPos - 0.6f*(jumpman.positionY - jumpman.ground));
		MoveGameObject(reallyFar, reallyFarStillPos - 0.1f*(jumpman.positionY - jumpman.ground));

		if (jumpman.positionY < endGameHeight / 2) {
			float ratio = (jumpman.positionY - jumpman.ground) / (endGameHeight / 2f);
			Camera.main.backgroundColor = Color.Lerp (sky, ionosphere, ratio);
			if (jumpman.positionY > (endGameHeight / 4) && obstacleCounter >= OBSTACLE_WAIT && Random.value < OBSTACLE_CHANCE) {
				obstacles.SpawnObstacle("duck");
				obstacleCounter = 0;
			}
		} else {
			float ratio = (jumpman.positionY - jumpman.ground - (endGameHeight / 2f)) / (endGameHeight / 2f);
			Camera.main.backgroundColor = Color.Lerp (ionosphere, space, ratio);
			SetAlpha(stars, ratio);
			if (obstacleCounter >= OBSTACLE_WAIT && Random.value < OBSTACLE_CHANCE) {
				obstacles.SpawnObstacle("satellite");
				obstacleCounter = 0;
			}
		}

	}

	void MoveGameObject(GameObject o, float y) {
		o.transform.position = 
			new Vector3(o.transform.position.x, y, o.transform.position.z);
	}
}
