using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace GameSystems
{
    public static class SignalHub
    {
        public static UnityEvent<int> onCoinUpdate = new UnityEvent<int>();

        public static UnityEvent onGameStart = new UnityEvent();
        public static UnityEvent<bool> onGameEnd = new UnityEvent<bool>();
        public static UnityEvent onAnotherGame = new UnityEvent();
    }
}