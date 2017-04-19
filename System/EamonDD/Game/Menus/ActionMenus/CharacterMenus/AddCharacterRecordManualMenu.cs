﻿
// AddCharacterRecordManualMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AddCharacterRecordManualMenu : AddRecordManualMenu<ICharacter>, IAddCharacterRecordManualMenu
	{
		public override void UpdateGlobals()
		{
			Globals.CharactersModified = true;
		}

		public AddCharacterRecordManualMenu()
		{
			Title = "ADD CHARACTER RECORD";

			RecordTable = Globals.Database.CharacterTable;

			RecordTypeName = "character";
		}
	}
}
