using UnityEngine;
using System.Collections;

public class BasicAnimation : MonoBehaviour {
	private tk2dSpriteAnimator anim;

	void Start () {
		anim = GetComponent<tk2dSpriteAnimator>();
	}

	public void Animate () {
		anim.Play ("basic");
	}
}
