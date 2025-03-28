using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public BoardManager boardManager;
    public PlayerController playerController;
    public TurnManager turnManager { get; private set; }
    

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        turnManager = new TurnManager();
        boardManager.Init();


        playerController.Spawn(boardManager, new Vector2Int(1, 1));

    }



    // Update is called once per frame
    void Update()
    {
  
    }
}
