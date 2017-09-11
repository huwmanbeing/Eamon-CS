﻿
// EatCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using TheTempleOfNgurct.Framework.Commands;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IEatCommand))]
	public class EatCommand : EamonRT.Game.Commands.EatCommand, IEatCommand
	{
		protected virtual long DmgTaken { get; set; }

		protected override void PrintVerbItAll(Eamon.Framework.IArtifact artifact)
		{
			// Carcass

			if (artifact.Uid == 67)
			{
				PrintOkay(artifact);
			}
		}

		protected override void PrintFeelBetter(Eamon.Framework.IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			if (DmgTaken > 0)
			{
				Globals.Out.Write("{0}Some of your wounds seem to clear up.{0}", Environment.NewLine);
			}
		}

		protected override void PrintCantVerbHere()
		{
			PrintEnemiesNearby();
		}

		protected override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			DmgTaken = ActorMonster.DmgTaken;

			base.PlayerExecute();
		}

		protected override bool IsAllowedInRoom()
		{
			return Globals.GameState.GetNBTL(Enums.Friendliness.Enemy) <= 0;
		}

		public EatCommand()
		{
			IsPlayerEnabled = true;

			IsMonsterEnabled = true;
		}
	}
}
