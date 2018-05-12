﻿
// SayCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.Commands
{
	[ClassMappings]
	public class SayCommand : EamonRT.Game.Commands.SayCommand, ISayCommand
	{
		protected override void PlayerProcessEvents()
		{
			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			// Summon Alkanda

			if (string.Equals(ProcessedPhrase, "annal natthrac", StringComparison.OrdinalIgnoreCase))
			{
				var medallionArtifact = Globals.ADB[77];

				Debug.Assert(medallionArtifact != null);

				if (medallionArtifact.IsCarriedByCharacter() || medallionArtifact.IsInRoom(ActorRoom))
				{
					if (!gameState.AlkandaKilled)
					{
						var alkandaMonster = Globals.MDB[56];

						Debug.Assert(alkandaMonster != null);

						alkandaMonster.SetInRoom(ActorRoom);

						Globals.Engine.CheckEnemies();

						NextState = Globals.CreateInstance<IStartState>();
					}
				}
				else
				{
					Globals.Out.Print("You don't have the medallion of Ngurct!");

					NextState = Globals.CreateInstance<IStartState>();

					GotoCleanup = true;

					goto Cleanup;
				}
			}

			base.PlayerProcessEvents();

		Cleanup:

			;
		}
	}
}
