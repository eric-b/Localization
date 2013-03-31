using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Localization.Core
{
    public class DefaultTypeNameFactory : ITypeNameFactory
    {
        public string GetSourceName(Type type)
        {
            var fullname = type.FullName;

            #region ImpromptuInterface special case
            const string generatedSuffix = "_generated";
            const string actLikePrefix = "ActLike_";
            if (fullname.StartsWith(actLikePrefix))
            {
                // Ex: "ActLike_IStep1_a2c76f01ac234addaf7deb40dbf4f2bc" -> "ActLike_IStep1_generated"
                var index = fullname.LastIndexOf('_');
                if (index > actLikePrefix.Length)
                    fullname = fullname.Substring(0, index) + generatedSuffix;
            }
            #endregion

            return fullname;
        }
    }
}
