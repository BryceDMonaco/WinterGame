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

	public EdgeHandler[] intersectingEdges;

	public enum TileTypes {Nonstatic, Corner, Straight, TShape};
	private TileTypes spareTileTypeTemp = TileTypes.Nonstatic;

	public enum EdgeDirections {PosX, NegX, PosZ, NegZ, Unassigned};
	public EdgeDirections thisEdgeDirection = EdgeDirections.Unassigned;

	private Color startColor;
	private bool areTilesShifting = false;
	private Transform startTransform;
	public Transform[] nodes;

	void Start () 
	{
		startTransform = transform.GetChild(0);
		//nodes = new Transform[7];
		
	}
	
	void Update () 
	{
		
	}

	public void OnMouseEnter ()
	{
		MeshRenderer myRenderer = GetComponent <MeshRenderer> ();

		startColor = myRenderer.material.color;

		myRenderer.material.color = Color.red;

        UpdatePreviewTile();


    }

	public void OnMouseExit ()
	{
        if (mazeHandler.previewTileNode != null)
        {
            Destroy(mazeHandler.previewTileNode.transform.parent.gameObject);
        }

        mazeHandler.previewTileNode = null;

        MeshRenderer myRenderer = GetComponent <MeshRenderer> ();

		myRenderer.material.color = startColor;


	}

	public void OnMouseOver ()
	{
		if (!areTilesShifting && Input.GetMouseButtonDown (0))
		{	
			//areTilesShifting = true;

			ShiftTilesV2 ();
            UpdatePreviewTile ();

		}

	}

    public void UpdatePreviewTile ()
    {
        if (mazeHandler.previewTileNode != null)
        {
            Destroy(mazeHandler.previewTileNode.transform.parent.gameObject);
        }

        mazeHandler.previewTileNode = null;

        GameObject preview = null;

        switch ((TileTypes)mazeHandler.spareTileType)
        {
            case TileTypes.Corner:
                preview = Instantiate(mazeHandler.spareHolder.tileCorner, new Vector3(1000, 0, 1000), Quaternion.identity);
                break;
            case TileTypes.Straight:
                preview = Instantiate(mazeHandler.spareHolder.tileStraight, new Vector3(1000, 0, 1000), Quaternion.identity);
                break;
            case TileTypes.TShape:
                preview = Instantiate(mazeHandler.spareHolder.tileTShape, new Vector3(1000, 0, 1000), Quaternion.identity);
                break;
        }

        foreach (Transform part in preview.gameObject.GetComponentsInChildren<Transform>())
        {
            part.gameObject.layer = 2;
        }

        preview.name = "Preview Tile";

        switch (thisEdgeDirection)
        {
            case EdgeDirections.NegX:
                preview.transform.position = new Vector3(20, 0.5f, transform.position.z);
                break;
            case EdgeDirections.PosX:
                preview.transform.position = new Vector3(-20, 0.5f, transform.position.z);
                break;
            case EdgeDirections.NegZ:
                preview.transform.position = new Vector3(transform.position.x, 0.5f, 20);
                break;
            case EdgeDirections.PosZ:
                preview.transform.position = new Vector3(transform.position.x, 0.5f, -20);
                break;
        }

        preview.transform.rotation = mazeHandler.spareTileNode.transform.parent.rotation;

        mazeHandler.previewTileNode = preview.GetComponentInChildren<TileNode>();

    }

    public void GenerateNodesArray ()
	{
		nodes = new Transform[7];

		//Debug.Log("Generating!");		

		for (int i = 0; i < 7; i++)
		{
			float distance = 5 + (5 * i);
			
			RaycastHit[] hitNodes;
			hitNodes = Physics.RaycastAll (transform.position, transform.up, distance, LayerMask.GetMask ("MazeNode"));

			Transform newNode = null;

			//Debug.Log(transform.name + " number of tiles found = " + hitNodes.Length + "(i = " + i + ")");

			foreach (RaycastHit hit in hitNodes)
			{
				Transform foundNode = hit.transform.parent;

				//Debug.Log(transform.name + ": Checking " + foundNode.name);

				if (!ArrayContainsTransform(nodes, foundNode))
				{
					newNode = foundNode;

					break;

				}

			}

			//Debug.Log ("Assigned " + newNode.name + " to i = " + i);
			nodes[i] = newNode;

		}

	}

	//Deprecated, use ShiftTilesV2
	public void ShiftTiles () //Move tiles in the direction equivalent to the edge's local +Y axis
	{
		Debug.Log ("Start.");

		RaycastHit[] hitNodes;

		hitNodes = Physics.RaycastAll (transform.position, transform.up, 50f, LayerMask.GetMask ("MazeNode"));
		Debug.DrawLine(transform.position, transform.position + (transform.up * 50f), Color.red, 5f);

		Debug.Log (transform.name + " hit " + hitNodes.Length + "Nodes.");

		for (int i = 0; i < hitNodes.Length; i++)
		{
			hitNodes [i].transform.parent.name = i.ToString ();

		}

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
		//Vector3 startPostion = hitNodes [0].transform.parent.position;
		Vector3 startPostion = startTransform.position;


		mazeHandler.spareTileNode.transform.parent.position = startPostion; //Move the spare to the first spot

		for (int i = 0; i < hitNodes.Length; i++)
		{
			hitNodes [i].transform.parent.position = new Vector3 (hitNodes [i].transform.parent.position.x + (5 * xOffset), hitNodes [i].transform.parent.position.y, hitNodes [i].transform.parent.position.z + (5 * zOffset));

		}

		mazeHandler.spareTileType = (MazeHandler.TileTypes) hitNodes [hitNodes.Length - 1].transform.GetComponent <TileNode> ().myTileType; //Hopefully this cast works
		mazeHandler.spareTileNode = hitNodes [hitNodes.Length - 1].transform.GetComponent <TileNode> ();

		hitNodes [hitNodes.Length - 1].transform.parent.position = sparePosition; //Move the last tile to the spare position

		TileNode[] allTileNodes = FindObjectsOfType <TileNode> (); //This is really slow and there's definitely a faster way to do this

		foreach (TileNode node in allTileNodes)
		{
			node.ClearNeighbors ();
			node.CheckForNeighbors ();

		}

		Debug.Log ("Done.");

	}


	public void ShiftTilesV2 () //Move tiles in the direction equivalent to the edge's local +Y axis
	{
		//First we need to store the old spare tile in a temp var so spareTileNode can be overwritten
		Transform oldSpare = mazeHandler.spareTileNode.transform;

		//Set spareTileNode to the tile which will be pushed out
		mazeHandler.spareTileNode = nodes[6].GetComponentInChildren<TileNode>();

		//Move the tiles up the array (so nodes[6] becomes nodes[5], 5 becomes 4, all the way to 1 becomes 0)
		for (int i = 6; i > 0; i--)
		{
			nodes[i] = nodes [i - 1];	

		}

		//Place the old spare tile at 0
		nodes[0] = oldSpare.parent;

		//Set spare tile variables and move the spare tile to the correct position
		mazeHandler.spareTileType = (MazeHandler.TileTypes) mazeHandler.spareTileNode.transform.GetComponent <TileNode> ().myTileType; //Hopefully this cast works
		mazeHandler.spareTileNode.transform.parent.position = mazeHandler.spareHolder.transform.position;

		Vector3 startPosition = startTransform.position;

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

		//Go through all nodes that should now be in the row and move them to their correct spots.
		for (int i = 0; i < 7; i++)
		{
			nodes [i].position = new Vector3 (startPosition.x + (5 * xOffset * i), startPosition.y, startPosition.z + (5 * zOffset * i));

		}

		//Have all nodes regenerate their paths
		mazeHandler.UpdateAllNodeNeighbors ();

		//Have all the rows which intersect with this row update their node arrays since at least one node has changed
		UpdateIntersectingNodesArrays ();


	}

	public bool ArrayContainsTransform (Transform[] array, Transform target)
	{
		bool contained = false;

		foreach (Transform t in array)
		{
			if (t == target)
			{
				contained = true;

				break;

			}

		}

		return contained;

	}

	public void UpdateIntersectingNodesArrays ()
	{
		foreach (EdgeHandler arrow in intersectingEdges)
		{
			arrow.GenerateNodesArray ();

		}

	}

}