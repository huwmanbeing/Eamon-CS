﻿
// AdventureSupportMenu01.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Automation;
using Eamon.Game.Menus;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	public abstract class AdventureSupportMenu01 : Menu, IAdventureSupportMenu01
	{
		protected virtual bool GotoCleanup { get; set; }

		protected virtual string AdventureName { get; set; }

		protected virtual string AdventureName01 { get; set; }

		protected virtual string AuthorName { get; set; }

		protected virtual string AuthorInitials { get; set; }

		protected virtual string AdvTemplateDir { get; set; }

		protected virtual IList<string> SelectedAdvDbTextFiles { get; set; }

		protected virtual Assembly VsaAssembly { get; set; }

		protected virtual IVisualStudioAutomation VsaObject { get; set; }

		protected virtual string ReplaceMacros(string fileText)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(fileText));

			return fileText.Replace("YourAdventureName", AdventureName).Replace("YourAuthorName", AuthorName).Replace("YourAuthorInitials", AuthorInitials);
		}

		protected virtual void LoadVsaAssemblyIfNecessary()
		{
			if (VsaAssembly == null)
			{
				VsaAssembly = Assembly.LoadFrom(Globals.Path.GetFullPath(@".\EamonVS.dll"));
			}
		}

		protected virtual void GetVsaObjectIfNecessary()
		{
			if (VsaAssembly != null && VsaObject == null)
			{
				var type = VsaAssembly.GetType("EamonVS.VisualStudioAutomation");

				if (type != null)
				{
					VsaObject = (IVisualStudioAutomation)Activator.CreateInstance(type);

					if (VsaObject != null)
					{
						VsaObject.DevenvExePath = Globals.DevenvExePath;

						VsaObject.SolutionFile = Globals.Path.GetFullPath(Constants.EamonDesktopSlnFile);
					}
				}
			}
		}

		protected virtual void GetAdventureName()
		{
			Globals.Out.Print("You must enter a name for your new adventure (eg, The Beginner's Cave).  This should be the formal name of the adventure shown in the Main Hall's list of adventures.");

			Globals.Out.Print("Note:  the name will be used to produce a shortened form suitable for use as a folder name under the Adventures directory and also as a C# namespace (eg, TheBeginnersCave).");

			AdventureName = string.Empty;

			while (AdventureName.Length == 0)
			{
				Globals.Out.Write("{0}Enter the name of the new adventure: ", Environment.NewLine);

				Buf.Clear();

				var rc = Globals.In.ReadField(Buf, Constants.FsNameLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				AdventureName01 = Buf.Trim().ToString();

				var tempStr = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(AdventureName01);

				AdventureName = new string((from char ch in tempStr where Globals.Engine.IsCharAlnum(ch) select ch).ToArray());

				if (AdventureName.Length > 0 && Globals.Engine.IsCharDigit(AdventureName[0]))
				{
					AdventureName = string.Empty;
				}

				if (AdventureName.Length > Constants.FsFileNameLen - 4)
				{
					AdventureName = AdventureName.Substring(0, Constants.FsFileNameLen - 4);
				}

				if (AdventureName.Length == 0)
				{
					Globals.Out.Print("{0}", Globals.LineSep);
				}
			}

			if (Globals.Directory.Exists(Constants.AdventuresDir + @"\" + AdventureName))
			{
				Globals.Out.Print("{0}", Globals.LineSep);

				Globals.Out.Print("The adventure already exists.");

				GotoCleanup = true;
			}
		}

		protected virtual void GetAuthorName()
		{
			AuthorName = string.Empty;

			while (AuthorName.Length == 0)
			{
				Globals.Out.Print("{0}", Globals.LineSep);

				Globals.Out.Write("{0}Enter the name(s) of the adventure's Eamon CS author(s): ", Environment.NewLine);

				Buf.Clear();

				var rc = Globals.In.ReadField(Buf, Constants.ModAuthorLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				AuthorName = Buf.Trim().ToString();
			}
		}

		protected virtual void GetAuthorInitials()
		{
			Globals.Out.Print("{0}", Globals.LineSep);

			Globals.Out.Write("{0}Enter the initials of the adventure's main Eamon CS author: ", Environment.NewLine);

			Buf.Clear();

			var rc = Globals.In.ReadField(Buf, Constants.ModVolLabelLen - 4, null, '_', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharAlpha, null);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			AuthorInitials = Buf.Trim().ToString();
		}

		protected virtual void SelectAdvDbTextFiles()
		{
			RetCode rc;

			SelectedAdvDbTextFiles = new List<string>();

			var advDbTextFiles = new string[] { "ADVENTURES.XML", "FANTASY.XML", "SCIFI.XML", "CONTEMPORARY.XML", "TEST.XML", "WIP.XML" };

			var inputDefaultValue = "Y";

			foreach (var advDbTextFile in advDbTextFiles)
			{
				Globals.Out.Print("{0}", Globals.LineSep);

				Globals.Out.Write("{0}Add this game to adventure database \"{1}\" (Y/N) [{2}]: ", Environment.NewLine, advDbTextFile, inputDefaultValue);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', true, inputDefaultValue, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				if (Buf.Length == 0 || Buf[0] != 'N')
				{
					SelectedAdvDbTextFiles.Add(advDbTextFile);

					if (!string.Equals(advDbTextFile, "ADVENTURES.XML", StringComparison.OrdinalIgnoreCase))
					{
						inputDefaultValue = "N";
					}
				}
			}

			var customAdvDbTextFile = string.Empty;

			while (true)
			{
				Globals.Out.Print("{0}", Globals.LineSep);

				if (customAdvDbTextFile.Length == 0)
				{
					Globals.Out.Print("If you would like to add this adventure to one or more custom adventure databases, enter those file names now (eg, HORROR.XML).  To skip this step, or if you are done, just press enter.");
				}

				Globals.Out.Write("{0}Enter name of custom adventure database: ", Environment.NewLine);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.FsFileNameLen, null, '_', '\0', true, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharAlnumPeriodUnderscore, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				customAdvDbTextFile = Buf.Trim().ToString();

				if (customAdvDbTextFile.Length > 0)
				{
					if (!SelectedAdvDbTextFiles.Contains(customAdvDbTextFile))
					{
						SelectedAdvDbTextFiles.Add(customAdvDbTextFile);
					}
				}
				else
				{
					break;
				}
			}
		}

		protected virtual void QueryToAddAdventure()
		{
			RetCode rc;

			Globals.Out.Print("{0}", Globals.LineSep);

			Globals.Out.Write("{0}Would you like to add this adventure to Eamon CS (Y/N): ", Environment.NewLine);

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, null);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			if (Buf.Length == 0 || Buf[0] != 'Y')
			{
				Globals.Out.Print("{0}", Globals.LineSep);

				Globals.Out.Print("The adventure was not created.");

				GotoCleanup = true;
			}
		}

		protected virtual void CopyQuickLaunchFiles()
		{
			var fileText = Globals.File.ReadAllText(AdvTemplateDir + @"\QuickLaunch\Unix\EamonDD\EditYourAdventureName.sh");

			Globals.File.WriteAllText(Constants.QuickLaunchDir + @"\Unix\EamonDD\Edit" + AdventureName + ".sh", ReplaceMacros(fileText));

			fileText = Globals.File.ReadAllText(AdvTemplateDir + @"\QuickLaunch\Unix\EamonRT\ResumeYourAdventureName.sh");

			Globals.File.WriteAllText(Constants.QuickLaunchDir + @"\Unix\EamonRT\Resume" + AdventureName + ".sh", ReplaceMacros(fileText));

			fileText = Globals.File.ReadAllText(AdvTemplateDir + @"\QuickLaunch\Windows\EamonDD\EditYourAdventureName.bat");

			Globals.File.WriteAllText(Constants.QuickLaunchDir + @"\Windows\EamonDD\Edit" + AdventureName + ".bat", ReplaceMacros(fileText));

			fileText = Globals.File.ReadAllText(AdvTemplateDir + @"\QuickLaunch\Windows\EamonRT\ResumeYourAdventureName.bat");

			Globals.File.WriteAllText(Constants.QuickLaunchDir + @"\Windows\EamonRT\Resume" + AdventureName + ".bat", ReplaceMacros(fileText));
		}

		protected virtual void CreateAdventureFolder()
		{
			Globals.Directory.CreateDirectory(Constants.AdventuresDir + @"\" + AdventureName);
		}

		protected virtual void CopyHintsXml()
		{
			var fileText = Globals.File.ReadAllText(AdvTemplateDir + @"\Adventures\YourAdventureName\HINTS.XML");

			Globals.File.WriteAllText(Constants.AdventuresDir + @"\" + AdventureName + @"\HINTS.XML", ReplaceMacros(fileText));
		}

		protected virtual void UpdateAdvDbTextFiles()
		{
			RetCode rc;

			foreach (var advDbTextFile in SelectedAdvDbTextFiles)
			{
				rc = Globals.PushDatabase();

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				var fsfn = Globals.Path.Combine(".", advDbTextFile);

				rc = Globals.Database.LoadFilesets(fsfn, printOutput: false);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				var fileset = Globals.CreateInstance<IFileset>(x =>
				{
					x.Uid = Globals.Database.GetFilesetUid();

					x.IsUidRecycled = true;

					x.Name = AdventureName01;

					x.WorkDir = @"..\..\Adventures\" + AdventureName;

					x.PluginFileName = this is IAddStandardAdventureMenu ? "EamonRT.dll" : AdventureName + ".dll";

					x.ConfigFileName = "NONE";

					x.FilesetFileName = "NONE";

					x.CharacterFileName = "NONE";

					x.ModuleFileName = "MODULE.XML";

					x.RoomFileName = "ROOMS.XML";

					x.ArtifactFileName = "ARTIFACTS.XML";

					x.EffectFileName = "EFFECTS.XML";

					x.MonsterFileName = "MONSTERS.XML";

					x.HintFileName = "HINTS.XML";

					x.GameStateFileName = "NONE";
				});

				rc = Globals.Database.AddFileset(fileset);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				rc = Globals.Database.SaveFilesets(fsfn, printOutput: false);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				rc = Globals.PopDatabase();

				Debug.Assert(Globals.Engine.IsSuccess(rc));
			}
		}

		protected virtual void RebuildSolution()
		{
			LoadVsaAssemblyIfNecessary();

			GetVsaObjectIfNecessary();

			if (VsaObject != null)
			{
				VsaObject.RebuildSolution();

				VsaObject.Shutdown();
			}
			else
			{
				Globals.Out.Print("{0}", Globals.LineSep);

				Globals.Out.Print("The adventure was not created.");

				GotoCleanup = true;
			}
		}

		protected virtual void PrintAdventureCreated()
		{
			Globals.Out.Print("{0}", Globals.LineSep);

			Globals.Out.Print("The adventure was successfully created.");
		}

		public AdventureSupportMenu01()
		{
			Buf = Globals.Buf;

			AdvTemplateDir = this is IAddStandardAdventureMenu ? 
				Constants.AdventuresDir + @"\AdventureTemplates\Standard" : 
				Constants.AdventuresDir + @"\AdventureTemplates\Custom";
		}
	}
}
