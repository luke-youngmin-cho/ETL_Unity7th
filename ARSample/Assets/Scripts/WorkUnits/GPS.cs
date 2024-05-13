using UnityEngine;

public class GPS : MonoBehaviour, IGPS
{
    public float latitude => throw new System.NotImplementedException();

    public float longitude => throw new System.NotImplementedException();

    public bool isDirty => throw new System.NotImplementedException();
}