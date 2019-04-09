using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpotter : MonoBehaviour
{
	[SerializeField] GameObject enemyIndicator;
	[SerializeField] float defaultTime = 5;
	float pingTimer;
	// Use this for initialization
	void Start()
	{
		pingTimer = defaultTime;
	}

	// Update is called once per frame
	void Update()
	{

		Clear();
		Radar();
	}

	void Radar()
	{
		//find all players
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		List<Vector3> directions = new List<Vector3>();

		//iterate over players
		foreach (GameObject p in players)
		{
			//ignore local player
			if (p == GameController.LocalPlayer)
			{
				continue;
			}
			else
			{

				//add normalised direction to list
				Vector3 dir = GameController.LocalPlayer.transform.position - p.transform.position;
				dir = dir.normalized;
				directions.Add(dir);

			}
		}

		foreach (Vector3 dir in directions)
		{
			Instantiate(enemyIndicator, gameObject.transform.position + (dir * (gameObject.transform.localScale.x / 2)), gameObject.transform.rotation);
		}
	}

	void Clear()
	{
		GameObject[] playerIndicators = GameObject.FindGameObjectsWithTag("GUI dot");
		foreach (GameObject p in playerIndicators)
		{

			print(p.name);
			Destroy(p);
		}
	}
}