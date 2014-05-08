using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CashMonies : MonoBehaviour {
	public tk2dSprite moneySilver;
	public tk2dSprite moneyGold;
	public tk2dSprite moneyBronze;
	public Jumpman jumpman;

	public int cashCollected = 0;

	private List<Tuple<tk2dSprite, float>> coins = new List<Tuple<tk2dSprite, float>>();

	void Start () {
	}

	public void SpawnNCoinsBetween(int n, float start, float finish) {
		float level = start;
		float range = (finish - start) / n;
		for (int i = 0; i < n; i++) {
			SpawnCoinAt(level + (Random.value * range));
			level += range;
		}
	}

	public void SpawnCoinAt(float y) {
		tk2dSprite coin;
		if (Random.value < 0.1) {
			coin = (tk2dSprite) Instantiate(moneyGold);
		} else if (Random.value < 0.3) {
			coin = (tk2dSprite) Instantiate(moneySilver);
		} else {
			coin = (tk2dSprite) Instantiate(moneyBronze);
		}

		float x = 0.05f + Random.value * 0.9f;

		coin.transform.position = new Vector3(x, y, -0.5f);
		coins.Add(new Tuple<tk2dSprite, float>(coin, y));
	}

	void Update () {
		// move all the coins
		for(int i = 0; i < coins.Count; i ++) {
			Tuple<tk2dSprite,float> coin = coins[i];
			MoveY (coin.fst, coin.snd - (jumpman.positionY - jumpman.ground));
			if (coin.fst.renderer.bounds.Intersects(jumpman.renderer.bounds)) {
				if (coin.fst.name == "moneyBronze(Clone)") {
					cashCollected += 1;
				} else if (coin.fst.name == "moneySilver(Clone)") {
					cashCollected += 2;
				} else {
					cashCollected += 5;
				}
				Destroy (coin.fst.gameObject);
				coins[i] = null;
			}
		}

		// remove nulls
		for (int i = coins.Count -1; i >= 0; i --) {
			if (coins[i] == null) {
				coins.RemoveAt(i);
			}
		}
	}

	private void MoveY(tk2dSprite s, float y) {
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