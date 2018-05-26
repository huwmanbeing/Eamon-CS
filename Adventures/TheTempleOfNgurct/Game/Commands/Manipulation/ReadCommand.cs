﻿
// ReadCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.Commands
{
	[ClassMappings]
	public class ReadCommand : EamonRT.Game.Commands.ReadCommand, IReadCommand
	{
		protected override void PrintCantVerbHere()
		{
			PrintEnemiesNearby();
		}

		protected override void PlayerProcessEvents(long eventType)
		{
			// Book

			if (eventType == PpeAfterArtifactRead && DobjArtifact.Uid == 61)
			{
				Globals.Engine.RemoveWeight(DobjArtifact);

				DobjArtifact.SetInRoom(ActorRoom);

				ActorMonster.SetInRoomUid(58);

				Globals.Engine.CheckEnemies();

				NextState = Globals.CreateInstance<IStartState>();

				GotoCleanup = true;
			}
			else
			{
				base.PlayerProcessEvents(eventType);
			}
		}

		protected override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			// Brown potion

			if (DobjArtifact.Uid == 51)
			{
				Globals.Engine.PrintEffectDesc(1);
			}

			// Yellow potion

			else if (DobjArtifact.Uid == 53)
			{
				Globals.Engine.PrintEffectDesc(2);
			}

			// Red/black potion, fireball wand

			else if (DobjArtifact.Uid == 52 || DobjArtifact.Uid == 62 || DobjArtifact.Uid == 63)
			{
				Globals.Engine.PrintEffectDesc(3);
			}

			// Wine

			else if (DobjArtifact.Uid == 69)
			{
				Globals.Engine.PrintEffectDesc(4);
			}

			// Ring

			else if (DobjArtifact.Uid == 64)
			{
				Globals.Engine.PrintEffectDesc(5);
			}
			else
			{
				base.PlayerExecute();
			}

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		protected override bool IsAllowedInRoom()
		{
			return Globals.GameState.GetNBTL(Enums.Friendliness.Enemy) <= 0;
		}

		public ReadCommand()
		{
			IsPlayerEnabled = true;

			IsMonsterEnabled = true;
		}
	}
}
