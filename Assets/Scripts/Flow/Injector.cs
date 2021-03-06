﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelPanda.Player.Events;
using VoxelPanda.ProcGen;
using VoxelPanda.Score;

namespace VoxelPanda.Flow 
{
	public class Injector : MonoBehaviour
	{
		public ConstMoveData constMoveData;
        public DynamicMoveData dMoveData;
		public SpawnData spawnData;
		public ScoreUI scoreUI;
		public DeathUI deathUI;
		public PlayerElements playerElements;
		public Crusher crusher;
		public AdManager adManager;
		public Tutorial tutorial;
        public OptionsController optionsController;
		public string randomSeed;

		private MoveEvents moveEvents;
		private ProcEvents procEvents;
		private ScoreCalculator scoreCalculator;
		private DeathController deathController;
		private GameManager gameManager;

		private const string playAliveEvent = "Play_GameplayMusic";

		private void Start()
		{
			AkSoundEngine.PostEvent(playAliveEvent, Camera.main.transform.GetChild(0).gameObject);
		}

		private void Awake()
		{
			BindAll();
		}

		public void BindAll()
		{
			moveEvents = new MoveEvents(dMoveData);
			procEvents = new ProcEvents(moveEvents);


			scoreCalculator = new ScoreCalculator(moveEvents);
			scoreCalculator.Subscribe(scoreUI);
            ProcGenInjector pgInjector = new ProcGenInjector(spawnData, procEvents, scoreCalculator);
            pgInjector.BindAll();
            deathController = new DeathController(scoreCalculator, deathUI, adManager);
			gameManager = new GameManager(playerElements, deathController, crusher, procEvents, scoreCalculator, pgInjector);
			moveEvents.Subscribe(gameManager);
			if (!string.IsNullOrEmpty(randomSeed)) { gameManager.SetRandomSeed(randomSeed); } else {
				randomSeed = Random.seed.ToString();
			}
			crusher.Bind(gameManager, playerElements.physicsController.transform, scoreCalculator);
            optionsController.SetGameManager(gameManager);

			InputInjector inputInjector = new InputInjector(constMoveData, dMoveData, moveEvents, playerElements, crusher, tutorial);

			gameManager.StartLevel();
		}
	}
}

