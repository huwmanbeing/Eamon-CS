﻿
// HintHelper.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Eamon.Framework;
using Eamon.Framework.Helpers.Generic;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Helpers.Generic;
using Enums = Eamon.Framework.Primitive.Enums;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.Helpers
{
	[ClassMappings(typeof(IHelper<IHint>))]
	public class HintHelper : Helper<IHint>
	{
		#region Protected Methods

		#region Interface IHelper

		#region GetPrintedName Methods

		protected virtual string GetPrintedNameNumAnswers()
		{
			return "Number Of Answers";
		}

		protected virtual string GetPrintedNameAnswersElement()
		{
			var i = Index;

			return string.Format("Answer #{0}", i + 1);
		}

		#endregion

		#region GetName Methods

		protected virtual string GetNameAnswers(bool addToNamesList)
		{
			for (Index = 0; Index < Record.Answers.Length; Index++)
			{
				GetName("AnswersElement", addToNamesList);
			}

			return "Answers";
		}

		protected virtual string GetNameAnswersElement(bool addToNamesList)
		{
			var i = Index;

			var result = string.Format("Answers[{0}].Element", i);

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		#endregion

		#region GetValue Methods

		protected virtual object GetValueAnswersElement()
		{
			var i = Index;

			return Record.GetAnswers(i);
		}

		#endregion

		#region Validate Methods

		protected virtual bool ValidateUid()
		{
			return Record.Uid > 0;
		}

		protected virtual bool ValidateQuestion()
		{
			return string.IsNullOrWhiteSpace(Record.Question) == false && Record.Question.Length <= Constants.HntQuestionLen;
		}

		protected virtual bool ValidateNumAnswers()
		{
			return Record.NumAnswers >= 1 && Record.NumAnswers <= Record.Answers.Length;
		}

		protected virtual bool ValidateAnswers()
		{
			var result = true;

			for (Index = 0; Index < Record.Answers.Length; Index++)
			{
				result = ValidateField("AnswersElement");

				if (result == false)
				{
					break;
				}
			}

			return result;
		}

		protected virtual bool ValidateAnswersElement()
		{
			var i = Index;

			Debug.Assert(i >= 0 && i < Record.Answers.Length);

			return i < Record.NumAnswers ? string.IsNullOrWhiteSpace(Record.GetAnswers(i)) == false && Record.GetAnswers(i).Length <= Constants.HntAnswerLen : Record.GetAnswers(i) == "";
		}

		#endregion

		#region ValidateInterdependencies Methods

		protected virtual bool ValidateInterdependenciesQuestion()
		{
			var result = true;

			long invalidUid = 0;

			var rc = Globals.Engine.ResolveUidMacros(Record.Question, Buf, false, false, ref invalidUid);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			if (invalidUid > 0)
			{
				result = false;

				Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("Question"), "effect", invalidUid, "which doesn't exist");

				ErrorMessage = Buf.ToString();

				RecordType = typeof(IEffect);

				NewRecordUid = invalidUid;

				goto Cleanup;
			}

		Cleanup:

			return result;
		}

		protected virtual bool ValidateInterdependenciesAnswers()
		{
			var result = true;

			for (Index = 0; Index < Record.Answers.Length; Index++)
			{
				result = ValidateFieldInterdependencies("AnswersElement");

				if (result == false)
				{
					break;
				}
			}

			return result;
		}

		protected virtual bool ValidateInterdependenciesAnswersElement()
		{
			var result = true;

			long invalidUid = 0;

			var i = Index;

			Debug.Assert(i >= 0 && i < Record.Answers.Length);

			if (i < Record.NumAnswers)
			{
				var rc = Globals.Engine.ResolveUidMacros(Record.GetAnswers(i), Buf, false, false, ref invalidUid);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				if (invalidUid > 0)
				{
					result = false;

					Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("AnswersElement"), "effect", invalidUid, "which doesn't exist");

					ErrorMessage = Buf.ToString();

					RecordType = typeof(IEffect);

					NewRecordUid = invalidUid;

					goto Cleanup;
				}
			}

		Cleanup:

			return result;
		}

		#endregion

		#region PrintDesc Methods

		protected virtual void PrintDescActive()
		{
			var fullDesc = "Enter the active status of the hint." + Environment.NewLine + Environment.NewLine + "An active hint is immediately available to the player, while inactive hints must be activated by special (user programmed) events.";

			var briefDesc = "0=Inactive; 1=Active";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		protected virtual void PrintDescQuestion()
		{
			var fullDesc = "Enter the name, topic or question of the hint.";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		protected virtual void PrintDescNumAnswers()
		{
			var fullDesc = "Enter the number of answers for the hint.";

			var briefDesc = string.Format("1-{0}=Valid value", Record.Answers.Length);

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		protected virtual void PrintDescAnswersElement()
		{
			var i = Index;

			var fullDesc = string.Format("Enter the hint's answer #{0}.", i + 1);

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		#endregion

		#region List Methods

		protected virtual void ListUid()
		{
			if (FullDetail)
			{
				if (!ExcludeROFields)
				{
					var listNum = NumberFields ? ListNum++ : 0;

					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Uid"), null), Record.Uid);
				}
			}
			else
			{
				Buf.SetFormat("{0,3}. {1}", Record.Uid, Record.Question);

				Globals.Engine.WordWrap(Buf.ToString(), Buf01);

				var k = Buf01.IndexOf(Environment.NewLine);

				if (k >= 0)
				{
					Buf01.Length = k--;

					if (k >= 0)
					{
						Buf01[k--] = '.';
					}

					if (k >= 0)
					{
						Buf01[k--] = '.';
					}

					if (k >= 0)
					{
						Buf01[k--] = '.';
					}
				}

				Globals.Out.Write("{0}{1}", Environment.NewLine, Buf01);
			}
		}

		protected virtual void ListActive()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Active"), null), Convert.ToInt64(Record.Active));
			}
		}

		protected virtual void ListQuestion()
		{
			if (FullDetail)
			{
				Buf.Clear();

				if (ResolveEffects)
				{
					var rc = Globals.Engine.ResolveUidMacros(Record.Question, Buf, true, true);

					Debug.Assert(Globals.Engine.IsSuccess(rc));
				}
				else
				{
					Buf.Append(Record.Question);
				}

				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.WriteLine("{0}{1}{0}{0}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Question"), null), Buf);
			}
		}

		protected virtual void ListNumAnswers()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("NumAnswers"), null), Record.NumAnswers);
			}
		}

		protected virtual void ListAnswers()
		{
			for (Index = 0; Index < Record.Answers.Length; Index++)
			{
				ListField("AnswersElement");
			}

			AddToListedNames = false;
		}

		protected virtual void ListAnswersElement()
		{
			var i = Index;

			if (FullDetail && i < Record.NumAnswers)
			{
				Buf.Clear();

				if (ResolveEffects)
				{
					var rc = Globals.Engine.ResolveUidMacros(Record.GetAnswers(i), Buf, true, true);

					Debug.Assert(Globals.Engine.IsSuccess(rc));
				}
				else
				{
					Buf.Append(Record.GetAnswers(i));
				}

				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.WriteLine("{0}{1}{0}{0}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("AnswersElement"), null), Buf);
			}
		}

		#endregion

		#region Input Methods

		protected virtual void InputUid()
		{
			Globals.Out.Print("{0}{1}", Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("Uid"), null), Record.Uid);

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputActive()
		{
			var fieldDesc = FieldDesc;

			var active = Record.Active;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", Convert.ToInt64(active));

				PrintFieldDesc("Active", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("Active"), "1"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsChar0Or1, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.Active = Convert.ToInt64(Buf.Trim().ToString()) != 0 ? true : false;

				if (ValidateField("Active"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputQuestion()
		{
			var fieldDesc = FieldDesc;

			var question = Record.Question;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", question);

				PrintFieldDesc("Question", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("Question"), null));

				Globals.Out.WordWrap = false;

				var rc = Globals.In.ReadField(Buf, Constants.HntQuestionLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Out.WordWrap = true;

				Record.Question = Buf.Trim().ToString();

				if (ValidateField("Question"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputNumAnswers()
		{
			var fieldDesc = FieldDesc;

			var numAnswers = Record.NumAnswers;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", numAnswers);

				PrintFieldDesc("NumAnswers", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("NumAnswers"), "1"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.NumAnswers = Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("NumAnswers"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			var i = Math.Min(Record.NumAnswers, numAnswers);

			var j = Math.Max(Record.NumAnswers, numAnswers);

			while (i < j)
			{
				Record.SetAnswers(i, Record.NumAnswers > numAnswers ? "NONE" : "");

				i++;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputAnswers()
		{
			for (Index = 0; Index < Record.Answers.Length; Index++)
			{
				InputField("AnswersElement");
			}
		}

		protected virtual void InputAnswersElement()
		{
			var i = Index;

			if (i < Record.NumAnswers)
			{
				var fieldDesc = FieldDesc;

				var answer = Record.GetAnswers(i);

				while (true)
				{
					Buf.SetFormat(EditRec ? "{0}" : "", answer);

					PrintFieldDesc("AnswersElement", EditRec, EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("AnswersElement"), null));

					Globals.Out.WordWrap = false;

					var rc = Globals.In.ReadField(Buf, Constants.HntAnswerLen, null, '_', '\0', false, null, null, null, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Globals.Out.WordWrap = true;

					Record.SetAnswers(i, Buf.Trim().ToString());

					if (ValidateField("AnswersElement"))
					{
						break;
					}

					fieldDesc = Enums.FieldDesc.Brief;
				}

				Globals.Out.Print("{0}", Globals.LineSep);
			}
			else
			{
				Record.SetAnswers(i, "");
			}
		}

		#endregion

		#region BuildValue Methods

		// do nothing

		#endregion

		#endregion

		#region Class HintHelper

		protected override void SetUidIfInvalid()
		{
			if (Record.Uid <= 0)
			{
				Record.Uid = Globals.Database.GetHintUid();

				Record.IsUidRecycled = true;
			}
			else if (!EditRec)
			{
				Record.IsUidRecycled = false;
			}
		}

		#endregion

		#endregion

		#region Public Methods

		#region Interface IHelper

		public override void ListErrorField()
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(ErrorFieldName));

			Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, GetPrintedName("Uid"), null), Record.Uid);

			if (string.Equals(ErrorFieldName, "Question", StringComparison.OrdinalIgnoreCase))
			{
				Globals.Out.WriteLine("{0}{1}{0}{0}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, GetPrintedName("Question"), null), Record.Question);
			}
			else if (string.Equals(ErrorFieldName, "AnswersElement", StringComparison.OrdinalIgnoreCase))
			{
				var i = Index;

				Buf.SetFormat("{0}{1}", Globals.Engine.BuildPrompt(27, '.', 0, GetPrintedName("Question"), null), Record.Question);

				Globals.Engine.WordWrap(Buf.ToString(), Buf);

				var k = Buf.IndexOf(Environment.NewLine);

				if (k >= 0)
				{
					Buf.Length = k--;

					if (k >= 0)
					{
						Buf[k--] = '.';
					}

					if (k >= 0)
					{
						Buf[k--] = '.';
					}

					if (k >= 0)
					{
						Buf[k--] = '.';
					}
				}

				Globals.Out.Write("{0}{1}", Environment.NewLine, Buf);

				Globals.Out.WriteLine("{0}{1}{0}{0}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, GetPrintedName("AnswersElement"), null), Record.GetAnswers(i));
			}
		}

		#endregion

		#region Class HintHelper

		public HintHelper()
		{
			FieldNames = new List<string>()
			{
				"Uid",
				"IsUidRecycled",
				"Active",
				"Question",
				"NumAnswers",
				"Answers",
			};
		}

		#endregion

		#endregion
	}
}
