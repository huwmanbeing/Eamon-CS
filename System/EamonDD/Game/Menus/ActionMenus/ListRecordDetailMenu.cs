﻿
// ListRecordDetailMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Helpers.Generic;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	public abstract class ListRecordDetailMenu<T, U> : RecordMenu<T>, IListRecordDetailMenu<T> where T : class, IGameBase where U : class, IHelper<T>
	{
		public override void Execute()
		{
			RetCode rc;

			var buf01 = new StringBuilder(Constants.BufSize);

			var recUids = new long[2];

			Globals.Out.WriteLine();

			Globals.Engine.PrintTitle(Title, true);

			var maxRecUid = RecordTable.GetRecordUid(false);

			Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(43, '\0', 0, string.Format("Enter the starting {0} uid", RecordTypeName), "1"));

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsCharDigit, null);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			recUids[0] = Convert.ToInt64(Buf.Trim().ToString());

			Globals.Out.Print("{0}", Globals.LineSep);

			Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(43, '\0', 0, string.Format("Enter the ending {0} uid", RecordTypeName), maxRecUid > 0 ? maxRecUid.ToString() : "1"));

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, maxRecUid > 0 ? maxRecUid.ToString() : "1", null, Globals.Engine.IsCharDigit, null);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			recUids[1] = Convert.ToInt64(Buf.Trim().ToString());

			var helper = Globals.CreateInstance<U>();

			var records = RecordTable.Records.Where(x => x.Uid >= recUids[0] && x.Uid <= recUids[1]);

			foreach (var record in records)
			{
				helper.Record = record;

				Globals.Out.Print("{0}", Globals.LineSep);

				helper.ListRecord(true, Globals.Config.ShowDesc, Globals.Config.ResolveEffects, true, false, false);

				PrintPostListLineSep();

				Globals.Out.Write("{0}Press any key to continue or X to exit: ", Environment.NewLine);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', true, null, Globals.Engine.ModifyCharToNullOrX, null, Globals.Engine.IsCharAny);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				if (Buf.Length > 0 && Buf[0] == 'X')
				{
					break;
				}
			}

			Globals.Out.Print("{0}", Globals.LineSep);

			Globals.Out.Print("Done listing {0} record details.", RecordTypeName);
		}
	}
}
