using UnityEngine;

public class MockGPS : MonoBehaviour, IGPS
{
    public float latitude
    {
        get => _latitude;
        set
        {
            _latitude = value;
            isDirty = true;
        }
    }

    public float longitude
    {
        get => _longitude;
        set
        {
            _longitude = value;
            isDirty = true;
        }
    }

    public bool isDirty 
    {
        get
        {
            if (_isDirty)
            {
                _isDirty = false;
                return true;
            }

            return false;
        }
        set
        {
            _isDirty = value;
        }
    }


    private float _latitude = 37.5138649f;
    private float _longitude = 127.0295296f;
    private bool _isDirty;
}