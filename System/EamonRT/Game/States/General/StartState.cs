﻿
// StartState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class StartState : State, IStartState
	{
		/// <summary>
		/// This event fires at the start of a new round, before any processing has been done.
		/// </summary>
		public const long PeBeforeRoundStart = 1;

		public override void Execute()
		{
			ProcessEvents(PeBeforeRoundStart);

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IBurnDownLightSourceState>();
			}

			Globals.NextState = NextState;
		}

		public StartState()
		{
			Name = "StartState";
		}
	}
}

/* EamonCsCodeTemplate

// StartState.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static YourAdventureName.Game.Plugin.PluginContext;

namespace YourAdventureName.Game.States
{
	[ClassMappings]
	public class StartState : EamonRT.Game.States.StartState, IStartState
	{

	}
}
EamonCsCodeTemplate */
