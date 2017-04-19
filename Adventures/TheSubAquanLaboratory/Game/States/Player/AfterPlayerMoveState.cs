﻿
// AfterPlayerMoveState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using TheSubAquanLaboratory.Framework;
using TheSubAquanLaboratory.Framework.States;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.States
{
	[ClassMappings(typeof(EamonRT.Framework.States.IAfterPlayerMoveState))]
	public class AfterPlayerMoveState : EamonRT.Game.States.AfterPlayerMoveState, IAfterPlayerMoveState
	{
		protected override void ProcessEvents()
		{
			var gameState = Globals.GameState as IGameState;

			Debug.Assert(gameState != null);

			if (gameState.R3 == 43 && gameState.Ro != gameState.R3)
			{
				var artifact = Globals.ADB[84];

				Debug.Assert(artifact != null);

				if (!artifact.IsInLimbo())
				{
					artifact = Globals.ADB[16];

					Debug.Assert(artifact != null);

					if (artifact.IsInLimbo())
					{
						artifact.SetInRoomUid(43);
					}
				}

				gameState.Sterilize = false;
			}

			base.ProcessEvents();
		}
	}
}
