/*
 *	Author: Bryce Monaco
 *
 *	Description:
 *	
 *	TODO: Occasionly the spare and the first tile will stay on eachother and create a gap
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeHandler : MonoBehaviour 
{
	public MazeHandler mazeHandler;

	public enum TileTypes {Nonstatic, Corner, Straight, TShape};
	private TileTypes spareTileTypeTemp = TileTypes.Nonstatic;

	public enum EdgeDirections {PosX, NegX, PosZ, NegZ, Unassigned};
	public EdgeDirections thisEdgeDirection = EdgeDirections.Unassigned;

	private Color startColor;

	void Start () 
	{
		
	}
	
	void Update () 
	{
		
	}

	public void OnMouseEnter ()
	{
		MeshRenderer myRenderer = GetComponent <MeshRenderer> ();

		startColor = myRenderer.material.color;

		myRenderer.material.color = Color.red;


	}

	public void OnMouseExit ()
	{
		MeshRenderer myRenderer = GetComponent <MeshRenderer> ();

		myRenderer.material.color = startColor;


	}

	public void OnMouseOver ()
	{
		if (Input.GetMouseButtonDown (0))
		{
			ShiftTiles ();

		}

	}

	public void ShiftTiles () //Move tiles in the direction equivalent to the edge's local +Y axis
	{
		Debug.Log ("Start.");

		RaycastHit[] hitNodes;

		hitNodes = Physics.RaycastAll (transform.position, transform.up, 50f, LayerMask.GetMask ("MazeNode"));

		Debug.Log (transform.name + " hit " + hitNodes.Length + "Nodes.");

		//Debug.DrawLine (transform.position, hitNodes [hitNodes.Length - 1].transform.position, Color.red, 5f);

		//Used to determine how the tiles should be offset, only one value should be non-zero and will be +/- 1.
		int xOffset = 0;
		int zOffset = 0;

		//This was taken out of the loop to speed it up a little bit
		if (thisEdgeDirection == EdgeDirections.PosX)
		{
			xOffset = 1;

		} else if (thisEdgeDirection == EdgeDirections.NegX)
		{
			xOffset = -1;

		} else if (thisEdgeDirection == EdgeDirections.PosZ)
		{
			zOffset = 1;

		} else if (thisEdgeDirection == EdgeDirections.NegZ)
		{
			zOffset = -1;

		}

		spareTileTypeTemp = TileTypes.Nonstatic; //Equivalent to empty var
		Vector3 sparePosition = mazeHandler.spareHolder.transform.position;
		Vector3 startPostion = hitNodes [0].transform.parent.position;

		for (int i = 0; i < hitNodes.Length; i++)
		{
			if (i == 0)
			{
				mazeHandler.spareTileNode.transform.parent.position = startPostion; //Move the spare to the first spot

			}

			hitNodes [i].transform.parent.position = new Vector3 (hitNodes [i].transform.parent.position.x + (5 * xOffset), hitNodes [i].transform.parent.position.y, hitNodes [i].transform.parent.position.z + (5 * zOffset));

			if (i == hitNodes.Length - 1) //On the tile farthest from the edge
			{
				mazeHandler.spareTileType = (MazeHandler.TileTypes) hitNodes [i].transform.GetComponent <TileNode> ().myTileType; //Hopefully this cast works
				mazeHandler.spareTileNode = hitNodes [i].transform.GetComponent <TileNode> ();

				hitNodes [i].transform.parent.position = sparePosition; //Move the last tile to the spare position

			}

		}

		TileNode[] allTileNodes = FindObjectsOfType <TileNode> (); //This is really slow and there's definitely a faster way to do this

		foreach (TileNode node in allTileNodes)
		{
			node.ClearNeighbors ();
			node.CheckForNeighbors ();

		}

		Debug.Log ("Done.");
		

	}
}