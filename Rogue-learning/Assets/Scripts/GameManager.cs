using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public BoardManager boardManager;
    public PlayerController playerController;
    private int m_FoodAmount = 100;
    public UIDocument uiDocument;
    private Label m_FoodLabel;
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
        turnManager.OnTick += OnTurnHappen;
        boardManager.Init();
        m_FoodLabel = uiDocument.rootVisualElement.Q<Label>("FoodLabel");
        m_FoodLabel.text = $"Food : {m_FoodAmount}";


        playerController.Spawn(boardManager, new Vector2Int(1, 1));

    }

    void OnTurnHappen()
    {
        ChangeFood(-1);
    }

    public void ChangeFood(int amount)
    {
        m_FoodAmount += amount;
        m_FoodLabel.text = "Food : " + m_FoodAmount;
    }


    // Update is called once per frame
    void Update()
    {
  
    }
}
