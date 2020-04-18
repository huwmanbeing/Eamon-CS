
// SearchCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings]
	public class SearchCommand : EamonRT.Game.Commands.Command, Framework.Commands.ISearchCommand
	{
		public virtual void PrintNothingFound(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("You find nothing new.");
		}

		public override void PlayerExecute()
		{
			gOut.Print("Okay, you're doing a thorough search now...");

			gGameState.Minute += 20;

			if (gDobjArtifact != null)
			{
				// TODO
			}
			else
			{
				// TODO
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public override void PlayerFinishParsing()
		{
			if (gCommandParser.CurrToken < gCommandParser.Tokens.Length)
			{
				gCommandParser.ParseName();

				if (!string.Equals(gCommandParser.ObjData.Name, "room", StringComparison.OrdinalIgnoreCase) && !string.Equals(gCommandParser.ObjData.Name, "area", StringComparison.OrdinalIgnoreCase))
				{
					PlayerResolveArtifact();
				}
			}
		}

		public SearchCommand()
		{
			SortOrder = 440;

			IsNew = true;

			IsMonsterEnabled = false;

			Name = "SearchCommand";

			Verb = "search";

			Type = CommandType.Interactive;
		}
	}
}
