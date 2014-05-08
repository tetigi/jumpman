using UnityEngine;
using System.Collections;

public class ScoreScript : MonoBehaviour
{
	static private TextMesh textMesh;
	static private int score;
	
	static public int Score
	{
		get
		{
			return score;	
		}
		
		set
		{
			score = value;	
		}
	}
	
	void Start ()
	{
		score = 0;
		textMesh = GetComponent<TextMesh>();
	}
	
	void Update ()
	{
		textMesh.text = string.Format("{0}", score);
	}
}