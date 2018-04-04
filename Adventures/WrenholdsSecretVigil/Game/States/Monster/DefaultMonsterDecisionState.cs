﻿
// DefaultMonsterDecisionState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using WrenholdsSecretVigil.Framework.States;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.States
{
	[ClassMappings(typeof(EamonRT.Framework.States.IDefaultMonsterDecisionState))]
	public class DefaultMonsterDecisionState : EamonRT.Game.States.DefaultMonsterDecisionState, IDefaultMonsterDecisionState
	{
		public override void Execute()
		{
			var monster = Globals.MDB[Globals.LoopMonsterUid];

			Debug.Assert(monster != null);

			// Try to open running device, all flee

			if (monster.CanMoveToRoom(true) && Globals.DeviceOpened)
			{
				NextState = Globals.CreateInstance<EamonRT.Framework.States.IBeforeMonsterFleesRoomState>();

				Globals.NextState = NextState;
			}
			else
			{
				base.Execute();
			}
		}
	}
}

