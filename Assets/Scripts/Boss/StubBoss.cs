using UnityEngine;

public class StubBoss : Boss
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float eggInterval = 1.2f;
    [SerializeField] private float hoverViewportY = 0.78f;
    [SerializeField] private float sideMargin = 2.5f;

    private class PatrolState : BossState
    {
        private readonly StubBoss stub;
        private int direction = 1;
        private float eggTimer;

        public PatrolState(StubBoss owner) : base(owner)
        {
            stub = owner;
        }

        public override void Enter()
        {
            eggTimer = stub.eggInterval;
        }

        public override void Tick()
        {
            Transform body = stub.transform;
            float hoverY = ScreenArea.FromViewport(0.5f, stub.hoverViewportY).y;
            if (body.position.y > hoverY)
            {
                body.position += Vector3.down * stub.moveSpeed * Time.deltaTime;
                return;
            }
            body.position += Vector3.right * direction * stub.moveSpeed * Time.deltaTime;
            if (body.position.x > ScreenArea.Right - stub.sideMargin)
            {
                direction = -1;
            }
            else if (body.position.x < ScreenArea.Left + stub.sideMargin)
            {
                direction = 1;
            }
            eggTimer -= Time.deltaTime;
            if (eggTimer <= 0f)
            {
                eggTimer = stub.eggInterval;
                stub.DropEgg();
            }
        }
    }

    protected override void BuildStates()
    {
        transform.position = new Vector3(0f, ScreenArea.Top + 3f, 0f);
        machine.ChangeState(new PatrolState(this));
    }

    private void DropEgg()
    {
        if (data.eggPrefab != null)
        {
            PoolManager.Instance.Get(data.eggPrefab, transform.position, Quaternion.identity);
        }
    }
}
