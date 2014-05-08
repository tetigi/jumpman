using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Platforms : MonoBehaviour {
	public Jumpman jumpman;
	public CashMonies money;

	List<Tuple<GameObject, float>> plats = new List<Tuple<GameObject, float>>();

	public void SpawnNPlatsBetween(int n, float start, float finish) {
		float level = start;
		float range = (finish - start) / n;
		for (int i = 0; i < n; i++) {
			SpawnPlatAt(level + (Random.value * range));
			level += range;
		}
	}

	public void SpawnPlatAt(float y) {
		GameObject newObject = (GameObject) Instantiate(gameObject);

		float x = 0.05f + Random.value * 0.9f;

		newObject.transform.position = new Vector3(x, y, -0.5f);
		plats.Add(new Tuple<GameObject, float>(newObject, y));
	}

	void Update () {
		// move all the coins
		for(int i = 0; i < plats.Count; i ++) {
			Tuple<GameObject,float> plat = plats[i];
			MoveY (plat.fst, plat.snd - (jumpman.positionY - jumpman.ground));
			if (jumpman.velocity <= 0 && plat.fst.renderer.bounds.Intersects(jumpman.renderer.bounds)) {
				jumpman.velocity = (-jumpman.velocity*0.5f) + 2f;
				money.cashCollected += 1;
				Destroy (plat.fst);
				plats[i] = null;
			}
		}

		// remove nulls
		for (int i = plats.Count -1; i >= 0; i --) {
			if (plats[i] == null) {
				plats.RemoveAt(i);
			}
		}
	}

	private void MoveY(GameObject s, float y) {
		Vector3 oldPos = s.transform.position;
		s.transform.position = new Vector3(oldPos.x, y, oldPos.z);
	}

	private class Tuple<T, U> {
		public T fst;
		public U snd;

		public Tuple(T t, U u) {
			fst = t;
			snd = u;
		}
	}
}