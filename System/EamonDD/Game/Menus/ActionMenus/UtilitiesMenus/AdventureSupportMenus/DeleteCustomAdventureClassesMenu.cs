﻿
// DeleteCustomAdventureClassesMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class DeleteCustomAdventureClassesMenu : AdventureSupportMenu01, IDeleteCustomAdventureClassesMenu
	{
		/// <summary></summary>
		protected virtual void SelectClassFilesToDelete()
		{
			var invalidClassFileNames = new string[] { "Program.cs", "Engine.cs", "IPluginClassMappings.cs", "IPluginConstants.cs", "IPluginGlobals.cs", "PluginClassMappings.cs", "PluginConstants.cs", "PluginContext.cs", "PluginGlobals.cs" };

			SelectedClassFiles = new List<string>();

			var classFileName = string.Empty;

			while (true)
			{
				gOut.Print("{0}", Globals.LineSep);

				gOut.Write("{0}Enter file name of interface/class: ", Environment.NewLine);

				Buf.Clear();

				gOut.WordWrap = false;

				var rc = Globals.In.ReadField(Buf, 120, null, '_', '\0', true, null, null, null, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				gOut.WordWrap = true;

				classFileName = Buf.Trim().ToString().Replace('/', '\\');

				if (classFileName.Length == 0)
				{
					goto Cleanup;
				}

				if (!classFileName.StartsWith(@".\"))
				{
					classFileName = string.Empty;
				}
				else if (!classFileName.Contains(@"\Game\") && !classFileName.Contains(@"\Framework\"))
				{
					classFileName = string.Empty;
				}
				else if (!classFileName.EndsWith(".cs") || classFileName.Contains(@"\.") || invalidClassFileNames.FirstOrDefault(fn => string.Equals(fn, Globals.Path.GetFileName(classFileName), StringComparison.OrdinalIgnoreCase)) != null || SelectedClassFiles.FirstOrDefault(fn => string.Equals(fn, classFileName, StringComparison.OrdinalIgnoreCase)) != null || !Globals.File.Exists(classFileName))
				{
					classFileName = string.Empty;
				}

				gOut.Print("{0}", Globals.LineSep);

				if (classFileName.Length > 0)
				{
					SelectedClassFiles.Add(classFileName);

					gOut.Print("The file name path was added to the selected class files list.");
				}
				else
				{ 
					gOut.Print("The file name path was invalid or the file was not found.");
				}
			}

		Cleanup:

			if (SelectedClassFiles.Count == 0)
			{
				gOut.Print("{0}", Globals.LineSep);

				gOut.Print("The adventure was not processed.");

				GotoCleanup = true;
			}
		}

		/// <summary></summary>
		protected virtual void DeleteSelectedClassFiles()
		{
			gOut.Print("{0}", Globals.LineSep);

			gOut.WriteLine();

			foreach (var selectedClassFile in SelectedClassFiles)
			{
				if (Globals.File.Exists(selectedClassFile))
				{
					Globals.File.Delete(selectedClassFile);
				}

				if (selectedClassFile.Contains(@"\Game\"))
				{
					var selectedInterfaceFile = Globals.Path.GetDirectoryName(selectedClassFile.Replace(@"\Game\", @"\Framework\")) + @"\I" + Globals.Path.GetFileName(selectedClassFile);

					if (Globals.File.Exists(selectedInterfaceFile))
					{
						Globals.File.Delete(selectedInterfaceFile);
					}
				}
			}

			if (IsAdventureNameValid())
			{
				Globals.Directory.DeleteEmptySubdirectories(@"..\" + AdventureName, true);
			}
		}

		public override void Execute()
		{
			gOut.WriteLine();

			gEngine.PrintTitle("DELETE CUSTOM ADVENTURE CLASSES", true);

			Debug.Assert(!gEngine.IsAdventureFilesetLoaded());

			GotoCleanup = false;

			var workDir = Globals.Directory.GetCurrentDirectory();

			CheckForPrerequisites();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			GetAdventureName();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			GetAuthorName();

			GetAuthorInitials();

			if (IsAdventureNameValid())
			{
				Globals.Directory.SetCurrentDirectory(Constants.AdventuresDir + @"\" + AdventureName);

				SelectClassFilesToDelete();

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				QueryToProcessAdventure();

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				DeleteSelectedClassFiles();

				UpdateXmlFileClasses();

				Globals.Directory.SetCurrentDirectory(workDir);
			}

			DeleteAdvBinaryFiles();

			RebuildSolution();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			PrintAdventureProcessed();

		Cleanup:

			if (GotoCleanup)
			{
				// TODO: rollback classes delete if possible
			}

			Globals.Directory.SetCurrentDirectory(workDir);
		}

		public DeleteCustomAdventureClassesMenu()
		{

		}
	}
}
