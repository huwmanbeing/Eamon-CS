﻿
// BeforePlayerMoveState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class BeforePlayerMoveState : State, IBeforePlayerMoveState
	{
		/// <summary>
		/// An event that fires after the player's destination <see cref="IRoom">Room</see> <see cref="IGameBase.Uid"> Uid</see>
		/// is calculated and stored.
		/// </summary>
		public const long PeAfterDestinationRoomSet = 1;

		public virtual IRoom Room { get; set; }

		public virtual Direction Direction { get; set; }

		public virtual IArtifact Artifact { get; set; }

		public override void ProcessEvents(long eventType)
		{
			if (eventType == PeAfterDestinationRoomSet)
			{
				if (gGameState.GetNBTL(Friendliness.Enemy) > 0 && Room.IsLit())
				{
					PrintEnemiesNearby();

					NextState = Globals.CreateInstance<IStartState>();
				}
			}
		}

		public override void Execute()
		{
			Debug.Assert(Enum.IsDefined(typeof(Direction), Direction) || Artifact != null);

			Room = gRDB[gGameState.Ro];

			Debug.Assert(Room != null);

			gGameState.R2 = Artifact != null ? 0 : Room.GetDirs(Direction);

			ProcessEvents(PeAfterDestinationRoomSet);

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IPlayerMoveCheckState>(x =>
				{
					x.Direction = Direction;

					x.Artifact = Artifact;
				});
			}

			Globals.NextState = NextState;
		}

		public BeforePlayerMoveState()
		{
			Name = "BeforePlayerMoveState";
		}
	}
}
