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

    public class CellData
    {
        public bool Passable;
    }
    private CellData[,] m_BoardData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Tilemap = GetComponentInChildren<Tilemap>();

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
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
