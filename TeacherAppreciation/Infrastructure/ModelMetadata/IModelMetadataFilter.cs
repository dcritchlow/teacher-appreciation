using System;
using System.Collections.Generic;

namespace TeacherAppreciation.Infrastructure.ModelMetadata
{
    public interface IModelMetadataFilter
    {
        void TransformMetadata(System.Web.Mvc.ModelMetadata metadata, IEnumerable<Attribute> attributes);
    }
}