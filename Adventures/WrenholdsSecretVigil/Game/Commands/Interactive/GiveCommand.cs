﻿
// GiveCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using Eamon.Game.Attributes;
using WrenholdsSecretVigil.Framework.Commands;
using Enums = Eamon.Framework.Primitive.Enums;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IGiveCommand))]
	public class GiveCommand : EamonRT.Game.Commands.GiveCommand, IGiveCommand
	{
		protected override void PlayerProcessEvents()
		{
			if (IobjMonster.Uid == 1)
			{
				// Give death dog the dead rabbit

				if (DobjArtifact.Uid == 15)
				{
					Globals.Engine.RemoveWeight(DobjArtifact);

					DobjArtifact.SetInLimbo();

					IobjMonster.Friendliness = (Enums.Friendliness)150;

					IobjMonster.OrigFriendliness = (Enums.Friendliness)150;

					Globals.Engine.CheckEnemies();

					PrintGiveObjToActor(DobjArtifact, IobjMonster);

					Globals.Engine.PrintEffectDesc(13);

					if (IobjMonster.Friendliness == Enums.Friendliness.Friend)
					{
						Globals.Out.Write("{0}{1} barks once and wags its tail!{0}", Environment.NewLine, IobjMonster.GetDecoratedName03(true, true, false, false, Globals.Buf));
					}
				}
				else
				{
					Globals.Engine.MonsterSmiles(IobjMonster);

					Globals.Out.WriteLine();
				}

				GotoCleanup = true;
			}

			// Further disable bribing

			else if (base.MonsterRefusesToAccept())
			{
				Globals.Engine.MonsterSmiles(IobjMonster);

				Globals.Out.WriteLine();

				GotoCleanup = true;
			}
			else
			{
				base.PlayerProcessEvents();
			}
		}

		protected override void PlayerProcessEvents02()
		{
			// Disable bribing

			if (IobjMonster.Uid == 1 || IobjMonster.Friendliness < Enums.Friendliness.Friend)
			{
				Globals.Engine.MonsterSmiles(IobjMonster);

				Globals.Out.WriteLine();

				GotoCleanup = true;
			}
			else
			{
				base.PlayerProcessEvents02();
			}
		}

		protected override bool MonsterRefusesToAccept()
		{
			return false;
		}
	}
}
