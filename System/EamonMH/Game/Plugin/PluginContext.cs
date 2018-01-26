﻿
// PluginContext.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using EamonMH.Framework.Plugin;

namespace EamonMH.Game.Plugin
{
	public static class PluginContext
	{
		public static IPluginConstants Constants
		{
			get
			{
				return (IPluginConstants)Eamon.Game.Plugin.PluginContext.Constants;
			}
			set
			{
				Eamon.Game.Plugin.PluginContext.Constants = value;
			}
		}

		public static IPluginClassMappings ClassMappings
		{
			get
			{
				return (IPluginClassMappings)Eamon.Game.Plugin.PluginContext.ClassMappings;
			}
			set
			{
				Eamon.Game.Plugin.PluginContext.ClassMappings = value;
			}
		}

		public static IPluginGlobals Globals
		{
			get
			{
				return (IPluginGlobals)Eamon.Game.Plugin.PluginContext.Globals;
			}
			set
			{
				Eamon.Game.Plugin.PluginContext.Globals = value;
			}
		}
	}
}
