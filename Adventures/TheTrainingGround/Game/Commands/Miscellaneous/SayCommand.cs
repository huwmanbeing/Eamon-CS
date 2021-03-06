﻿
// SayCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static TheTrainingGround.Game.Plugin.PluginContext;

namespace TheTrainingGround.Game.Commands
{
	[ClassMappings]
	public class SayCommand : EamonRT.Game.Commands.SayCommand, ISayCommand
	{
		public override void PlayerProcessEvents(long eventType)
		{
			if (eventType == PpeAfterPlayerSay)
			{
				var hammerArtifact = gADB[24];

				Debug.Assert(hammerArtifact != null);

				var magicWordsSpoken = string.Equals(ProcessedPhrase, "thor", StringComparison.OrdinalIgnoreCase) || string.Equals(ProcessedPhrase, "thor's hammer", StringComparison.OrdinalIgnoreCase);

				var hammerPresent = hammerArtifact.IsCarriedByCharacter() || hammerArtifact.IsInRoom(gActorRoom);

				// Hammer of Thor

				if (magicWordsSpoken && hammerPresent)
				{
					var command = Globals.CreateInstance<IUseCommand>();

					CopyCommandData(command);

					command.Dobj = hammerArtifact;

					NextState = command;

					GotoCleanup = true;
				}
				else
				{
					base.PlayerProcessEvents(eventType);
				}
			}
			else
			{
				base.PlayerProcessEvents(eventType);
			}
		}
	}
}
