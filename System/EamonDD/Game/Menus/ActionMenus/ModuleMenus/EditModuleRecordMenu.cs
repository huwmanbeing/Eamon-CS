﻿
// EditModuleRecordMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Game.Menus;
using Eamon.Game.Utilities;
using EamonDD.Framework.Menus.ActionMenus;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	public abstract class EditModuleRecordMenu : Menu, IEditModuleRecordMenu
	{
		public virtual IModule EditRecord { get; set; }

		public virtual void CompareAndSave(IModule editModule01)
		{
			RetCode rc;

			Debug.Assert(editModule01 != null);

			Globals.Thread.Sleep(150);

			if (!Globals.CompareInstances(EditRecord, editModule01))
			{
				Globals.Out.Write("{0}Would you like to save this updated module record (Y/N): ", Environment.NewLine);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, Globals.Engine.IsCharYOrN);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Thread.Sleep(150);

				if (Buf.Length > 0 && Buf[0] == 'N')
				{
					goto Cleanup;
				}

				if (EditRecord.NumDirs == 10 && editModule01.NumDirs == 6)
				{
					var directionValues = EnumUtil.GetValues<Enums.Direction>();

					foreach (var room in Globals.Database.RoomTable.Records)
					{
						for (var i = editModule01.NumDirs; i < EditRecord.NumDirs; i++)
						{
							var dv = directionValues[(int)i];

							room.SetDirs(dv, 0);
						}

						Globals.RoomsModified = true;
					}
				}

				var module = Globals.Database.RemoveModule(EditRecord.Uid);

				Debug.Assert(module != null);

				rc = Globals.Database.AddModule(editModule01);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				if (Globals.Module == EditRecord)
				{
					Globals.Module = editModule01;
				}

				Globals.ModulesModified = true;
			}
			else
			{
				Globals.Out.Print("Module record not modified.");
			}

		Cleanup:

			;
		}
	}
}
