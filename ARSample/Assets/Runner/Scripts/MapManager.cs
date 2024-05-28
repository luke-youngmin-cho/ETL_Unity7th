using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] List<MapNote> _mapNotes;
    [SerializeField] MapUnit _mapUnit;
    [SerializeField] int _initNum = 5;
    [SerializeField] float _term = 1f;
    [SerializeField] float _speed = 3f;
    private Queue<MapUnit> _mapUnits;
    private List<Transform> _transforms;

    private void Awake()
    {
        _mapUnits = new Queue<MapUnit>(_initNum);
        _transforms = new List<Transform>(_initNum);
    }

    private void Start()
    {
        Spawn();
    }

    private void FixedUpdate()
    {
        MapUnit first = _mapUnits.Peek();

        if (first.transform.position.z < _term)
        {
            Roll();
        }

        for (int i = 0; i < _transforms.Count; i++)
        {
            _transforms[i].position += Vector3.back * _speed * Time.fixedDeltaTime;
        }
    }

    private void Spawn()
    {
        for (int i = 0; i < _initNum; i++)
        {
            MapUnit unit = Instantiate(_mapUnit);
            unit.transform.position = Vector3.forward *_term * i;
            _mapUnits.Enqueue(unit);
            _transforms.Add(unit.transform);

            if (Random.Range(0f, 1f) < 0.2f)
            {
                MapNote note = Instantiate(_mapNotes[Random.Range(0, _mapNotes.Count)]);
                note.transform.position = unit.transform.position + Vector3.up;
            }
        }
    }

    private void Roll()
    {
        MapUnit first = _mapUnits.Dequeue();
        _mapUnits.Enqueue(first);
        first.transform.position = Vector3.forward * _term * _mapUnits.Count;

        if (Random.Range(0f, 1f) < 0.2f)
        {
            MapNote note = Instantiate(_mapNotes[Random.Range(0, _mapNotes.Count)]);
            note.transform.position = first.transform.position + Vector3.up;
        }
    }
}
