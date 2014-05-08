using UnityEngine;
using System.Collections;

public class Jumpman : MonoBehaviour {

	public enum State { JETPACK, DOUBLE_JUMP, JUMP, MAN, CROUCH, SAIYAN };
	public State state = State.MAN;
	public ChargeBar chargeBar;

	// Physics
	public float gravity = -9.8f;
	//float maxFallSpeed = -2f;
	public float velocity = 0f;
	public float ground;
	public float originalX;
	public float positionY;
	public float positionX;
	public float rotation = 0;
	float timeStep = 0.01f;
	public float jumpVelocity = 10.0f;
	public float doubleJumpVelocity = 10.0f;

	// Game derived
	public tk2dSprite sprite;
	public GameObject trampoline;
	public GameObject hat;
	public BasicAnimation saiyanEffects;
	public AudioSource saiyanSound;

	public bool collided = false;

	void Awake () {
		ground = transform.position.y;
		originalX = transform.position.x;
		positionY = ground;
		positionX = transform.position.x;

		chargeBar.Hide();
		trampoline.renderer.enabled = false;
		hat.renderer.enabled = ApplicationModel.HasBoughtItem(Items.HAT);
		saiyanEffects.renderer.enabled = false;
	}

	public IEnumerator CrouchJump() {
		if (ApplicationModel.HasBoughtItem(Items.CROUCH_MAN)) {
			int power = 0;
			int speed = 4;
			int dir = speed;
			state = State.CROUCH;
			sprite.SetSprite("crouch");
			chargeBar.Show();
			while (Input.GetButton("Fire1")) {
				power += dir;
				chargeBar.SetPower(power);
				if (power > 100) {
					dir = -speed;
				} else if (power < 0) {
					dir = speed;
				}
				yield return new WaitForSeconds(0.01f);
			}
			chargeBar.Hide();
			jumpVelocity += (power * ApplicationModel.CountItem(Items.CROUCH_MAN)) / 30;
		}
		yield return StartCoroutine(Jump());
	}

	public IEnumerator Jetpack(AudioSource jetpackSound) {
		jetpackSound.Play();
		State oldState = state;
		state = State.JETPACK;
		rotation = 0;
		collided = false;
		for (int i = 0; !collided && i < 100 * ApplicationModel.CountItem(Items.JETPACK); i ++) {
			velocity = 2f;
			sprite.SetSprite("jetpack");
			yield return new WaitForSeconds(0.01f);
		}
		collided = false;
		sprite.SetSprite("jump");
		state = oldState;
		jetpackSound.Stop();
	}

	private float GetJumpPotion() {
		return ApplicationModel.CountItem(Items.JUMP_POTION) * 2f;
	}

	private float GetDoubleJump() {
		return ApplicationModel.CountItem(Items.DOUBLE_JUMP_MAN) * 2f;
	}

	private IEnumerator SuperSaiyanGO() {
		saiyanSound.Play ();
		saiyanEffects.renderer.enabled = true;
		saiyanEffects.Animate();


		while (true) {
			state = State.SAIYAN;
			sprite.SetSprite("saiyan");
			velocity = 2f;
			rotation = 0f;
			yield return new WaitForSeconds(0.01f);
		}
	}

	private readonly object syncLock = new object();
	public IEnumerator Jump () {
		// Check if already jumping
		lock(syncLock) {
			if (state == State.DOUBLE_JUMP) return true;

			if (state == State.JUMP) {
				if (ApplicationModel.CountItem(Items.SAIYAN_POTION) == 5) {
					sprite.SetSprite("saiyan");
					yield return StartCoroutine(SuperSaiyanGO());
				} else if (ApplicationModel.HasBoughtItem(Items.DOUBLE_JUMP_MAN)) {
					velocity = doubleJumpVelocity + GetDoubleJump();
					state = State.DOUBLE_JUMP;
				}
				return true;
			}

			state = State.JUMP;
		}

		// Change to jump sprite
		sprite.SetSprite("jump");

		// Start jumping
		velocity = jumpVelocity + GetJumpPotion();
		positionY += velocity * timeStep;
		while (positionY > ground) {
			UpdatePosition();
			yield return new WaitForSeconds(timeStep);
			velocity += timeStep * gravity;
			//velocity = Mathf.Max (maxFallSpeed, velocity);
			positionY += velocity * timeStep;

			if (Input.GetKeyDown(KeyCode.LeftArrow)) {
				positionX -= 0.1f;
			} else if (Input.GetKeyDown(KeyCode.RightArrow)) {
				positionX += 0.1f;
			} else if  (ApplicationModel.HasBoughtItem(Items.HAT)) {
				positionX += Input.acceleration.x * 0.05f;
			} else {
				positionX += Input.acceleration.x * 0.02f;
			}
			if (Camera.main.WorldToScreenPoint(transform.position).x > Screen.width + 20) {
				positionX = Camera.main.ScreenToWorldPoint(new Vector3(-20, 0, 0)).x;
			} else if (positionX < Camera.main.ScreenToWorldPoint(new Vector3(-20, 0, 0)).x) {
				positionX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width + 20, 0, 0)).x;
			}

			if ((state == State.JUMP || state == State.DOUBLE_JUMP) && ApplicationModel.HasBoughtItem(Items.STUNT_MAN)) {
				rotation = CalculateRotation(Input.mousePosition, Camera.main.WorldToScreenPoint(transform.position));
			}

			if (positionY <= ground) {
				positionY = ground;
				if (trampoline.renderer.enabled == true && positionX > 0.35f && positionX < 0.55f) {
					velocity = -velocity;
					positionY = ground + 0.01f;
					trampoline.renderer.enabled = false;
				}
			}
		}

		positionY = ground;
		rotation = 0f;
		velocity = 0f;
		UpdatePosition();

		// Change to stand sprite
		sprite.SetSprite("man");

		// Set state back to normal
		lock(syncLock) {
			state = State.MAN;
		}
	}

	public void UpdatePosition() {
		Vector3 newPos = new Vector3(positionX, transform.position.y, transform.position.z);
		transform.position = newPos;
		//if (Camera.main.WorldToScreenPoint(newPos).y < Screen.height - 100) {
		//	transform.position = newPos;
		//}
		transform.eulerAngles = new Vector3(0, 0, rotation);
	}

	public IEnumerator WalkTo(float x) {
		sprite.SetSprite("jump");
		if (x < positionX) {
			transform.eulerAngles = new Vector3(0, 180, 0);
		}
		for (int i = 0; i < 50; i ++) {
			positionX += (x - positionX)/2;
			if (Mathf.Abs(x - positionX) < 0.01) {
				sprite.SetSprite("man");
			}
			UpdatePosition();
			yield return new WaitForSeconds(0.01f);
		}

		sprite.SetSprite("man");
	}

	float CalculateRotation(Vector3 mousePos, Vector3 myPos) {
		float deltaX = mousePos.x - myPos.x;
		float deltaY = mousePos.y - myPos.y;

		float rot = (Mathf.Atan2 (deltaY, deltaX) * Mathf.Rad2Deg) - 90;
		return rot;
	}

}
