using Commons;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace GameSystems
{
    public class GameMaster : MonoBehaviour
    {
        public static GameMaster Instance { get; private set; }
        public static Transform PoolingObjectContainer => Instance.poolingObjectContainer;
        public Transform poolingObjectContainer;
        const string SAVE_FILE_NAME = "player.dat";
        const string SAVE_FILE_PATH = "Data/";
        public static PlayerModel PlayerModel => Instance.playerModel;
        PlayerModel playerModel;
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            Load();
        }

        void Start()
        {
            SignalHub.onCoinUpdate.AddListener(UpdateCoins);
            SignalHub.onGameEnd.AddListener(Save);
        }

        void UpdateCoins(int delta)
        {
            playerModel.coins += delta;
        }

        public void Save(bool x) => Save();
        public void Save()
        {
            DataManager.Save(SAVE_FILE_NAME, SAVE_FILE_PATH, playerModel);
        }

        /// <summary>
        /// Load player data from file, only use this method on start. Don't use this method to sync.
        /// </summary>
        public void Load()
        {
            playerModel = DataManager.Load<PlayerModel>(SAVE_FILE_NAME, SAVE_FILE_PATH);
            if (playerModel == null)
            {
                playerModel = new PlayerModel();
            }
        }
    }
}