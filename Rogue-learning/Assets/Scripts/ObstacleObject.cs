using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObstacleObject : CellObject
{
    public Tile ObstacleTile;
    public int ObstacleMaxHealth = 3;
    private int m_HealthPoint;
    private Tile m_OriginalTile;
    public Tile almostBrokenObstacle;
    

    public override void Init(Vector2Int cell)
    {
        base.Init(cell);
        m_HealthPoint = ObstacleMaxHealth;

        m_OriginalTile = GameManager.Instance.boardManager.GetCellTile(cell);
        

        GameManager.Instance.boardManager.SetCellTile(cell, ObstacleTile);
    }

    public override bool PlayerWantsToEnter()
    {

        m_HealthPoint -= 1;

        if (m_HealthPoint == 1 && m_HealthPoint > 0)
        {
            Debug.Log("SHOW ME: " + m_HealthPoint);
            GameManager.Instance.boardManager.SetCellTile(m_Cell, almostBrokenObstacle);
            return false;
        }
        else if (m_HealthPoint > 1)
        {
            return false;
        }

        GameManager.Instance.boardManager.SetCellTile(m_Cell, m_OriginalTile);
        Destroy(gameObject);

        return true;
    }

}
