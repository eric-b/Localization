using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Localization.Core.Utils;

namespace Localization.Core
{
    /// <summary>
    /// <para>Default implementation of <see cref="ITextKeyFactory"/>.</para>
    /// <para>Generates a SHA-1 hash.</para>
    /// </summary>
    public sealed class DefaultTextKeyFactory : ITextKeyFactory
    {
        private readonly Encoding _encoding = Encoding.UTF8;

        private readonly Utils.MD5Managed _md5 = new MD5Managed();

        public string Create(string sourceName, string textName)
        {
            var data = _encoding.GetBytes(string.Format("{0};{1}", sourceName, textName));
            _md5.HashCore(data, 0, data.Length);
            var hash = _md5.HashFinal();
            _md5.InitializeVariables();
            var hex = string.Empty;
            for (int i = 0; i < hash.Length; i++)
                hex += hash[i].ToString("x2");
            return hex;
        }

    }
}
