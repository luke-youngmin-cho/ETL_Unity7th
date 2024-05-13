using UnityEngine;

public class MockUnitOfWork : IUnitOfWork
{
    public MockUnitOfWork()
    {
        gps = new GameObject(nameof(MockGPS)).AddComponent<MockGPS>();
    }


    public IGPS gps { get; private set; }
}