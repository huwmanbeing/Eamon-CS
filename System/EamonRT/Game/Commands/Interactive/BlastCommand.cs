﻿
// BlastCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Commands;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class BlastCommand : Command, IBlastCommand
	{
		public virtual bool CastSpell { get; set; }

		public virtual bool CheckAttack { get; set; }

		protected virtual void PlayerProcessEvents()
		{

		}

		protected virtual void PlayerProcessEvents01()
		{

		}

		protected virtual bool AllowSkillIncrease()
		{
			return DobjMonster != null || DobjArtifact.IsAttackable();
		}

		protected override void PlayerExecute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			if (!CheckAttack && DobjMonster != null && DobjMonster.Friendliness != Enums.Friendliness.Enemy)
			{
				Globals.Out.Write("{0}Attack non-enemy (Y/N): ", Environment.NewLine);

				Globals.Buf.Clear();

				rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				if (Globals.Buf.Length == 0 || Globals.Buf[0] == 'N')
				{
					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				CheckAttack = true;
			}

			if (CastSpell && !Globals.RtEngine.CheckPlayerSpellCast(Enums.Spell.Blast, AllowSkillIncrease()))
			{
				goto Cleanup;
			}

			PlayerProcessEvents();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			if (DobjMonster != null && DobjMonster.Friendliness != Enums.Friendliness.Enemy)
			{
				Globals.RtEngine.MonsterGetsAggravated(DobjMonster);
			}

			PlayerProcessEvents01();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			NextState = Globals.CreateInstance<IAttackCommand>(x =>
			{
				x.BlastSpell = true;

				x.CheckAttack = CheckAttack;
			});

			CopyCommandData(NextState as ICommand);

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		protected override void PlayerFinishParsing()
		{
			CommandParser.ObjData.MonsterMatchFunc = PlayerMonsterMatch03;

			CommandParser.ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
			{
				a => a.IsInRoom(ActorRoom),
				a => a.IsEmbeddedInRoom(ActorRoom)
			};

			CommandParser.ObjData.ArtifactNotFoundFunc = PrintNobodyHereByThatName;

			PlayerResolveMonster();
		}

		public BlastCommand()
		{
			SortOrder = 260;

			Name = "BlastCommand";

			Verb = "blast";

			Type = Enums.CommandType.Interactive;

			CastSpell = true;
		}
	}
}
