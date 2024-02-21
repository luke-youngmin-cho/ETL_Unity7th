using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardGamePlayManager : MonoBehaviour
{
    [SerializeField] Transform[] _nodes;
    [SerializeField] Transform _player;
    [SerializeField] float _moveSpeed = 1.0f;
    private int _playerLocationIndex;
    [SerializeField] BoardGamePlayStatusUI _statusUI;

    private void Start()
    {
        RollADice();
    }

    public void RollADice()
    {
        int value = Random.Range(1, 7);
        _statusUI.PlayRollingAnimation(value, DoMove);
    }

    private void DoMove(int value)
    {
        StartCoroutine(C_Move(_playerLocationIndex, value));
        _playerLocationIndex = _playerLocationIndex + value <= _nodes.Length - 1 ? _playerLocationIndex + value : _nodes.Length - 1;
    }

    IEnumerator C_Move(int currentIndex, int value)
    {
        for (int i = 0; i < value; i++)
        {
            if (currentIndex + 1 >= _nodes.Length)
                break;

            float t = 0.0f;
            while (t < 1.0f)
            {
                _player.position = Vector3.Lerp(_nodes[currentIndex].position, _nodes[currentIndex + 1].position, t);
                t += _moveSpeed * Time.deltaTime;
                yield return null;
            }
            _player.position = _nodes[currentIndex + 1].position;
            currentIndex++;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
