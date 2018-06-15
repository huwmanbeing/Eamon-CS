﻿
// SearchCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.Commands
{
	[ClassMappings]
	public class SearchCommand : EamonRT.Game.Commands.Command, Framework.Commands.ISearchCommand
	{
		protected virtual void PrintNothingFound(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("Searching {0} reveals nothing of interest.", artifact.GetDecoratedName03(false, true, false, false, Globals.Buf));
		}

		public override void PrintCantVerbObj(IGameBase obj)
		{
			Debug.Assert(obj != null);

			Globals.Out.Print("You can only {0} dead bodies.", Verb);
		}

		public override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			switch (DobjArtifact.Uid)
			{
				case 89:

					// Dismantled worker android

					var plasticCardArtifact = Globals.ADB[82];

					Debug.Assert(plasticCardArtifact != null);

					if (plasticCardArtifact.IsInLimbo())
					{
						Globals.Out.Print("{0}", plasticCardArtifact.Desc);

						plasticCardArtifact.SetInRoom(ActorRoom);

						plasticCardArtifact.Seen = true;

						var command = Globals.CreateInstance<IGetCommand>();

						CopyCommandData(command);

						command.Dobj = plasticCardArtifact;

						NextState = command;
					}
					else
					{
						PrintNothingFound(DobjArtifact);
					}

					goto Cleanup;

				default:

					if (DobjArtifact.IsDeadBody() && DobjArtifact.Uid != 107)
					{
						PrintNothingFound(DobjArtifact);
					}
					else
					{ 
						PrintCantVerbObj(DobjArtifact);

						NextState = Globals.CreateInstance<IStartState>();
					}

					goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public override void PlayerFinishParsing()
		{
			PlayerResolveArtifact();
		}

		public SearchCommand()
		{
			SortOrder = 460;

			IsNew = true;

			IsMonsterEnabled = false;

			Name = "SearchCommand";

			Verb = "search";

			Type = Enums.CommandType.Interactive;
		}
	}
}
