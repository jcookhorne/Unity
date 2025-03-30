using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    private Tilemap m_Tilemap;
    public int width;
    public int height;
    public Tile[] GroundTiles;
    public Tile[] WallTiles;
    private Grid m_Grid;
    public int LeastFoodAmount;
    public int MaxFoodAmount;
    public PlayerController Player;
    public FoodObject[] FoodPrefab;
    private CellData[,] m_BoardData;
    private List<Vector2Int> m_EmptyCellsList;

    public class CellData
    {
        public bool Passable;
        public CellObject ContainedObject;
    }


    void GenerateFood()
    {
        int randomFood = Random.Range(LeastFoodAmount, MaxFoodAmount);

        for (int i = 0; i < randomFood; ++i)
        {

            Debug.Log(randomFood);
            int randomIndex = Random.Range(randomFood, m_EmptyCellsList.Count);
            Debug.Log(randomIndex + " " + m_EmptyCellsList.Count);
            Vector2Int coord = m_EmptyCellsList[randomIndex];
            Debug.Log(coord);

            m_EmptyCellsList.RemoveAt(randomIndex);
            CellData data = m_BoardData[coord.x, coord.y];
            FoodObject food = FoodPrefab[Random.Range(0, FoodPrefab.Length)];
            Debug.Log(food);
            FoodObject newFood = Instantiate(food);
            newFood.transform.position = CellToWorld(coord);
            data.ContainedObject = newFood;

        }
    }
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
                    tile = WallTiles[Random.Range(0, WallTiles.Length)];
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


    // Update is called once per frame
    void Update()
    {
        
    }
}
