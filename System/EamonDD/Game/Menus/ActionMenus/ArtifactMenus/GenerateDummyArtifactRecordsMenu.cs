﻿
// GenerateDummyArtifactRecordsMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Menus;
using EamonDD.Framework.Menus.ActionMenus;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class GenerateDummyArtifactRecordsMenu : Menu, IGenerateDummyArtifactRecordsMenu
	{
		public override void Execute()
		{
			IArtifact artifact;
			RetCode rc;

			var artUids = new long[2];

			Globals.Out.WriteLine();

			Globals.Engine.PrintTitle("GENERATE DUMMY ARTIFACT RECORDS", true);

			Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(43, '\0', 0, "Enter the number to generate", "0"));

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsCharDigit, null);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			var j = Convert.ToInt64(Buf.Trim().ToString());

			for (var i = 0; i < j; i++)
			{
				artifact = Globals.CreateInstance<IArtifact>(x =>
				{
					x.Uid = Globals.Database.GetArtifactUid();
					x.Name = string.Format("artifact {0}", x.Uid);
					x.Desc = string.Format("You see artifact {0}.", x.Uid);
					x.GetClasses(0).Type = Enums.ArtifactType.Treasure;
				});

				if (i == 0)
				{
					artUids[0] = artifact.Uid;
				}

				if (i == j - 1)
				{
					artUids[1] = artifact.Uid;
				}

				rc = Globals.Database.AddArtifact(artifact);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.ArtifactsModified = true;

				if (Globals.Module != null)
				{
					Globals.Module.NumArtifacts++;

					Globals.ModulesModified = true;
				}
			}

			if (j > 0)
			{
				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

				Buf.SetFormat(j > 1 ? "Generated dummy artifacts with uids between {0} and {1}, inclusive." : "Generated a dummy artifact with uid {0}.", artUids[0], artUids[1]);

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Buf);
			}
		}

		public GenerateDummyArtifactRecordsMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
