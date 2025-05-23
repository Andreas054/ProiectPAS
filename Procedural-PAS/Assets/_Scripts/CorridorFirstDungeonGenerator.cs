using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using UnityEngine.WSA;

public class CorridorFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
	[SerializeField]
	private int corridorLength = 15, corridorCount = 5;
	[SerializeField]
	[Range(0.1f, 1)]
	private float roomPercent = 0.7f;
	[SerializeField] GameObject player;
    [SerializeField]
	private ItemPlacementHelper itemPlacementHelper;
    private List<TileBase> possibleDoorList;
	[SerializeField]
	public GameObject door;
	[SerializeField]
	public GameObject canvas;

    // Data
    private Dictionary<Vector2Int, HashSet<Vector2Int>> roomsDictionary
		= new Dictionary<Vector2Int, HashSet<Vector2Int>>();

	private HashSet<Vector2Int> floorPositions, corridorPositions, firstRoomFloor;


	//Gizmos Data
	private List<Color> roomColors = new List<Color>();
	[SerializeField]
	private bool showRoomGizmo = false, showCorridorsGizmo;

	protected override void RunProceduralGeneration()
	{
		CorridorFirstGeneration();
	}

    private void CorridorFirstGeneration()
	{
        canvas.SetActive(false);

        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
		HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();

		List<List<Vector2Int>> corridors = CreateCorridors(floorPositions, potentialRoomPositions);

		HashSet<Vector2Int> roomPositions = CreateRooms(potentialRoomPositions);
		player.transform.position = new Vector3(roomPositions.First().x, roomPositions.First().y, 0);


        HashSet<Vector2Int> allRoomPositions = new HashSet<Vector2Int>(roomPositions);

        List<Vector2Int> deadEnds = FindAllDeadEnds(floorPositions);

		CreateRoomsAtDeadEnd(deadEnds, roomPositions);

        HashSet<Vector2Int> roomPositionsWithoutCorridors = new HashSet<Vector2Int>(roomPositions);
        roomPositionsWithoutCorridors.ExceptWith(corridorPositions);

        foreach (var position in roomPositionsWithoutCorridors)
        {
			continue;
			// astea sunt pozitiile de camera in care vrem sa punem obiecte / inamici
		}

        itemPlacementHelper.initialize(allRoomPositions, roomPositionsWithoutCorridors);

        itemPlacementHelper.PlaceItems();
		itemPlacementHelper.PlaceEnemies(firstRoomFloor);

        floorPositions.UnionWith(roomPositions);

        for (int i = 0; i < corridors.Count; i++)
		{
			corridors[i] = IncreaseCorridorSizeByOne(corridors[i]);
			corridors[i] = IncreaseCorridorBrush3by3(corridors[i]);
			floorPositions.UnionWith(corridors[i]);
		}

        tilemapVisualizer.PaintFloorTiles(floorPositions);
        List<WallTileData> possibleDoorList = new List<WallTileData>();

        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer, possibleDoorList);

		int idx = UnityEngine.Random.Range(0, possibleDoorList.Count);
        WallTileData doorTileData = possibleDoorList[idx];
        tilemapVisualizer.PaintSingleTile(doorTileData.wallTilemap, doorTileData.tile, doorTileData.position);
		door.transform.position = new Vector3Int(doorTileData.position.x, doorTileData.position.y, 0);

	}

	private List<Vector2Int> IncreaseCorridorBrush3by3(List<Vector2Int> corridor)
	{
		List<Vector2Int> newCorridor = new List<Vector2Int>();

		for (int i = 1; i < corridor.Count; i++)
		{
			for (int x = -1; x < 2; x++)
			{
				for (int y = -1; y < 2; y++)
				{
					newCorridor.Add(corridor[i - 1] + new Vector2Int(x, y));
				}
			}
		}

		return newCorridor;
	}

	private List<Vector2Int> IncreaseCorridorSizeByOne(List<Vector2Int> corridor)
	{
		List<Vector2Int> newCorridor = new List<Vector2Int>();
		Vector2Int previousDirection = Vector2Int.zero;

		for (int i = 1; i < corridor.Count; i++)
		{
			Vector2Int directionFromCell = corridor[i] - corridor[i - 1];
			if (previousDirection != Vector2Int.zero && directionFromCell != previousDirection)
			{
				// Schimbare de directie
				for (int x = -1; x < 2; x++)
				{
					for (int y = -1; y < 2; y++)
					{
						newCorridor.Add(corridor[i - 1] + new Vector2Int(x, y));
					}
				}
				previousDirection = directionFromCell;
			}
			else
			{
				// Adauga o casuta + 90 grade
				Vector2Int newCorridorTileOffset = GetDirection90From(directionFromCell);
				newCorridor.Add(corridor[i - 1]);
				newCorridor.Add(corridor[i - 1] + newCorridorTileOffset);
			}
		}
		return newCorridor;
	}

	private Vector2Int GetDirection90From(Vector2Int direction)
	{
		if (direction == Vector2Int.up)
			return Vector2Int.right;
		if (direction == Vector2Int.down)
			return Vector2Int.left;
		if (direction == Vector2Int.left)
			return Vector2Int.up;
		if (direction == Vector2Int.right)
			return Vector2Int.down;

		return Vector2Int.zero;
	}

	private void CreateRoomsAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloors)
	{
		foreach (var position in deadEnds)
		{
			if (!roomFloors.Contains(position))
			{
				var room = RunRandomWalk(randomWalkParameters, position);
				roomFloors.UnionWith(room);
			}
		}
	}

	private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPositions)
	{
		List<Vector2Int> deadEnds = new List<Vector2Int>();
		foreach (var position in floorPositions)
		{
			int neighborsCount = 0;
			foreach (var direction in Direction2D.cardinalDirectionsList)
			{
				if (floorPositions.Contains(position + direction))
				{
					neighborsCount++;
				}
			}
			if (neighborsCount == 1)
			{
				deadEnds.Add(position);
			}
		}

		return deadEnds;
	}

	private void ClearRoomData()
	{
		roomsDictionary.Clear();
		roomColors.Clear();
	}

	private void SaveRoomData(Vector2Int roomPosition, HashSet<Vector2Int> roomFloor)
	{
		roomsDictionary[roomPosition] = roomFloor;
		roomColors.Add(UnityEngine.Random.ColorHSV());
	}

	private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPositions)
	{
		HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
		int roomToCreateCount = Mathf.RoundToInt(potentialRoomPositions.Count * roomPercent);

		List<Vector2Int> roomsToCreate = potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();
		ClearRoomData();

		bool first_room = false;

		foreach (var roomPosition in roomsToCreate)
		{
			var roomFloor = RunRandomWalk(randomWalkParameters, roomPosition);
			if (first_room == false)
			{
				firstRoomFloor = roomFloor;
                first_room = true;
			}

			SaveRoomData(roomPosition, roomFloor);
			roomPositions.UnionWith(roomFloor);
		}

        // Debug.Log(roomPositions.First());
        // Debug.Log(roomPositions.GetType().Name);


        return roomPositions;
	}

	private List<List<Vector2Int>> CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPositions)
	{
		var currentPosition = startPosition;
		potentialRoomPositions.Add(currentPosition);
		List<List<Vector2Int>> corridors = new List<List<Vector2Int>>();

		for (int i = 0; i < corridorCount; i++)
		{
			var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPosition, corridorLength);
			corridors.Add(corridor);
			currentPosition = corridor[corridor.Count - 1];
			potentialRoomPositions.Add(currentPosition);
			floorPositions.UnionWith(corridor);
		}

		corridorPositions = new HashSet<Vector2Int>(floorPositions);

		return corridors;
	}
}
