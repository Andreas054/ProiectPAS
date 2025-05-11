using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemPlacementHelper : MonoBehaviour
{
    [SerializeField]
    public int ItemsTotal = 40;
    [SerializeField]
    public GameObject ItemPrefab;
    public GameObject ItemPrefab2;
    [SerializeField]
    public GameObject ItemsContainer;


    Dictionary<PlacementType, HashSet<Vector2Int>>
        tileByType = new Dictionary<PlacementType, HashSet<Vector2Int>>();

    HashSet<Vector2Int> roomFloorNoCorridor;

    public void initialize(HashSet<Vector2Int> roomFloor, HashSet<Vector2Int> roomFloorNoCorridor)
    {
        tileByType.Clear();

        Graph graph = new Graph(roomFloor);
        this.roomFloorNoCorridor = roomFloorNoCorridor;

        foreach (var position in roomFloorNoCorridor)
        {
            int neighboursCount8Dir = graph.GetNeighours8Directions(position).Count;
            PlacementType type = neighboursCount8Dir < 8 ? PlacementType.NearWall : PlacementType.OpenSpace;

            if (tileByType.ContainsKey(type) == false)
                tileByType[type] = new HashSet<Vector2Int>();

            if (type == PlacementType.NearWall && graph.GetNeighours4Directions(position).Count > 3) // ???
                continue;

            tileByType[type].Add(position);
        }

    }

    public void PlaceItems()
    {
        List<Vector2?> itemPositions = new List<Vector2?>();

        foreach (Transform child in ItemsContainer.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < ItemsTotal; i++)
        {
            Vector2? itemPosition;

            if (i % 2 == 0)
            {
                itemPosition = GetItemPlacementPosition(PlacementType.NearWall, 25, ItemPrefab.GetComponent<Item>().size, false);
                if (itemPosition != null)
                {
                    GameObject newPrefabInstance = Instantiate(ItemPrefab, itemPosition.Value, Quaternion.identity);
                    newPrefabInstance.transform.SetParent(ItemsContainer.transform);
                }
            }
            else
            {
                itemPosition = GetItemPlacementPosition(PlacementType.NearWall, 25, ItemPrefab2.GetComponent<Item>().size, false);
                if (itemPosition != null)
                {
                    GameObject newPrefabInstance = Instantiate(ItemPrefab2, itemPosition.Value, Quaternion.identity);
                    newPrefabInstance.transform.SetParent(ItemsContainer.transform);
                }
            }

            itemPositions.Add(itemPosition);
        }

    }


    public Vector2? GetItemPlacementPosition(PlacementType placementType, int iterationsMax, Vector2Int size, bool addOffset)
    {
        int itemArea = size.x * size.y;
        if (tileByType[placementType].Count < itemArea)
            return null;

        int iteration = 0;
        while (iteration < iterationsMax)
        {
            iteration++;
            int index = UnityEngine.Random.Range(0, tileByType[placementType].Count);
            Vector2Int position = tileByType[placementType].ElementAt(index);

            if (itemArea > 1)
            {
                var (result, placementPosition) = PlaceBigItem(position, size, addOffset);

                if (result == false)
                    continue;

                tileByType[placementType].ExceptWith(placementPosition);
                tileByType[PlacementType.NearWall].ExceptWith(placementPosition);
            }
            else
            {
                tileByType[placementType].Remove(position);
            }

            return position;
        }
        return null;
    }

    private (bool, List<Vector2Int>) PlaceBigItem(Vector2Int originPosition, Vector2Int size, bool addOffset)
    {
        List<Vector2Int> positions = new List<Vector2Int>() { originPosition };
        int maxX = addOffset ? size.x + 1 : size.x;
        int maxY = addOffset ? size.y + 1 : size.y;
        int minX = addOffset ? -1 : 0;
        int minY = addOffset ? -1 : 0;

        for (int row = minX; row <= maxX; row++)
        {
            for (int col = minY; col <= maxY; col++)
            {
                if (row == 0 && col == 0)
                    continue;

                Vector2Int newPosToCheck = new Vector2Int(originPosition.x + row, originPosition.y + col);
                if (roomFloorNoCorridor.Contains(newPosToCheck) == false)
                    return (false, positions);
                positions.Add(newPosToCheck);
            }
        }
        return (true, positions);
    }

}


public enum PlacementType
{
    OpenSpace,
    NearWall
}