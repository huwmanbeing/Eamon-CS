﻿
// OpenCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings]
	public class OpenCommand : EamonRT.Game.Commands.OpenCommand, EamonRT.Framework.Commands.IOpenCommand
	{
		protected override void PlayerProcessEvents()
		{
			// Try to open running device, all flee

			if (DobjArtifact.Uid == 44)
			{
				Globals.DeviceOpened = true;

				GotoCleanup = true;
			}
			else
			{
				base.PlayerProcessEvents();
			}
		}

		protected override void PrintOpened(Eamon.Framework.IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			// Large green device

			if (artifact.Uid == 44)
			{
				Globals.Out.Print("You try to open the glowing device.");
			}
			else
			{
				base.PrintOpened(artifact);
			}
		}

		protected override void PrintLocked(Eamon.Framework.IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			// Swallower shark

			if (artifact.Uid == 31)
			{
				Globals.Out.Print("The hide is too hard to cut!");

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();
			}
			else
			{
				base.PrintLocked(artifact);
			}
		}

		protected override void PrintOpenObjWithKey(Eamon.Framework.IArtifact artifact, Eamon.Framework.IArtifact key)
		{
			Debug.Assert(artifact != null && key != null);

			// Large green device

			if (artifact.Uid == 44)
			{
				Globals.Out.Print("You try to open the glowing device with {0}.", key.GetDecoratedName03(false, true, false, false, Globals.Buf));
			}
			else
			{
				base.PrintOpenObjWithKey(artifact, key);
			}
		}

		protected override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			// Large rock

			if (DobjArtifact.Uid == 17)
			{
				PrintCantVerbObj(DobjArtifact);

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();
			}
			else
			{
				base.PlayerExecute();
			}
		}
	}
}
