using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewTile : MonoBehaviour {
	public MazeHandler mazeHandler;

	public GameObject straightPreview;
	public GameObject cornerPreview;
	public GameObject tShapePreview;

	public bool isVisible = false;

	public enum TileTypes {Nonstatic, Corner, Straight, TShape};

	// Use this for initialization
	void Start () {
		HidePreview ();
	}
	
	// Update is called once per frame
	void Update () {
		if ((Input.GetKeyDown(KeyCode.R) || Input.GetMouseButtonDown(1)) && isVisible)
		{
			transform.Rotate(0f, 90f, 0f);

		}
		
	}

	public void MoveToPosition (Transform newPosition)
	{
		transform.position = newPosition.position;

		TileTypes spareType = (PreviewTile.TileTypes) mazeHandler.spareTileType;

		if (spareType == TileTypes.Straight)
		{
			straightPreview.SetActive(true);
			cornerPreview.SetActive(false);
			tShapePreview.SetActive(false);

		} else if (spareType == TileTypes.Corner)
		{
			straightPreview.SetActive(false);
			cornerPreview.SetActive(true);
			tShapePreview.SetActive(false);

		} else if (spareType == TileTypes.TShape)
		{
			straightPreview.SetActive(false);
			cornerPreview.SetActive(false);
			tShapePreview.SetActive(true);

		} else
		{
			Debug.Log("Preview Tile Error: Unrecognized tile type or nonstatic.");

		}

		isVisible = true;


	}

	public void HidePreview ()
	{
		straightPreview.SetActive(false);
		cornerPreview.SetActive(false);
		tShapePreview.SetActive(false);

		isVisible = false;

	}
}
