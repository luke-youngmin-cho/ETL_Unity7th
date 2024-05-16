using UnityEngine;

public class MockGPS : MonoBehaviour, IGPS
{
    public float latitude
    {
        get => _latitude;
        private set
        {
            _latitude = value;
            isDirty = true;
        }
    }

    public float longitude
    {
        get => _longitude;
        private set
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
        private set
        {
            _isDirty = value;
        }
    }


    [SerializeField] private float _latitude = 37.5138649f;
    [SerializeField] private float _longitude = 127.0295296f;
    [SerializeField] private bool _isDirty;
}