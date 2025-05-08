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
    private VisualElement m_GameOverPanel;
    private Label m_GameOverMessage;
    private int m_CurrentLevel = 1;
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
  
   m_FoodLabel = uiDocument.rootVisualElement.Q<Label>("FoodLabel");
  
   m_GameOverPanel = uiDocument.rootVisualElement.Q<VisualElement>("GameOverPanel");
   m_GameOverMessage = m_GameOverPanel.Q<Label>("GameOverMessage");

   StartNewGame();

    }

    public void StartNewGame()
    {
        m_GameOverPanel.style.visibility = Visibility.Hidden;

        m_CurrentLevel = 1;
        m_FoodLabel.text = "Food : " + m_FoodAmount;

        boardManager.Clean();
        boardManager.Init();

        playerController.Init();
        playerController.Spawn(boardManager, new Vector2Int(1, 1));
    }


    public void NewLevel()
    {
        boardManager.Clean();
        boardManager.Init();
        playerController.Spawn(boardManager, new Vector2Int(1, 1));

        m_CurrentLevel++;
    }

    void OnTurnHappen()
    {
        ChangeFood(-1);
    }

    public void ChangeFood(int amount)
    {

        m_FoodAmount += amount;
        m_FoodLabel.text = "Food : " + m_FoodAmount;

        if (m_FoodAmount <= 0)
        {
            playerController.GameOver();
            m_GameOverPanel.style.visibility = Visibility.Visible;
            m_GameOverMessage.text = "Game Over!\n\nSurvived " + m_CurrentLevel + " days";
        }

    }


    
}
