﻿
// ExamineCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using TheSubAquanLaboratory.Framework;
using TheSubAquanLaboratory.Framework.Commands;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IExamineCommand))]
	public class ExamineCommand : EamonRT.Game.Commands.ExamineCommand, IExamineCommand
	{
		public virtual bool ExamineConsole { get; set; }

		protected virtual void RevealArtifact(long artifactUid, bool examineConsole = false)
		{
			var artifact = Globals.ADB[artifactUid];

			Debug.Assert(artifact != null);

			if (!artifact.Seen)
			{
				artifact.SetInRoom(ActorRoom);

				var command = Globals.CreateInstance<EamonRT.Framework.Commands.IExamineCommand>(x =>
				{
					((IExamineCommand)x).ExamineConsole = examineConsole;
				});

				CopyCommandData(command);

				command.DobjArtifact = artifact;

				NextState = command;
			}
		}

		protected override void PlayerProcessEvents()
		{
			var gameState = Globals.GameState as IGameState;

			Debug.Assert(gameState != null);

			switch (DobjArtifact.Uid)
			{
				case 2:
				case 83:

					// Engraving/fake wall

					if (gameState.FakeWallExamines < 2)
					{
						Globals.Out.Write("{0}Examining {1} reveals something curious:{0}", Environment.NewLine, DobjArtifact.GetDecoratedName03(false, true, false, false, Globals.Buf));
					}

					gameState.FakeWallExamines++;

					Globals.Engine.PrintEffectDesc(40 + gameState.FakeWallExamines);

					if (gameState.FakeWallExamines > 2)
					{
						gameState.FakeWallExamines = 2;
					}

					break;

				case 23:

					// Magnetic fusion power plant

					Globals.Engine.PrintEffectDesc(37);

					var combatSystem = Globals.CreateInstance<EamonRT.Framework.Combat.ICombatSystem>(x =>
					{
						x.SetNextStateFunc = s => NextState = s;

						x.DfMonster = ActorMonster;

						x.OmitArmor = true;
					});

					combatSystem.ExecuteCalculateDamage(1, 6);

					break;

				case 25:

					// Pool pals

					if (!gameState.Shark)
					{
						var monster = Globals.MDB[7];

						Debug.Assert(monster != null);

						monster.SetInRoom(ActorRoom);

						monster = Globals.MDB[8];

						Debug.Assert(monster != null);

						monster.SetInRoom(ActorRoom);

						Globals.Engine.CheckEnemies();

						Globals.Engine.PrintEffectDesc(1);

						gameState.Shark = true;

						NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();
					}

					break;

				case 45:

					RevealArtifact(46);

					break;

				case 63:

					if (ExamineConsole)
					{
						RevealArtifact(64, true);
					}
					else
					{
						RevealArtifact(65);
					}

					break;

				case 58:

					RevealArtifact(59);

					break;

				case 59:

					RevealArtifact(60);

					break;

				case 64:

					if (!ExamineConsole)
					{
						RevealArtifact(66);
					}

					break;

				case 66:

					RevealArtifact(67);

					break;

				case 67:

					RevealArtifact(68);

					break;

				case 68:

					RevealArtifact(69);

					break;

				case 69:

					RevealArtifact(70);

					break;

				case 62:

					RevealArtifact(63, true);

					break;

				default:

					base.PlayerProcessEvents();

					break;
			}
		}
	}
}
