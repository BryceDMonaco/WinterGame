/*
 *	Author: Bryce Monaco
 *
 *	Description:
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileNode : MonoBehaviour 
{
	public TileNode[] neighbors = new TileNode [3]; //Each tile will have 0 to 3 neighbors depending on the type and orientation 
	
	public enum TileTypes {Nonstatic, Corner, Straight, TShape};
	public TileTypes myTileType = TileTypes.Nonstatic; 

	void Start () 
	{
		string parentName = transform.parent.name;

		if (parentName.Contains("Corner"))
		{
			myTileType = TileTypes.Corner;

		} else if (parentName.Contains("Straight"))
		{
			myTileType = TileTypes.Straight;

		} else if (parentName.Contains("Tile_T"))
		{
			myTileType = TileTypes.TShape;

		}

		CheckForNeighbors ();

	}
	
	void Update () 
	{
		for (int i = 0; i < 3; i++)
		{
			if (neighbors [i] != null)
			{
				Debug.DrawLine (transform.position, neighbors [i].transform.position, Color.green);

			} 

		}
	}

	public void CheckForNeighbors ()
	{
		if (myTileType == TileTypes.Corner) //Check on local +X and +Z, Neighbor Array is {+X neighbor, +Z neighbor, NULL}
		{
			RaycastHit hit;

			if (Physics.Raycast (transform.position, transform.right, out hit, 5f, LayerMask.GetMask ("MazeNode", "MazeWall"))) //+X
			{
				if (hit.transform.tag == "Node")
				{
					neighbors [0] = hit.transform.GetComponent <TileNode> ();

				}
			}

			if (Physics.Raycast (transform.position, transform.forward, out hit, 5f, LayerMask.GetMask ("MazeNode", "MazeWall"))) //+Z
			{
				if (hit.transform.tag == "Node")
				{
					neighbors [1] = hit.transform.GetComponent <TileNode> ();

				}
			}

		} else if (myTileType == TileTypes.Straight) //Check on local +Z and -Z, Neighbor Array is {+Z neighbor, -Z neighbor, NULL}
		{
			RaycastHit hit;

			if (Physics.Raycast (transform.position, transform.forward, out hit, 5f, LayerMask.GetMask ("MazeNode", "MazeWall"))) //+Z
			{
				if (hit.transform.tag == "Node")
				{
					neighbors [0] = hit.transform.GetComponent <TileNode> ();

				}
			}

			if (Physics.Raycast (transform.position, -transform.forward, out hit, 5f, LayerMask.GetMask ("MazeNode", "MazeWall"))) //-Z
			{
				if (hit.transform.tag == "Node")
				{
					neighbors [1] = hit.transform.GetComponent <TileNode> ();

				}
			}

		}else if (myTileType == TileTypes.TShape) //Check on local +X, +Z, and -Z, Neighbor Array is {+X neighbor, +Z neighbor, -Z neighbor}
		{
			RaycastHit hit;

			if (Physics.Raycast (transform.position, transform.right, out hit, 5f, LayerMask.GetMask ("MazeNode", "MazeWall"))) //+X
			{
				if (hit.transform.tag == "Node")
				{
					neighbors [0] = hit.transform.GetComponent <TileNode> ();

				}
			}

			if (Physics.Raycast (transform.position, transform.forward, out hit, 5f, LayerMask.GetMask ("MazeNode", "MazeWall"))) //+Z
			{
				if (hit.transform.tag == "Node")
				{
					neighbors [1] = hit.transform.GetComponent <TileNode> ();

				}
			}

			if (Physics.Raycast (transform.position, -transform.forward, out hit, 5f, LayerMask.GetMask ("MazeNode", "MazeWall"))) //-Z
			{
				if (hit.transform.tag == "Node")
				{
					neighbors [2] = hit.transform.GetComponent <TileNode> ();

				}
			}

		}

	}

	public void ClearNeighbors ()
	{
		neighbors = new TileNode [3];

	}

	public void AddNeighbor (TileNode sentNeighbor)
	{


	}
}