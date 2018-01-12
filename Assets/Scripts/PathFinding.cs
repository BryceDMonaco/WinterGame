/*
 *	Author: Bryce Monaco
 *
 *	Description: This script adapted from https://gist.github.com/hermesespinola/15cf66af8edf059df9f38c6c879db0cb
 *				 DoesAnyPathExist is a Depth First Search function adapted for this game
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
	public bool DoesAnyPathExist (TileNode start, TileNode end) 
	{ 

		Stack<TileNode> work = new Stack<TileNode> ();
		List<TileNode> visited = new List<TileNode> ();

		work.Push (start);
		visited.Add (start);
		//start.history = new List<TileNode> ();

		while(work.Count > 0)
		{
			TileNode current = work.Pop ();
			if (current == end) 
			{
				//List<TileNode> result = current.history;
				//result.Add (current);
				return true;

			} else 
			{
				for(int i = 0; i < current.neighbors.Length; i++)
				{
					TileNode currentChild = current.neighbors [i];
					if (currentChild != null && !visited.Contains(currentChild))
					{

						work.Push (currentChild);
						visited.Add (currentChild);
						//currentChild.history = new List<TileNode> (current.history);
						//currentChild.history.Add (current);
					}
				}	
			}
		}

		return false; 
	
	}
}