﻿
// HintsCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using BeginnersForest.Framework.Commands;
using static BeginnersForest.Game.Plugin.PluginContext;

namespace BeginnersForest.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IHintsCommand))]
	public class HintsCommand : EamonRT.Game.Commands.HintsCommand, IHintsCommand
	{
		protected override void PrintHintsQuestion(IList<IHint> hints, int i)
		{
			Debug.Assert(hints != null);

			var prefix = "Beginner's Forest -- ";

			var question = hints[i].Question;

			if (question.StartsWith(prefix))
			{
				question = question.Substring(prefix.Length);
			}

			Globals.Out.Write("{0}{1,3}. {2}", Environment.NewLine, i + 1, question);
		}
	}
}
