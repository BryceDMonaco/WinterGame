/*
 *	Author: Bryce Monaco
 *
 *	Description:
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeHandler : MonoBehaviour 
{
	public TileHolder[] tileHolders;
	public TileHolder spareHolder;
	

	void Start () 
	{
		int tileCountCorner = 16;
		int tileCountStraight = 12; //12, 1 is out of the board
		int tileCountTShape = 6;

		for (int i = 0; i < 49; i++)
		{
			if (!tileHolders[i].isStaticTile)
			{
				bool tileAvailable = false;
				int choice = 0;

				while (!tileAvailable)
				{
					choice = Random.Range (0, 3);
					GameObject spawnedTile = null;

					if (choice == 0 && tileCountCorner > 0)
					{
						tileAvailable = true;
						tileCountCorner--;

						spawnedTile = Instantiate (tileHolders[i].tileCorner, tileHolders[i].transform.position, tileHolders[i].transform.rotation);

					} else if (choice == 1 && tileCountStraight > 0)
					{
						tileAvailable = true;
						tileCountStraight--;

						spawnedTile = Instantiate (tileHolders[i].tileStraight, tileHolders[i].transform.position, tileHolders[i].transform.rotation);

					} else if (choice == 2 && tileCountTShape > 0)
					{
						tileAvailable = true;
						tileCountTShape--;

						spawnedTile = Instantiate (tileHolders[i].tileTShape, tileHolders[i].transform.position, tileHolders[i].transform.rotation);

					} else
					{
						Debug.Log ("Error: Tile Type " + choice + " not available. Skipping.");

						choice = Random.Range (0, 3);

						//tileAvailable = true;

					}

					if (spawnedTile != null)
					{
						spawnedTile.transform.rotation = Quaternion.Euler (0f, 90 * (int) Random.Range(0, 5), 0f);

					}
				}
			} 
		}

		if (tileCountCorner == 1)
		{
			Instantiate (spareHolder.tileCorner, spareHolder.transform.position, spareHolder.transform.rotation);

		} else if (tileCountStraight == 1)
		{
			Instantiate (spareHolder.tileStraight, spareHolder.transform.position, spareHolder.transform.rotation);

		} else if (tileCountTShape == 1)
		{
			Instantiate (spareHolder.tileTShape, spareHolder.transform.position, spareHolder.transform.rotation);

		}
	}
	
	void Update () 
	{
	}
}