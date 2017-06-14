﻿
// PluginGlobals.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using System.Text;
using Eamon.Framework;
using Eamon.Framework.Commands;
using Eamon.Framework.Parsing;
using Eamon.Framework.States;
using EamonRT.Framework;
using EamonRT.Framework.Plugin;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Plugin
{
	public class PluginGlobals : EamonDD.Game.Plugin.PluginGlobals, IPluginGlobals
	{
		public virtual StringBuilder Buf01 { get; set; } = new StringBuilder(Constants.BufSize);

		public virtual StringBuilder Buf02 { get; set; } = new StringBuilder(Constants.BufSize);

		public virtual IList<ICommand> CommandList { get; set; }

		public virtual IList<ICommand> LastCommandList { get; set; }

		public virtual long LoopMonsterUid { get; set; }

		public virtual IRtEngine RtEngine { get; set; }

		public virtual IIntroStory IntroStory { get; set; }

		public virtual IMainLoop MainLoop { get; set; }

		public virtual ICommandParser CommandParser { get; set; }

		public virtual IState CurrState { get; set; }

		public virtual IState NextState { get; set; }

		public virtual IGameState GameState { get; set; }

		public virtual ICharacter Character { get; set; }

		public virtual Enums.ExitType ExitType { get; set; }

		public virtual string CommandPrompt { get; set; }

		public virtual ICommand LastCommand
		{
			get
			{
				return LastCommandList.Count > 0 ? LastCommandList[LastCommandList.Count - 1] : null;
			}
		}

		public virtual bool GameRunning
		{
			get
			{
				return ExitType == Enums.ExitType.None;
			}
		}

		public virtual bool DeleteGameStateAfterLoop
		{
			get
			{
				return ExitType == Enums.ExitType.GoToMainHall || ExitType == Enums.ExitType.StartOver || ExitType == Enums.ExitType.FinishAdventure;
			}
		}

		public virtual bool StartOver
		{
			get
			{
				return ExitType == Enums.ExitType.StartOver;
			}
		}

		public virtual bool ErrorExit
		{
			get
			{
				return ExitType == Enums.ExitType.Error;
			}
		}

		public virtual bool ExportCharacterGoToMainHall
		{
			get
			{
				return ExitType == Enums.ExitType.GoToMainHall || ExitType == Enums.ExitType.FinishAdventure;
			}
		}

		public virtual bool ExportCharacter
		{
			get
			{
				return ExitType == Enums.ExitType.FinishAdventure;
			}
		}

		public override void InitSystem()
		{
			base.InitSystem();

			CommandList = new List<ICommand>();

			LastCommandList = new List<ICommand>();

			RtEngine = CreateInstance<IRtEngine>();

			IntroStory = CreateInstance<IIntroStory>();

			MainLoop = CreateInstance<IMainLoop>();

			CommandParser = CreateInstance<ICommandParser>();

			CommandPrompt = Constants.CommandPrompt;
		}
	}
}
