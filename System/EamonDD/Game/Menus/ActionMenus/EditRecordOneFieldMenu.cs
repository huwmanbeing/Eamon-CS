﻿
// EditRecordOneFieldMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Helpers.Generic;
using Eamon.Game.Extensions;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	public abstract class EditRecordOneFieldMenu<T, U> : RecordMenu<T>, IEditRecordOneFieldMenu<T> where T : class, IGameBase where U : class, IHelper<T>
	{
		public virtual T EditRecord { get; set; }

		public virtual string EditFieldName { get; set; }

		public override void Execute()
		{
			RetCode rc;

			Globals.Out.WriteLine();

			Globals.Engine.PrintTitle(Title, true);

			if (EditRecord == null)
			{
				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(55, '\0', 0, string.Format("Enter the uid of the {0} record to edit", RecordTypeName), "1"));

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				var recordUid = Convert.ToInt64(Buf.Trim().ToString());

				Globals.Out.Print("{0}", Globals.LineSep);

				EditRecord = RecordTable.FindRecord(recordUid);

				if (EditRecord == null)
				{
					Globals.Out.Print("{0} record not found.", RecordTypeName.FirstCharToUpper());

					goto Cleanup;
				}
			}

			var editRecord01 = Globals.CloneInstance(EditRecord);

			Debug.Assert(editRecord01 != null);
			
			var helper = Globals.CreateInstance<U>(x =>
			{
				x.Record = editRecord01;
			});
			
			string editFieldName01 = null;

			if (string.IsNullOrWhiteSpace(EditFieldName))
			{
				helper.ListRecord(true, true, false, true, true, true);

				PrintPostListLineSep();

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(47, '\0', 0, "Enter the number of the field to edit", "0"));

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				var fieldNum = Convert.ToInt64(Buf.Trim().ToString());

				editFieldName01 = helper.GetFieldName(fieldNum);

				if (string.IsNullOrWhiteSpace(editFieldName01))
				{
					goto Cleanup;
				}

				Globals.Out.Print("{0}", Globals.LineSep);
			}
			else
			{
				editFieldName01 = EditFieldName;
			}

			helper.EditRec = true;
			helper.EditField = true;
			helper.FieldDesc = Globals.Config.FieldDesc;

			helper.InputField(editFieldName01);

			Globals.Thread.Sleep(150);

			if (!Globals.CompareInstances(EditRecord, editRecord01))
			{
				Globals.Out.Write("{0}Would you like to save this updated {1} record (Y/N): ", Environment.NewLine, RecordTypeName);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, Globals.Engine.IsCharYOrN);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Thread.Sleep(150);

				if (Buf.Length > 0 && Buf[0] == 'N')
				{
					goto Cleanup;
				}

				var character = editRecord01 as ICharacter;

				if (character != null)
				{
					character.StripPoundCharsFromWeaponNames();

					character.AddPoundCharsToWeaponNames();
				}

				var artifact = editRecord01 as IArtifact;

				if (artifact != null)
				{
					rc = artifact.SyncArtifactCategories();

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Globals.Engine.TruncatePluralTypeEffectDesc(artifact.PluralType, Constants.ArtNameLen);
				}

				var effect = editRecord01 as IEffect;

				if (effect != null)
				{
					Globals.Engine.TruncatePluralTypeEffectDesc(effect);
				}

				var monster = editRecord01 as IMonster;

				if (monster != null)
				{
					Globals.Engine.TruncatePluralTypeEffectDesc(monster.PluralType, Constants.MonNameLen);
				}

				var record = RecordTable.RemoveRecord(EditRecord.Uid);

				Debug.Assert(record != null);

				rc = RecordTable.AddRecord(editRecord01);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				UpdateGlobals();
			}
			else
			{
				Globals.Out.Print("{0} record not modified.", RecordTypeName.FirstCharToUpper());
			}

		Cleanup:

			EditRecord = null;

			EditFieldName = null;
		}
	}
}
