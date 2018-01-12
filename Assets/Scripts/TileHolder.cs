/*
 *	Author: Bryce Monaco
 *
 *	Description:
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHolder : MonoBehaviour 
{
	public bool isStaticTile = false; //Does this tile ever change?
	
	public enum TileTypes {Nonstatic, Corner, Straight, TShape};
	public TileTypes staticTileType = TileTypes.Nonstatic;

	public GameObject tileCorner;
	public GameObject tileStraight;
	public GameObject tileTShape;

	public bool spawnPlayer = false;
	public GameObject playerObject;

	void Start () 
	{
		GetComponent<MeshRenderer> ().enabled = false;

		if (isStaticTile)
		{
			if (staticTileType == TileTypes.Corner)
			{
				GameObject tile = Instantiate (tileCorner, transform.position, transform.rotation);

				if (spawnPlayer)
				{
					GameObject newPlayer = Instantiate (playerObject, tile.GetComponentInChildren <TileNode> ().transform.position, tile.GetComponentInChildren <TileNode> ().transform.rotation);

					newPlayer.GetComponent <PlayerController> ().currentNode = tile.GetComponentInChildren <TileNode> ();

				}


			} else if (staticTileType == TileTypes.Straight)
			{
				Instantiate (tileStraight, transform.position, transform.rotation);

			} else if (staticTileType == TileTypes.TShape)
			{
				Instantiate (tileTShape, transform.position, transform.rotation);

			}

		}
		
	}
	
	void Update () 
	{
		
	}
}