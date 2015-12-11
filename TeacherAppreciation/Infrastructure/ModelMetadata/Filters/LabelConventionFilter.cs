using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TeacherAppreciation.Infrastructure.ModelMetadata.Filters
{
    public class LabelConventionFilter : IModelMetadataFilter
    {
        public void TransformMetadata(System.Web.Mvc.ModelMetadata metadata, IEnumerable<Attribute> attributes)
        {
            if (!string.IsNullOrEmpty(metadata.PropertyName) && string.IsNullOrEmpty(metadata.DisplayName))
            {
                metadata.DisplayName = GetStringWithSpace(metadata.PropertyName);
            }
        }

        private string GetStringWithSpace(string input)
        {
            /*
             *  (?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z]|(?<![A-Z])[A-Z]$)
             *  (?<!^) Negative Lookbehind - Assert that it is impossible to match the regex below
             *      ^ assert position at start of the string
             *  1st Capturing group ([A-Z][a-z]|(?<=[a-z])[A-Z]|(?<![A-Z])[A-Z]$)
             *      1st Alternative: [A-Z][a-z]
             *          [A-Z] match a single character present in the list below
             *              A-Z a single character in the range between A and Z (case sensitive)
             *          [a-z] match a single character present in the list below
             *              a-z a single character in the range between a and z (case sensitive)
             *      2nd Alternative: (?<=[a-z])[A-Z]
             *          (?<=[a-z]) Positive Lookbehind - Assert that the regex below can be matched
             *              [a-z] match a single character present in the list below
             *                  a-z a single character in the range between a and z (case sensitive)
             *              [A-Z] match a single character present in the list below
             *                  A-Z a single character in the range between A and Z (case sensitive)
             *      3rd Alternative: (?<![A-Z])[A-Z]$
             *          (?<![A-Z]) Negative Lookbehind - Assert that it is impossible to match the regex below
             *              [A-Z] match a single character present in the list below
             *                  A-Z a single character in the range between A and Z (case sensitive)
             *              [A-Z] match a single character present in the list below
             *                  A-Z a single character in the range between A and Z (case sensitive)
             *          $ assert position at end of the string
             */
            return Regex.Replace(
                input,
                "(?<!^)" +
                "(" +
                "  [A-Z][a-z] |" +
                "  (?<=[a-z])[A-Z] |" +
                "  (?<![A-Z])[A-Z]$" +
                ")",
                " $1",
                RegexOptions.IgnorePatternWhitespace);
        }
    }
}