using Cameras;
using Characters;
using Cinemachine;
using Commons;
using Controllers;
using Maps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

namespace GameSystems
{
    public class GameplayController : MonoBehaviour
    {
        public Character characterTemplate;
        public Hunter hunterTemplate;
        [SerializeField] MapGenerator mapGenerator;
        public SwipeController playerController;
        public BotController botController;

        //[SerializeField] FixedFocusCamera camera;
        [SerializeField] CinemachineVirtualCamera virtualCamera;
        public Transform _poolingObjectContainer;

        public static Character player;
        public static List<Character> otherCharacters = new List<Character>();
        public static Transform poolingObjectContainer;

        /// <summary>
        /// Dispatch when player gain or lose coins. Parameter is delta of coins.
        /// </summary>

        public int CollectedCoins { get; private set; }
        private void Awake()
        {
            poolingObjectContainer = _poolingObjectContainer;
        }
        void Start()
        {

            //camera.SetFocusTarget(character.transform);
            InitNormalGame();
            SignalHub.onGameEnd.AddListener(OnGameEnded);
            //onCoinUpdate.AddListener(UpdateCoins);
        }

        private void OnDestroy()
        {
            SignalHub.onGameEnd.RemoveListener(OnGameEnded);
        }

        public void NewGame()
        {
            Destroy(player.gameObject);
            otherCharacters.ForEach(x => Destroy(x.gameObject));
            mapGenerator.ClearMap();
            InitNormalGame();
            SignalHub.onAnotherGame.Invoke();
        }

        public void InitNormalGame()
        {
            otherCharacters.Clear();
            mapGenerator.GeneratePathBlock(Random.Range(50, 90));
            player = InitCharacter();
            player.onReachEnd.AddListener(OnPlayerReachEnd);
            player.ReadyOnPath(mapGenerator.startBlock);
            Hunter hunter = InitHunter(player);
            hunter.onCatch.AddListener(OnHunterCatch);
            otherCharacters.Add(hunter);
            // TODO: Init other character if want to scale gameplay
        }

        public void StartNormalGame()
        {
            player.Depart();
            otherCharacters.ForEach(x => x.ReadyOnPath(mapGenerator.hunterStartBlock));
            otherCharacters.ForEach(x => x.Depart());
            SignalHub.onGameStart.Invoke();
        }

        private void OnHunterCatch(Character target)
        {
            target.State = (int)CharacterState.Lose;
            // TODO: Check other character if want to scale gameplay
            //StartCoroutine(IGameOver());
            SignalHub.onGameEnd.Invoke(target != player);
        }

        private void OnPlayerReachEnd()
        {
            // Just for normal game
            player.State = (int)CharacterState.Win;
            otherCharacters.ForEach(c => c.State = (int)CharacterState.Lose);
            SignalHub.onGameEnd.Invoke(true);
        }

        private void OnGameEnded(bool isWin) => OnGameEnded();
        private void OnGameEnded()
        {
            player.Stop();
            otherCharacters.ForEach(x => x.Stop());
        }

        private Character InitCharacter()
        {
            var character = Instantiate(characterTemplate)
                .SetController(playerController);
            virtualCamera.Follow = virtualCamera.LookAt = character.transform;
            return character;
        }
        private Hunter InitHunter(Character target)
        {
            Hunter hunter = Instantiate(hunterTemplate)
                .SetTarget(target);
            //hunter.SetController(new BotController());
            hunter.SetController(botController);

            return hunter;
        }


    }


    /// <summary>
    /// -2: Idle
    /// -1: Lose
    /// 0: Running
    /// 1: Win
    /// </summary>
    public enum CharacterState
    {
        Idle = -2,
        Lose = -1,
        Running = 0,
        Win = 1,
        HunterWin = 2
    }
}
