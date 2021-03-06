﻿
// GetCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static TheTrainingGround.Game.Plugin.PluginContext;

namespace TheTrainingGround.Game.Commands
{
	[ClassMappings]
	public class GetCommand : EamonRT.Game.Commands.GetCommand, IGetCommand
	{
		public virtual bool RevealSecretPassage { get; set; }

		public override void PrintTaken(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			base.PrintTaken(artifact);

			// Taking Purple book reveals secret passage

			if (artifact.Uid == 27 && gActorMonster.Uid == gGameState.Cm && gActorRoom.Uid == 24 && !gGameState.LibrarySecretPassageFound)
			{
				RevealSecretPassage = true;

				gGameState.LibrarySecretPassageFound = true;
			}
		}

		public override void PlayerExecute()
		{
			base.PlayerExecute();

			if (RevealSecretPassage)
			{
				gEngine.PrintEffectDesc(12);

				gActorRoom.SetDirs(Direction.East, 25);
			}
		}
	}
}
