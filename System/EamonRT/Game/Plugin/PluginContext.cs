﻿
// PluginContext.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using Eamon.Framework.DataStorage.Generic;
using Eamon.Framework.Portability;
using EamonRT.Framework;
using EamonRT.Framework.Parsing;
using EamonRT.Framework.Plugin;

namespace EamonRT.Game.Plugin
{
	public static class PluginContext
	{
		public static IPluginConstants Constants
		{
			get
			{
				return (IPluginConstants)EamonDD.Game.Plugin.PluginContext.Constants;
			}
			set
			{
				EamonDD.Game.Plugin.PluginContext.Constants = value;
			}
		}

		public static IPluginClassMappings ClassMappings
		{
			get
			{
				return (IPluginClassMappings)EamonDD.Game.Plugin.PluginContext.ClassMappings;
			}
			set
			{
				EamonDD.Game.Plugin.PluginContext.ClassMappings = value;
			}
		}

		public static IPluginGlobals Globals
		{
			get
			{
				return (IPluginGlobals)EamonDD.Game.Plugin.PluginContext.Globals;
			}
			set
			{
				EamonDD.Game.Plugin.PluginContext.Globals = value;
			}
		}

		public static ITextWriter gOut 
		{
			get 
			{
				return EamonDD.Game.Plugin.PluginContext.gOut;
			}
		}

		public static IEngine gEngine 
		{
			get 
			{
				return (IEngine)EamonDD.Game.Plugin.PluginContext.gEngine;
			}
		}

		public static IRecordDb<Eamon.Framework.IRoom> gRDB 
		{
			get 
			{
				return EamonDD.Game.Plugin.PluginContext.gRDB;
			}
		}

		public static IRecordDb<Eamon.Framework.IArtifact> gADB 
		{
			get 
			{
				return EamonDD.Game.Plugin.PluginContext.gADB;
			}
		}

		public static IRecordDb<Eamon.Framework.IEffect> gEDB 
		{
			get 
			{
				return EamonDD.Game.Plugin.PluginContext.gEDB;
			}
		}

		public static IRecordDb<Eamon.Framework.IMonster> gMDB 
		{
			get 
			{
				return EamonDD.Game.Plugin.PluginContext.gMDB;
			}
		}

		public static Eamon.Framework.IMonster gActorMonster
		{
			get
			{
				return Globals?.CommandParser?.NextCommand?.ActorMonster ?? Globals?.CurrCommand?.ActorMonster;
			}
		}

		public static Eamon.Framework.IRoom gActorRoom
		{
			get
			{
				return Globals?.CommandParser?.NextCommand?.ActorRoom ?? Globals?.CurrCommand?.ActorRoom;
			}
		}

		public static Eamon.Framework.IArtifact gDobjArtifact
		{
			get
			{
				return Globals?.CommandParser?.NextCommand?.DobjArtifact ?? Globals?.CurrCommand?.DobjArtifact;
			}
		}

		public static Eamon.Framework.IMonster gDobjMonster
		{
			get
			{
				return Globals?.CommandParser?.NextCommand?.DobjMonster ?? Globals?.CurrCommand?.DobjMonster;
			}
		}

		public static Eamon.Framework.IArtifact gIobjArtifact
		{
			get
			{
				return Globals?.CommandParser?.NextCommand?.IobjArtifact ?? Globals?.CurrCommand?.IobjArtifact;
			}
		}

		public static Eamon.Framework.IMonster gIobjMonster
		{
			get
			{
				return Globals?.CommandParser?.NextCommand?.IobjMonster ?? Globals?.CurrCommand?.IobjMonster;
			}
		}

		public static ICommandParser gCommandParser
		{
			get
			{
				return Globals?.CommandParser;
			}
		}

		public static Eamon.Framework.IGameState gGameState
		{
			get 
			{
				return Globals?.GameState;
			}
		}

		public static Eamon.Framework.ICharacter gCharacter
		{
			get
			{
				return Globals?.Character;
			}
		}
	}
}
