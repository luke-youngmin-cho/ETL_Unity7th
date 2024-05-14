using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystemSample : MonoBehaviour
{
    public class Map
    {
        public float pressDelay = 0.4f;
        private Dictionary<KeyCode, Action> _keyDownActions = new Dictionary<KeyCode, Action>();
        private Dictionary<KeyCode, Action> _keyUpActions = new Dictionary<KeyCode, Action>();
        private Dictionary<KeyCode, Action> _keyPressActions = new Dictionary<KeyCode, Action>();
        private Dictionary<KeyCode, float> _keyDownTimeMarks = new Dictionary<KeyCode, float>();
        public void RegisterKeyDownAction(KeyCode keyCode, Action action) => _keyDownActions.TryAdd(keyCode, action);
        public void RegisterKeyUpAction(KeyCode keyCode, Action action) => _keyUpActions.TryAdd(keyCode, action);
        public void RegisterKeyPressAction(KeyCode keyCode, Action action) => _keyPressActions.TryAdd(keyCode, action);

        public void InputAction()
        {
            foreach (var item in _keyDownActions)
            {
                if (Input.GetKeyDown(item.Key))
                {
                    item.Value?.Invoke();
                }
            }

            foreach (var item in _keyUpActions)
            {
                if (Input.GetKeyDown(item.Key))
                {
                    item.Value?.Invoke();
                }
            }

            foreach (var item in _keyPressActions)
            {
                if (Input.GetKeyDown(item.Key))
                {
                    item.Value?.Invoke();
                }
            }
        }
    }
    public Dictionary<string, Map> maps = new Dictionary<string, Map>
    {
        { "BattleField" ,  new Map() },
        { "UI" ,  new Map() },
        { "Chat" ,  new Map() },
    };

    private void Awake()
    {
        maps["BattleField"].RegisterKeyDownAction(KeyCode.Escape, () => Debug.Log("어플 종료 팝업"));
        maps["BattleField"].RegisterKeyDownAction(KeyCode.I, () => Debug.Log("가방열기"));
        maps["Chat"].RegisterKeyDownAction(KeyCode.Return, () => Debug.Log("채팅종료"));
    }

    private void Update()
    {
        foreach (var map in maps)
        {
            map.Value.InputAction();
        }
    }
}
