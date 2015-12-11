using System;
using System.Collections.Generic;

namespace TeacherAppreciation.Infrastructure.ModelMetadata.Filters
{
    public class TextAreaByNameTemplateSelectorFilter : IModelMetadataFilter
    {
        private static readonly HashSet<string> TextAreaFieldNames = new HashSet<string>
        {
            "body",
            "comments"
        };

        public void TransformMetadata(System.Web.Mvc.ModelMetadata metadata, IEnumerable<Attribute> attributes)
        {
            if (!string.IsNullOrEmpty(metadata.PropertyName) && string.IsNullOrEmpty(metadata.DataTypeName) &&
                TextAreaFieldNames.Contains(metadata.PropertyName.ToLower()))
            {
                metadata.DataTypeName = "MultilineText";
            }
        }
    }
}