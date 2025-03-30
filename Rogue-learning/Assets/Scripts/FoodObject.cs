using UnityEngine;

public class FoodObject : CellObject
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public int AmountGranted = 5;
    public override void PlayerEntered()
    {
        Destroy(gameObject);
        // increase food
        GameManager.Instance.ChangeFood(AmountGranted);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
