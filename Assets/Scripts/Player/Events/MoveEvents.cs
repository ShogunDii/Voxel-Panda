﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelPanda.Player.Events
{
	public class MoveEvents : MonoBehaviour
	{
		private List<IMoveListener> listeners = new List<IMoveListener>();

		public void Subscribe(IMoveListener listener)
		{
			if (!listeners.Contains(listener))
			{
				listeners.Add(listener);
			}
		}

		private void NotifyPositionChanged(Vector3 newPosition)
		{
			foreach (var listener in listeners)
			{
				listener.OnPositionChanged(newPosition);
			}
		}

		private void NotifyVelocityChanged(Vector3 newVelocity)
		{
			foreach(var listener in listeners)
			{
				listener.OnVelocityChanged(newVelocity);
			}
		}
	}
}