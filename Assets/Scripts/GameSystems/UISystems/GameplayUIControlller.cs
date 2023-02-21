using VisualizeFXs;
using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;

namespace GameSystems.UISystems
{
    public class GameplayUIControlller : MonoBehaviour
    {
        [SerializeField] TMP_Text txtCoin, txtGameResult;
        [SerializeField] DampingFX endgamePanel, btnStart, btnAnotherGame;

        private void Start()
        {
            SignalHub.onCoinUpdate.AddListener(UpdateCoins);
            SignalHub.onGameEnd.AddListener(OnGameEnd);
            SignalHub.onGameStart.AddListener(OnGameStart);
            SignalHub.onAnotherGame.AddListener(OnAnotherGame);
            txtCoin.text = GameMaster.PlayerModel.coins.ToString();
        }
        private void OnDestroy()
        {
            SignalHub.onCoinUpdate.RemoveListener(UpdateCoins);
            SignalHub.onGameEnd.RemoveListener(OnGameEnd);
            SignalHub.onGameStart.RemoveListener(OnGameStart);
            SignalHub.onAnotherGame.RemoveListener(OnAnotherGame);
        }

        private void UpdateCoins(int delta)
        {
            txtCoin.text = GameMaster.PlayerModel.coins.ToString();
            TextBubble.Spawn(txtCoin.transform).SetData("+" + delta);
        }

        private void OnGameEnd(bool isWin = false)
        {
            txtGameResult.text = isWin ? "<color=#00B4FF>You Win!</color>" : "You Lose!";
            endgamePanel.gameObject.SetActive(true);
            btnAnotherGame.gameObject.SetActive(true);
        }

        private void OnGameStart()
        {
            endgamePanel.BackToOrigin();
            btnStart.BackToOrigin();
        }

        private void OnAnotherGame()
        {
            btnAnotherGame.BackToOrigin();
            endgamePanel.BackToOrigin();
            btnStart.gameObject.SetActive(true);
        }


    }
}