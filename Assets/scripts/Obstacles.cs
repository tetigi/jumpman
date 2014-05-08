using UnityEngine;
using System.Collections;


public class Obstacles : MonoBehaviour {
	public Jumpman jumpman;
	public tk2dSprite things;

	void Start () {
		//SpawnObstacle("duck");
	}

	public void SpawnObstacle(string type) {
		tk2dSprite obstacle = (tk2dSprite) Instantiate(things);
		obstacle.SetSprite(type);

		float dir;
		if (Random.value < 0.5) {
			obstacle.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width + 20, 0, 0)).x, 1.55f, -0.5f);
			dir = 1;
		} else {
			obstacle.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(-20, 0, 0)).x, 1.55f, -0.5f);
			obstacle.transform.eulerAngles = new Vector3(0, 180, 0);
			dir = -1;
		}

		StartCoroutine(MoveObstacle(obstacle, dir));
	}

	IEnumerator MoveObstacle(tk2dSprite obs, float dir) {
		Vector3 pos = obs.transform.position;
		float x = pos.x;
		float y = pos.y;
		float speed = 0.01f * dir;
		bool hit = false;
		while (x >= Camera.main.ScreenToWorldPoint(new Vector3(-20, 0, 0)).x && x <= Camera.main.ScreenToWorldPoint(new Vector3(Screen.width + 20, 0, 0)).x) {
			x -= speed;
			y -= Mathf.Min(0.1f, jumpman.velocity * 0.01f);
			obs.transform.position = new Vector3(x, y, pos.z);
			if(!hit && obs.renderer.bounds.Intersects(jumpman.renderer.bounds)) {
				obs.SetSprite(obs.GetCurrentSpriteDef().name + "_hit");
				speed = 0.005f * dir;
				jumpman.velocity = jumpman.velocity * 0.5f;
				hit = true;
				jumpman.collided = true;
				TouchSense.instance.playBuiltinEffect(1);
			}
			yield return new WaitForSeconds(0.01f);
		}
		Destroy(obs.gameObject);
	}
}