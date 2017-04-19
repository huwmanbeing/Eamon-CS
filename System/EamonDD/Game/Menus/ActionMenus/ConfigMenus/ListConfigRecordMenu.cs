﻿
// ListConfigRecordMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Game.Attributes;
using Eamon.Game.Menus;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class ListConfigRecordMenu : Menu, IListConfigRecordMenu
	{
		public override void Execute()
		{
			RetCode rc;

			Globals.Out.WriteLine();

			Globals.Engine.PrintTitle("LIST CONFIG RECORD DETAILS", true);

			Globals.Config.ListRecord(true, false, false, false, false, false);

			Globals.Out.WriteLine();

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

			Globals.Out.Write("{0}Press any key to continue or X to exit: ", Environment.NewLine);

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', true, null, Globals.Engine.ModifyCharToNullOrX, null, Globals.Engine.IsCharAny);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			if (Buf.Length > 0 && Buf[0] == 'X')
			{
				// do nothing
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

			Globals.Out.WriteLine("{0}Done listing config record details.", Environment.NewLine);
		}

		public ListConfigRecordMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
