﻿
// MemberLoopIncrementState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class MemberLoopIncrementState : State, IMemberLoopIncrementState
	{
		public override void Execute()
		{
			var monster = Globals.MDB[Globals.LoopMonsterUid];

			Debug.Assert(monster != null);

			Debug.Assert(Globals.LoopMemberNumber >= 0 && Globals.LoopMemberNumber <= monster.GroupCount && Globals.LoopMemberNumber <= 5);

			Globals.LoopMemberNumber++;

			if (Globals.LoopMemberNumber > monster.GroupCount || Globals.LoopMemberNumber > 5)
			{
				NextState = Globals.CreateInstance<IMonsterLoopIncrementState>();

				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IAttackLoopInitializeState>();
			}

			Globals.NextState = NextState;
		}

		public MemberLoopIncrementState()
		{
			Name = "MemberLoopIncrementState";
		}
	}
}

/* EamonCsCodeTemplate

// MemberLoopIncrementState.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static YourAdventureName.Game.Plugin.PluginContext;

namespace YourAdventureName.Game.States
{
	[ClassMappings]
	public class MemberLoopIncrementState : EamonRT.Game.States.MemberLoopIncrementState, IMemberLoopIncrementState
	{

	}
}
EamonCsCodeTemplate */
