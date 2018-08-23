﻿
// MonsterLoopIncrementState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class MonsterLoopIncrementState : State, IMonsterLoopIncrementState
	{
		public virtual bool ShouldProcessMonster(IMonster monster)
		{
			Debug.Assert(monster != null);

			return monster.Location == Globals.GameState.Ro && !monster.IsCharacterMonster();
		}

		public override void Execute()
		{
			while (true)
			{
				Globals.LoopMonsterUid++;

				var monster = Globals.MDB[Globals.LoopMonsterUid];

				if (monster != null)
				{
					if (ShouldProcessMonster(monster))
					{
						NextState = Globals.CreateInstance<IDefaultMonsterDecisionState>();

						goto Cleanup;
					}
				}
				else
				{
					goto Cleanup;
				}
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IEndOfRoundState>();
			}

			Globals.NextState = NextState;
		}

		public MonsterLoopIncrementState()
		{
			Name = "MonsterLoopIncrementState";
		}
	}
}

/* EamonCsCodeTemplate

// MonsterLoopIncrementState.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static YourAdventureName.Game.Plugin.PluginContext;

namespace YourAdventureName.Game.States
{
	[ClassMappings]
	public class MonsterLoopIncrementState : EamonRT.Game.States.MonsterLoopIncrementState, IMonsterLoopIncrementState
	{

	}
}
EamonCsCodeTemplate */
