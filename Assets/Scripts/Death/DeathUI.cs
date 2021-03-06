﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoxelPanda.Flow;
using VoxelPanda.Player.Presentation;
using VoxelPanda.Score;
using TMPro;

namespace VoxelPanda.Flow
{
	public class DeathUI : MonoBehaviour
	{
        //Death screen
		public GameObject deathScreen;
		public GameObject deathOptionsPanel;
		public ScoreCalculator scoreCalculator;
        public TextMeshProUGUI scoreText;
		public TextMeshProUGUI scoreNumberText;
		//Revival prompt
		public GameObject revivalPrompt;
		public TextMeshProUGUI restartButtonText;
		//Get Ready panel
		public GameObject getReadyPanel;
		public TextMeshProUGUI countdownTimer;
        public UISFX uiSFX;

		public string restartText = "Try Again";
		public string reviveText = "Revive!";
		public string regularScorePrefix = "High score: ";
		public string highScoreReachedPrefix = "High Score Reached!";
		public string getReadyText = "Go!";
		private string countdownText = "";
		public float countdownTime = 3f;
		private float currentTime = 0f;
		private bool isCountingDown = false;
		private DeathController deathController;

		public void Bind(DeathController deathController, ScoreCalculator scoreCalculator)
		{
			this.deathController = deathController;
            this.scoreCalculator = scoreCalculator;
		}

		private void Update()
		{
			UpdateCountDown();
		}

		private void UpdateCountDown()
		{
			if(isCountingDown)
			{
                //if (currentTime < 0.2f)
                //{
                countdownText = getReadyText;
				//} //else
				//{
				//	countdownText = Mathf.Ceil(currentTime).ToString("0");
				//}
				countdownTimer.text = countdownText;
				currentTime -= Time.deltaTime;
				if(currentTime < 0)
				{
					TimerCountOut();
				}
			}
		}

		public void RaiseScreen()
		{
			revivalPrompt.SetActive(false);
			ChangeRestartText(false);
			deathOptionsPanel.SetActive(true);
			getReadyPanel.SetActive(false);
			deathScreen.SetActive(true);
            scoreNumberText.text = scoreCalculator.GetScore().ToString();
			if (scoreCalculator.HighScoreReached())
			{
				scoreText.text = highScoreReachedPrefix;
			} else
			{
				scoreText.text = regularScorePrefix;
			}
		}
		public void LowerScreen()
		{
			deathScreen.SetActive(false);
		}

		public void StartAgain()
		{
            uiSFX.PlayUIClick();
            StartCountDown();
		}

		public void Quit()
		{
            uiSFX.PlayUIClick();
			deathController.Quit();
		}

		private void StartCountDown()
		{
			deathOptionsPanel.SetActive(false);
			getReadyPanel.SetActive(true);
			isCountingDown = true;
			currentTime = countdownTime;
            uiSFX.PlayCountdown();

        }

		private void TimerCountOut()
		{
			deathOptionsPanel.SetActive(true);
			getReadyPanel.SetActive(false);
			isCountingDown = false;
			deathController.StartAgain();
		}

		public void ChangeRestartText(bool shouldRevive)
		{
			restartButtonText.text = (shouldRevive) ? reviveText : restartText;
		}

		public void EnableRevivalPrompt()
		{
			revivalPrompt.SetActive(true);
		}

		public void OnRevivalPromptClick()
		{
            uiSFX.PlayUIClick(true);
			deathController.TryToRevive();
			revivalPrompt.SetActive(false);
		}
	}
}


		
