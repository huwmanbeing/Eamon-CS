﻿
// UseCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using TheTrainingGround.Framework.Commands;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheTrainingGround.Game.Plugin.PluginContext;

namespace TheTrainingGround.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IUseCommand))]
	public class UseCommand : EamonRT.Game.Commands.UseCommand, IUseCommand
	{
		protected override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			// Hammer of Thor

			if (DobjArtifact.Uid == 24 && (DobjArtifact.IsCarriedByCharacter() || DobjArtifact.IsInRoom(ActorRoom)))
			{
				var monsters = Globals.Engine.GetMonsterList(() => true, m => m.IsInRoom(ActorRoom) && m.Friendliness == Enums.Friendliness.Enemy && m.Field1 == 0);

				foreach (var monster in monsters)
				{
					monster.Courage /= 4;

					monster.Field1 = 1;
				}

				Globals.RtEngine.CheckEnemies();

				Globals.Engine.PrintEffectDesc(32);

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IMonsterStartState>();
			}
			else
			{
				base.PlayerExecute();
			}
		}
	}
}
