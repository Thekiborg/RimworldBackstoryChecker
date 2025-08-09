global using RimWorld;
global using Verse;
using System.Text;
using System.Text.RegularExpressions;

namespace BackstoryChecker
{
	[StaticConstructorOnStartup]
	internal class BackstoryChecker
	{
		private const string regex = @"\[(?:PAWN_(?!(?:labelShort|nameFull|nameFullDef|nameDef|nameIndef|kind|kindDef|kindIndef|kindPlural|kindPluralDef|kindPluralIndef|kindBase|kindBaseDef|kindBaseIndef|kindBasePlural|kindBasePluralDef|kindBasePluralIndef|factionPawnSingular|factionPawnSingularDef|factionPawnSingularIndef|factionPawnsPlural|factionPawnsPluralDef|factionPawnsPluralIndef|lifestage|lifestageDef|lifestageIndef|lifestageAdjective|title|titleDef|titleIndef|humanlike|label|definite|indefinite|labelPlural|labelPluralDef|labelPluralIndef|pronoun|possessive|objective|factionName|gender)\])[^\]]+|(?!PAWN_)[Pp][Aa][Ww][Nn]_[^\]]+)\]";
		static BackstoryChecker()
		{
			CheckBackstories();
		}

		private static void CheckBackstories()
		{
			foreach (BackstoryDef backstoryDef in DefDatabase<BackstoryDef>.AllDefsListForReading)
			{
				MatchCollection invalidForms = Regex.Matches(backstoryDef.description, regex);

				if (invalidForms.Count > 0)
				{
					StringBuilder sb = new();

					sb.AppendLine($"Unresolvable grammar found in the BackstoryDef {backstoryDef.defName}:");
					sb.AppendLine(backstoryDef.description);
					sb.AppendLine("------------------------------------------------------------------------------------------------------------------------------------------------------");
					sb.AppendLine("Found errors:");
					foreach (Match form in invalidForms)
					{
						sb.AppendLine(form.ToString());
					}

					Log.Error(sb.ToString());
				}
			}
		}
	}
}
