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
    public PlayerController Player;
    private CellData[,] m_BoardData;

    public class CellData
    {
        public bool Passable;
    }

  
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Tilemap = GetComponentInChildren<Tilemap>();
        m_Grid = GetComponentInChildren<Grid>();
        m_BoardData = new CellData[width, height];

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
    
                }
 
                m_Tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }

        Player.Spawn(this, new Vector2Int(1, 1));
        
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
