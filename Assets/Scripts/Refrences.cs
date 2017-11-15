using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refrences : MonoBehaviour
{

	public static Refrences instance;

	public GameView GameView;
	
	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

}
