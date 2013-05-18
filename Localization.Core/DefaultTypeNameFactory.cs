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
            // Special treatment for generic types (removes assembly qualified name of parameter types).
            if (type.IsGenericType)
                return ToGenericTypeString(type);

            var fullname = type.FullName;

            #region ImpromptuInterface special case
            const string generatedSuffix = "_generated";
            const string actLikePrefix = "ActLike_";
            if (fullname.StartsWith(actLikePrefix))
            {
                // Ex: "ActLike_IMyInterface_a2c76f01ac234addaf7deb40dbf4f2bc" -> "ActLike_IMyInterface_generated"
                var index = fullname.LastIndexOf('_');
                if (index > actLikePrefix.Length)
                    fullname = fullname.Substring(0, index) + generatedSuffix;
            }
            #endregion

            return fullname;
        }

        /// <summary>
        /// <para>Returns a friendly name for generic types.
        /// Example: "My.GenericType`1[[My.ParameterType, My.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]" -> "My.GenericType&lt;My.ParameterType>"</para>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected string ToGenericTypeString(Type type)
        {
            if (!type.IsGenericType)
                return type.FullName;
            var genericTypeName = type.GetGenericTypeDefinition().FullName;
            genericTypeName = genericTypeName.Substring(0, genericTypeName.IndexOf('`'));
            var genericArgs = string.Join(", ", type.GetGenericArguments().Select(ToGenericTypeString));
            return genericTypeName + "<" + genericArgs + ">";
        }
    }
}
