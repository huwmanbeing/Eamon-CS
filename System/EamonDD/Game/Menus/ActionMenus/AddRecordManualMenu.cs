﻿
// AddRecordManualMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.DataEntry;
using Eamon.Game.Extensions;
using EamonDD.Framework.Menus.ActionMenus;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	public abstract class AddRecordManualMenu<T> : RecordMenu<T>, IAddRecordManualMenu<T> where T : class, IHaveUid
	{
		public virtual long NewRecordUid { get; set; }

		public override void Execute()
		{
			RetCode rc;

			T record;

			Globals.Out.WriteLine();

			Globals.Engine.PrintTitle(Title, true);

			if (!Globals.Config.GenerateUids && NewRecordUid == 0)
			{
				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(55, '\0', 0, string.Format("Enter the uid of the {0} record to add", RecordTypeName), null));

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', false, null, null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				NewRecordUid = Convert.ToInt64(Buf.Trim().ToString());

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

				if (NewRecordUid > 0)
				{
					record = RecordTable.FindRecord(NewRecordUid);

					if (record != null)
					{
						Globals.Out.WriteLine("{0}{1} record already exists.", Environment.NewLine, RecordTypeName.FirstCharToUpper());

						goto Cleanup;
					}

					RecordTable.FreeUids.Remove(NewRecordUid);
				}
			}

			record = Globals.CreateInstance<T>(x =>
			{
				x.Uid = NewRecordUid;
			});

			var editable = record as IEditable;

			Debug.Assert(editable != null);

			editable.InputRecord(false, Globals.Config.FieldDesc);

			Globals.Thread.Sleep(150);

			Globals.Out.Write("{0}Would you like to save this {1} record (Y/N): ", Environment.NewLine, RecordTypeName);

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, Globals.Engine.IsCharYOrN);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			Globals.Thread.Sleep(150);

			if (Buf.Length > 0 && Buf[0] == 'N')
			{
				record.Dispose();

				goto Cleanup;
			}

			var character = record as ICharacter;

			if (character != null)
			{
				character.StripPoundCharsFromWeaponNames();

				character.AddPoundCharsToWeaponNames();
			}

			var artifact = record as IArtifact;

			if (artifact != null)
			{
				var i = Globals.Engine.FindIndex(artifact.Classes, ac => ac != null && ac.Type == Enums.ArtifactType.None);
				
				if (i > 0)
				{
					rc = artifact.SetArtifactClassCount(i);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					artifact.FreeFields();
				}

				rc = artifact.SyncArtifactClasses();

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Engine.TruncatePluralTypeEffectDesc(artifact.PluralType, Constants.ArtNameLen);
			}

			var effect = record as IEffect;

			if (effect != null)
			{
				Globals.Engine.TruncatePluralTypeEffectDesc(effect);
			}

			var monster = record as IMonster;

			if (monster != null)
			{
				Globals.Engine.TruncatePluralTypeEffectDesc(monster.PluralType, Constants.MonNameLen);
			}

			rc = RecordTable.AddRecord(record);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			UpdateGlobals();

		Cleanup:

			NewRecordUid = 0;
		}
	}
}
