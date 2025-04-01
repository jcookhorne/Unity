using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    private Tilemap m_Tilemap;
    public int width;
    public int height;
    public Tile[] GroundTiles;
    public Tile[] BorderTiles;
    private Grid m_Grid;
    public int LeastFoodAmount;
    public int MaxFoodAmount;
    public PlayerController Player;
    public FoodObject[] FoodPrefab;
    private CellData[,] m_BoardData;
    private List<Vector2Int> m_EmptyCellsList;
    public ObstacleObject[] ObstaclePrefab;
    public ExitCellObject ExitCellPrefab;
 

    public class CellData
    {
        public bool Passable;
        public CellObject ContainedObject;
    }
    public Tile GetCellTile(Vector2Int cellIndex)
    {
        return m_Tilemap.GetTile<Tile>(new Vector3Int(cellIndex.x, cellIndex.y, 0));
    }

    public void SetCellTile(Vector2Int cellIndex, Tile tile)
    {
        m_Tilemap.SetTile(new Vector3Int(cellIndex.x, cellIndex.y, 0), tile);
    }

    void AddObject(CellObject obj, Vector2Int coord)
    {
        CellData data = m_BoardData[coord.x, coord.y];
        obj.transform.position = CellToWorld(coord);
        data.ContainedObject = obj;
        obj.Init(coord);

    }
    void GenerateFood()
    {
        int randomFood = Random.Range(LeastFoodAmount, MaxFoodAmount);
        for (int i = 0; i < randomFood; ++i)
        {
            int randomIndex = Random.Range(randomFood, m_EmptyCellsList.Count);
            //Debug.Log(randomIndex + " " + m_EmptyCellsList.Count);
            Vector2Int coord = m_EmptyCellsList[randomIndex];

            m_EmptyCellsList.RemoveAt(randomIndex);
            CellData data = m_BoardData[coord.x, coord.y];
            FoodObject food = FoodPrefab[Random.Range(0, FoodPrefab.Length)];
            
            FoodObject newFood = Instantiate(food);
           AddObject(newFood, coord);

        }
    }
    
    void GenerateObstacle()
    {
        int wallCount = Random.Range(10, 18);
        for (int i = 0; i < wallCount; i++)
        {
            int randomIndex = Random.Range(0, m_EmptyCellsList.Count);
            Vector2Int coord = m_EmptyCellsList[randomIndex];
            m_EmptyCellsList.RemoveAt(randomIndex);
            CellData data = m_BoardData[coord.x, coord.y];
            ObstacleObject randomObstacle = ObstaclePrefab[Random.Range(0, ObstaclePrefab.Length)];

            ObstacleObject newObstacle = Instantiate(randomObstacle);
            AddObject(newObstacle, coord);

        }
    }

   public void Clean()
    {
        // no board data, so exit early, nothing to clean
        if(m_BoardData == null)
            return;

        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                var cellData = m_BoardData[x, y];

                if(cellData.ContainedObject != null)
                {
                    // CAREFUL Destroy the Game Object NOT just cellData.ContainedObject
                    // Otherwise what you are destroying is the JUST CellObject COMPONENT
                    // and no the whole gameobnject with sprite

                    Destroy(cellData.ContainedObject.gameObject);
                }

                SetCellTile(new Vector2Int(x, y), null);

            }
        }
    }

    public void Init()
    {
        m_Tilemap = GetComponentInChildren<Tilemap>();
        m_Grid = GetComponentInChildren<Grid>();
        m_BoardData = new CellData[width, height];
        m_EmptyCellsList = new List<Vector2Int>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {

                Tile tile;
                m_BoardData[x, y] = new CellData();

                if (x == 0 ||  y == 0|| x== width -1 || y == height - 1)
                {
                    tile = BorderTiles[Random.Range(0, BorderTiles.Length)];
                    m_BoardData[x, y].Passable = false;
                }
                else
                {
                    tile = GroundTiles[Random.Range(0, GroundTiles.Length)];
                    m_BoardData[x, y].Passable = true;
                    m_EmptyCellsList.Add(new Vector2Int(x, y));
    
                }
 
                m_Tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }

        m_EmptyCellsList.Remove(new Vector2Int(1, 1));

        Vector2Int endCoord = new Vector2Int(width - 2, height - 2);
        AddObject(Instantiate(ExitCellPrefab), endCoord);
        m_EmptyCellsList.Remove(endCoord);
        
        GenerateObstacle();
        GenerateFood();
    }

    public CellData GetCellData(Vector2Int cellIndex)
    {
        if(cellIndex.x<0 || cellIndex.x >= width || cellIndex.y <0 || cellIndex.y >= height)
        {
            return null;
        }
        return m_BoardData[cellIndex.x, cellIndex.y];
    }
    public Vector3 CellToWorld(Vector2Int cellIndex)
    {
        return m_Grid.GetCellCenterWorld((Vector3Int)cellIndex);
    }

    
}
