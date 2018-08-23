﻿
// QuitCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class QuitCommand : Command, IQuitCommand
	{
		public virtual bool GoToMainHall { get; set; }

		public override void PlayerExecute()
		{
			RetCode rc;

			if (GoToMainHall)
			{
				Globals.Out.Write("{0}Return to the Main Hall (Y/N): ", Environment.NewLine);

				Globals.Buf.Clear();

				rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				if (Globals.Buf.Length > 0 && Globals.Buf[0] == 'Y')
				{
					Globals.GameState.Die = -1;

					Globals.ExitType = Enums.ExitType.GoToMainHall;

					Globals.MainLoop.ShouldShutdown = false;

					goto Cleanup;
				}
			}
			else
			{
				if (Globals.Database.GetFilesetsCount() == 0)
				{
					Globals.Out.Print("[You haven't saved a game yet but {0} will be left here should you choose to return.  Use \"quit hall\" if you don't want {1} to stay.]",
						ActorMonster.Name,
						ActorMonster.EvalGender("him", "her", "it"));
				}

				Globals.Out.Write("{0}Do you really want to quit (Y/N): ", Environment.NewLine);

				Globals.Buf.Clear();

				rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				if (Globals.Buf.Length > 0 && Globals.Buf[0] == 'Y')
				{
					Globals.ExitType = Enums.ExitType.Quit;

					Globals.MainLoop.ShouldShutdown = false;

					goto Cleanup;
				}
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IStartState>();
			}
		}

		public override void PlayerFinishParsing()
		{
			if (CommandParser.CurrToken < CommandParser.Tokens.Length && string.Equals(CommandParser.Tokens[CommandParser.CurrToken], "hall", StringComparison.OrdinalIgnoreCase))
			{
				GoToMainHall = true;

				CommandParser.CurrToken++;
			}
		}

		public override bool ShouldPreTurnProcess()
		{
			return false;
		}

		public QuitCommand()
		{
			SortOrder = 430;

			IsDarkEnabled = true;

			IsMonsterEnabled = false;

			Name = "QuitCommand";

			Verb = "quit";

			Type = Enums.CommandType.Miscellaneous;
		}
	}
}

/* EamonCsCodeTemplate

// QuitCommand.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static YourAdventureName.Game.Plugin.PluginContext;

namespace YourAdventureName.Game.Commands
{
	[ClassMappings]
	public class QuitCommand : EamonRT.Game.Commands.QuitCommand, IQuitCommand
	{

	}
}
EamonCsCodeTemplate */
