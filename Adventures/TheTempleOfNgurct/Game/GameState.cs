﻿
// GameState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game
{
	[ClassMappings(typeof(Eamon.Framework.IGameState))]
	public class GameState : Eamon.Game.GameState, Framework.IGameState
	{
		public virtual long WanderingMonster { get; set; }

		public virtual long DwLoopCounter { get; set; }

		public virtual long WandCharges { get; set; }

		public virtual long Regenerate { get; set; }

		public virtual long KeyRingRoomUid { get; set; }

		public virtual bool AlkandaKilled { get; set; }

		public virtual bool AlignmentConflict { get; set; }

		public virtual bool CobraAppeared { get; set; }

		public GameState()
		{
			// Sets up wandering monsters and fireball wand charges

			WanderingMonster = Globals.Engine.RollDice01(1, 14, 11);

			WandCharges = Globals.Engine.RollDice01(1, 4, 1);
		}
	}
}
