﻿
// EditConfigRecordMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Game.Menus;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	public abstract class EditConfigRecordMenu : Menu, IEditConfigRecordMenu
	{
		public virtual IConfig EditRecord { get; set; }

		public virtual void CompareAndSave(IConfig editConfig01)
		{
			RetCode rc;

			Debug.Assert(editConfig01 != null);

			Globals.Thread.Sleep(150);

			if (!Globals.CompareInstances(EditRecord, editConfig01))
			{
				Globals.Out.Write("{0}Would you like to save this updated config record (Y/N): ", Environment.NewLine);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, Globals.Engine.IsCharYOrN);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Thread.Sleep(150);

				if (Buf.Length > 0 && Buf[0] == 'N')
				{
					goto Cleanup;
				}

				var config = Globals.Database.RemoveConfig(EditRecord.Uid);

				if (config != null)
				{
					rc = Globals.Database.AddConfig(editConfig01);

					Debug.Assert(Globals.Engine.IsSuccess(rc));
				}

				if (Globals.Config == EditRecord)
				{
					Globals.Config = editConfig01;
				}

				Globals.ConfigsModified = true;
			}
			else
			{
				Globals.Out.WriteLine("{0}Config record not modified.", Environment.NewLine);
			}

		Cleanup:

			;
		}
	}
}
