﻿
// MonsterStartState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class MonsterStartState : State, IMonsterStartState
	{
		public override void Execute()
		{
			ProcessRevealContentArtifacts();

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterLoopInitializeState>();
			}

			Globals.NextState = NextState;
		}

		public MonsterStartState()
		{
			Name = "MonsterStartState";
		}
	}
}
