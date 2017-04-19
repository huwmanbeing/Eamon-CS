﻿
// DeleteMonsterRecordMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class DeleteMonsterRecordMenu : DeleteRecordMenu<IMonster>, IDeleteMonsterRecordMenu
	{
		public override void UpdateGlobals()
		{
			Globals.MonstersModified = true;

			if (Globals.Module != null)
			{
				Globals.Module.NumMonsters--;

				Globals.ModulesModified = true;
			}
		}

		public DeleteMonsterRecordMenu()
		{
			Title = "DELETE MONSTER RECORD";

			RecordTable = Globals.Database.MonsterTable;

			RecordTypeName = "monster";
		}
	}
}
