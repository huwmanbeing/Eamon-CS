﻿
// UseCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Framework.Commands;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class UseCommand : Command, IUseCommand
	{
		public virtual bool IobjSupport { get; set; }

		protected virtual void PlayerProcessEvents()
		{

		}

		protected override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			PlayerProcessEvents();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			var artClasses = new Enums.ArtifactType[] { Enums.ArtifactType.Weapon, Enums.ArtifactType.MagicWeapon, Enums.ArtifactType.DisguisedMonster, Enums.ArtifactType.Drinkable, Enums.ArtifactType.Edible, Enums.ArtifactType.Wearable };

			var ac = DobjArtifact.GetArtifactClass(artClasses, false);

			if (ac != null)
			{
				if (ac.IsWeapon01())
				{
					NextState = Globals.CreateInstance<IReadyCommand>();

					CopyCommandData(NextState as ICommand);

					goto Cleanup;
				}

				if (ac.Type == Enums.ArtifactType.DisguisedMonster)
				{
					if (!DobjArtifact.IsUnmovable() && !DobjArtifact.IsCarriedByCharacter())
					{
						PrintTakingFirst(DobjArtifact);
					}

					Globals.RtEngine.RevealDisguisedMonster(DobjArtifact);

					NextState = Globals.CreateInstance<IMonsterStartState>();

					goto Cleanup;
				}

				if (ac.Type == Enums.ArtifactType.Drinkable)
				{
					NextState = Globals.CreateInstance<IDrinkCommand>();

					CopyCommandData(NextState as ICommand);

					goto Cleanup;
				}

				if (ac.Type == Enums.ArtifactType.Edible)
				{
					NextState = Globals.CreateInstance<IEatCommand>();

					CopyCommandData(NextState as ICommand);

					goto Cleanup;
				}

				Debug.Assert(ac.Type == Enums.ArtifactType.Wearable);

				NextState = Globals.CreateInstance<IWearCommand>();

				CopyCommandData(NextState as ICommand);

				goto Cleanup;
			}
			else
			{
				PrintTryDifferentCommand(DobjArtifact);
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		protected override void PlayerFinishParsing()
		{
			PlayerResolveArtifact();

			if (DobjArtifact != null && IobjSupport && CommandParser.CurrToken < CommandParser.Tokens.Length)
			{
				CommandParser.ObjData = CommandParser.IobjData;

				CommandParser.ObjData.QueryDesc = string.Format("{0}Use {1} on who or what? ", Environment.NewLine, DobjArtifact.EvalPlural("it", "them"));

				CommandParser.ObjData.ArtifactMatchFunc = PlayerArtifactMatch01;

				CommandParser.ObjData.MonsterMatchFunc = PlayerMonsterMatch02;

				CommandParser.ObjData.MonsterNotFoundFunc = PrintDontHaveItNotHere;

				PlayerResolveArtifact();
			}
		}

		public UseCommand()
		{
			SortOrder = 230;

			if (Globals.IsClassicVersion(5))
			{
				IsPlayerEnabled = false;

				IsMonsterEnabled = false;
			}

			Name = "UseCommand";

			Verb = "use";

			Type = Enums.CommandType.Manipulation;
		}
	}
}
