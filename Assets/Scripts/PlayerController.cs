/*
 *	Author: Bryce Monaco
 *
 *	Description: This script handles a player choosing their next destination
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerController : MonoBehaviour 
{
	private PathFinding pathFinder;

	public TileNode startNode;
	public TileNode currentNode;

    public float minMoveTime = 1f;  // A set of tweens from start position to end position will take at least this long (s)
    public float maxMoveTime = 3f;  // A set of tweens from start position to end position will take no longer than this amount of time (s)

	void Start () 
	{
		pathFinder = GetComponent <PathFinding> ();

        //currentNode = startNode;

        //transform.position = currentNode.transform.position;

        transform.parent = currentNode.transform;
		
	}
	
	void Update () 
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;

			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 1000f, LayerMask.GetMask ("MazeFloor")))
			{
				TileNode hitNode = hit.collider.transform.parent.GetComponentInChildren <TileNode> ();

                List<TileNode> path;

				if ((path = pathFinder.DoesAnyPathExist (currentNode, hitNode)) != null)
				{
                    transform.parent = null;

                    MoveAlongPath(path);

                    Debug.Log("Path is " + path.Count + " tiles long.");
                    //transform.position = hitNode.transform.position;
					currentNode = hitNode;
                    transform.parent = currentNode.transform;

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

    void MoveAlongPath (List<TileNode> path)
    {
        float moveTime = minMoveTime;

        if (path.Count > (int) maxMoveTime)
        {
            moveTime = maxMoveTime / (float) path.Count;
        }

        Vector3[] positions = new Vector3[path.Count];

        for (int i = 0; i < path.Count; i++)
        {
            positions[i] = path[i].transform.GetComponentInChildren<TileNode>().transform.position;
        }

        transform.DOPath(positions, moveTime * path.Count);

    }


}