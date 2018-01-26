﻿
// ListArtifactRecordDetailMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class ListArtifactRecordDetailMenu : ListRecordDetailMenu<IArtifact>, IListArtifactRecordDetailMenu
	{
		public ListArtifactRecordDetailMenu()
		{
			Title = "LIST ARTIFACT RECORD DETAILS";

			RecordTable = Globals.Database.ArtifactTable;

			RecordTypeName = "artifact";
		}
	}
}
