using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Localization.Core
{
    /// <summary>
    /// Default implementation of <see cref="IMissingLocalizedStringExtensionPoint"/>.
    /// </summary>
    public class DefaultMissingLocalizedStringExtensionPoint : IMissingLocalizedStringExtensionPoint
    {
        private static volatile IMissingLocalizedStringExtensionPoint _singleton;
        private static readonly object _singletonSync = new object();

        /// <summary>
        /// Gets default instance (singleton).
        /// </summary>
        public static IMissingLocalizedStringExtensionPoint Instance
        {
            get
            {
                if (_singleton == null)
                {
                    lock(_singletonSync)
                    {
                        if (_singleton == null)
                            _singleton = new DefaultMissingLocalizedStringExtensionPoint();
                    }
                }
                return _singleton;
            }
        }

        public string ProcessMissingLocalizedString(string text, System.Globalization.CultureInfo targetCulture, System.Globalization.CultureInfo textCulture)
        {
            return textCulture.Name == targetCulture.Name ? text : string.Format("[{1}: {0}]", text, targetCulture.Name);
        }
    }
}
