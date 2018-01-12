/*
 *	Author: Bryce Monaco
 *
 *	Description:
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
	private PathFinding pathFinder;

	public TileNode startNode;
	public TileNode currentNode;

	void Start () 
	{
		pathFinder = GetComponent <PathFinding> ();

		//currentNode = startNode;

		//transform.position = currentNode.transform.position;
		
	}
	
	void Update () 
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;

			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 1000f, LayerMask.GetMask ("MazeFloor")))
			{
				TileNode hitNode = hit.collider.transform.parent.GetComponentInChildren <TileNode> ();

				if (pathFinder.DoesAnyPathExist (currentNode, hitNode))
				{
					transform.position = hitNode.transform.position;

					currentNode = hitNode;

				} else 
				{
					Debug.Log ("No path exists!");

				}

				

			} else
			{

				//transform.position = defaultPosition;
				//lastHit = null;
			}

		}
		
	}
}