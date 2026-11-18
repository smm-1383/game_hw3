public class GiftBox : Pickup
{
    protected override void Collect()
    {
        GameManager.Instance.AddPower(1);
    }
}
