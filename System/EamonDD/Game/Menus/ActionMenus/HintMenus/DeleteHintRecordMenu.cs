﻿
// DeleteHintRecordMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class DeleteHintRecordMenu : DeleteRecordMenu<IHint>, IDeleteHintRecordMenu
	{
		public override void PrintPostListLineSep()
		{
			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		public override void UpdateGlobals()
		{
			Globals.HintsModified = true;

			if (Globals.Module != null)
			{
				Globals.Module.NumHints--;

				Globals.ModulesModified = true;
			}
		}

		public DeleteHintRecordMenu()
		{
			Title = "DELETE HINT RECORD";

			RecordTable = Globals.Database.HintTable;

			RecordTypeName = "hint";
		}
	}
}
