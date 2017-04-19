﻿
// RoomRecordMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using Eamon.Framework.Menus;
using Eamon.Game.Attributes;
using Eamon.Game.Menus;
using EamonDD.Framework.Menus.ActionMenus;
using EamonDD.Framework.Menus.HierarchicalMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.HierarchicalMenus
{
	[ClassMappings]
	public class RoomRecordMenu : Menu, IRoomRecordMenu
	{
		public override void PrintSubtitle()
		{
			Globals.DdMenu.PrintRoomMenuSubtitle();
		}

		public RoomRecordMenu()
		{
			Title = "ROOM RECORD MENU";

			Buf = Globals.Buf;

			MenuItems = new List<IMenuItem>();

			MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItems.Count);
				x.LineText = string.Format("{0}{1}. Add a room record.", Environment.NewLine, MenuItems.Count + 1);
				x.SubMenu = Globals.CreateInstance<IAddRoomRecordMenu>();
			}));

			MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItems.Count);
				x.LineText = string.Format("{0}{1}. Edit a room record.", Environment.NewLine, MenuItems.Count + 1);
				x.SubMenu = Globals.CreateInstance<IEditRoomRecordMenu>();
			}));

			MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItems.Count);
				x.LineText = string.Format("{0}{1}. Delete a room record.", Environment.NewLine, MenuItems.Count + 1);
				x.SubMenu = Globals.CreateInstance<IDeleteRoomRecordMenu>();
			}));

			MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItems.Count);
				x.LineText = string.Format("{0}{1}. List room records.", Environment.NewLine, MenuItems.Count + 1);
				x.SubMenu = Globals.CreateInstance<IListRoomRecordMenu>();
			}));

			MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItems.Count);
				x.LineText = string.Format("{0}{1}. Room record utilities.", Environment.NewLine, MenuItems.Count + 1);
				x.SubMenu = Globals.CreateInstance<IRoomRecordUtilitiesMenu>();
			}));

			MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = 'X';
				x.LineText = string.Format("{0}X. Exit.{0}", Environment.NewLine);
				x.SubMenu = null;
			}));
		}
	}
}
