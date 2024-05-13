using UnityEngine;

public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork()
    {
        gps = new GameObject(nameof(GPS)).AddComponent<GPS>();
    }

    public IGPS gps { get; private set; }
}
