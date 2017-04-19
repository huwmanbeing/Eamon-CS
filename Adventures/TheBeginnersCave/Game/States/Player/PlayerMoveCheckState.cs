﻿
// PlayerMoveCheckState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using Eamon.Game.Attributes;
using TheBeginnersCave.Framework.States;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game.States
{
	[ClassMappings(typeof(EamonRT.Framework.States.IPlayerMoveCheckState))]
	public class PlayerMoveCheckState : EamonRT.Game.States.PlayerMoveCheckState, IPlayerMoveCheckState
	{
		protected override void ProcessEvents01()
		{
			if (Globals.GameState.R2 == -1)
			{
				Globals.Out.WriteLine("{0}Sorry, but I'm afraid to go into the water without my life preserver.", Environment.NewLine);

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IMonsterStartState>();
			}
			else
			{
				base.ProcessEvents01();
			}
		}
	}
}
