﻿
// ListEffectRecordMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class ListEffectRecordMenu : ListRecordDetailMenu<IEffect>, IListEffectRecordMenu
	{
		public override void PrintPostListLineSep()
		{
			Globals.Out.Print("{0}", Globals.LineSep);
		}

		public ListEffectRecordMenu()
		{
			Title = "LIST EFFECT RECORD DETAILS";

			RecordTable = Globals.Database.EffectTable;

			RecordTypeName = "effect";
		}
	}
}
