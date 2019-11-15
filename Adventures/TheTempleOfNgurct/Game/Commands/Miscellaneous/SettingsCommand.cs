
// SettingsCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.Commands
{
	[ClassMappings(typeof(ISettingsCommand))]
	public class SettingsCommand : EamonRT.Game.Commands.SettingsCommand, Framework.Commands.ISettingsCommand
	{
		public virtual bool? ExplicitContent { get; set; }

		public override void PrintUsage()
		{
			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			base.PrintUsage();

			Globals.Out.WriteLine("  {0,-22}{1,-22}{2,-22}", "ExplicitContent", "True, False", gameState.ExplicitContent);
		}

		public override void PlayerExecute()
		{
			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			if (ExplicitContent == null)
			{
				base.PlayerExecute();

				goto Cleanup;
			}

			if (ExplicitContent != null)
			{
				gameState.ExplicitContent = (bool)ExplicitContent;
			}

			Globals.Out.Print("Settings changed.");

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IStartState>();
			}

		Cleanup:

			;
		}

		public override void PlayerFinishParsing()
		{
			bool boolValue = false;

			if (CommandParser.CurrToken + 1 < CommandParser.Tokens.Length)
			{
				if (string.Equals(CommandParser.Tokens[CommandParser.CurrToken], "explicitcontent", StringComparison.OrdinalIgnoreCase) && bool.TryParse(CommandParser.Tokens[CommandParser.CurrToken + 1], out boolValue))
				{
					ExplicitContent = boolValue;

					CommandParser.CurrToken += 2;
				}
				else
				{
					base.PlayerFinishParsing();
				}
			}
			else
			{
				base.PlayerFinishParsing();
			}
		}
	}
}
