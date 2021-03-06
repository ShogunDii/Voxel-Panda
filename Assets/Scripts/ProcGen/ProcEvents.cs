﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelPanda.Player.Events;
using VoxelPanda.ProcGen.Poolers;
using VoxelPanda.ProcGen.Spawners;

namespace VoxelPanda.ProcGen
{
	public class SpawnerData
	{
		public ISpawning spawner;
		public int lastZGenerated;
		public int generationBuffer;
		public int generationOffset;
		public int batchHeight;
		public static int gridWidth = 10;
		public bool isInitialAreaSpawner = false;

		public SpawnerData(ISpawning spawner, int generationOffset = 0, int generationBuffer = 50, int batchHeight = 40, int lastZ = 0)
		{
			this.spawner = spawner;
			this.lastZGenerated = lastZ;
			this.generationBuffer = generationBuffer;
			this.generationOffset = generationOffset;
			this.batchHeight = batchHeight;
		}

		public void TryToSpawn(int ZPosition)
		{
			if(this.lastZGenerated < ZPosition + generationBuffer)
			{
				this.lastZGenerated = spawner.SpawnGrid(this.lastZGenerated + generationOffset, 10, batchHeight);
			}
		}
	}

	public class ProcEvents : IMoveListener
	{
		public List<SpawnerData> spawners = new List<SpawnerData>();
		public List<IPooling> poolers = new List<IPooling>();
		private bool canStartListeningToEvents = false;

		public ProcEvents(MoveEvents moveEvents)
		{
			moveEvents.Subscribe(this);
		}

		public void AddSpawningListener(SpawnerData spawnerData, bool addToInitialSpawners = false)
		{
			spawnerData.isInitialAreaSpawner = addToInitialSpawners;
			spawners.Add(spawnerData);
		}

		public void AddPoolingListener(IPooling pooling)
		{
			poolers.Add(pooling);
		}

		public void OnPositionChanged(Vector3 position)
		{
			if (canStartListeningToEvents) {
				int ZPosition = (int)position.z;
				for (int i = 0; i < spawners.Count; i++)
				{
					spawners[i].TryToSpawn(ZPosition);

				}
				for (int i = 0; i < poolers.Count; i++)
				{
					poolers[i].TryDespawning(position);
				}
			}
		}

		public void GenerateInitial(Vector3 position, int offsetPosition = 20)
		{
			int ZPosition = (int)position.z;
			for(int i = 0; i < spawners.Count; i++)
			{
				if (spawners[i].isInitialAreaSpawner)
				{
					spawners[i].TryToSpawn(ZPosition);
				} else
				{
					spawners[i].lastZGenerated = offsetPosition;
				}
			}
			for(int i = 0; i < poolers.Count; i++)
			{
				poolers[i].TryDespawning(position);
			}
			canStartListeningToEvents = true;
		}

		public void OnVelocityChanged(Vector3 velocity)
		{
			
		}

        public void ClearSubs()
        {
            spawners.Clear();
        }

		public void DespawnAll()
		{
			for (int i = 0; i < poolers.Count; i++)
			{
				poolers[i].DespawnAll();
			}
		}

		public void ResetAll()
		{
			canStartListeningToEvents = false;
			for (int i = 0; i < spawners.Count; i++)
			{
				spawners[i].lastZGenerated = 0;
			}
			DespawnAll();
		}


	}
}