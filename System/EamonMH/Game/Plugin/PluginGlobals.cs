﻿
// PluginGlobals.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Text;
using Eamon.Framework;
using Eamon.Framework.Menus;
using EamonMH.Framework;
using EamonMH.Framework.Menus;
using EamonMH.Framework.Plugin;
using static EamonMH.Game.Plugin.PluginContext;

namespace EamonMH.Game.Plugin
{
	public class PluginGlobals : Eamon.Game.Plugin.PluginGlobals, IPluginGlobals
	{
		public virtual string[] Argv { get; set; }

		public virtual StringBuilder Buf { get; set; } = new StringBuilder(Constants.BufSize);

		public virtual long WordWrapCurrColumn { get; set; }

		public virtual char WordWrapLastChar { get; set; }

		public virtual string ConfigFileName { get; set; }

		public virtual string CharacterName { get; set; }

		public virtual IMhEngine MhEngine { get; set; }

		public virtual IMhMenu MhMenu { get; set; }

		public virtual IMenu Menu { get; set; }

		public virtual IFileset Fileset { get; set; }

		public virtual ICharacter Character { get; set; }

		public virtual IConfig Config { get; set; }

		public virtual bool GoOnAdventure { get; set; }

		public virtual bool ConfigsModified { get; set; }

		public virtual bool FilesetsModified { get; set; }

		public virtual bool CharactersModified { get; set; }

		public virtual bool EffectsModified { get; set; }

		public override void InitSystem()
		{
			base.InitSystem();

			ConfigFileName = "";

			CharacterName = "";

			MhEngine = CreateInstance<IMhEngine>();

			MhMenu = CreateInstance<IMhMenu>();

			Config = CreateInstance<IConfig>();
		}
	}
}
