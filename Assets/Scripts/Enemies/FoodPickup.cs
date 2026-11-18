using UnityEngine;

public class FoodPickup : Pickup
{
    [SerializeField] private int points = 50;
    [SerializeField] private float foodAmount = 0.2f;

    protected override void Collect()
    {
        GameManager.Instance.AddScore(points);
        GameManager.Instance.AddFood(foodAmount);
    }
}
