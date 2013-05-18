using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using Localization.Core;

namespace Localization.MvcProviders
{
    public class ModelMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        private readonly ILocalizedStringProvider _localizer;
        private readonly ITypeNameFactory _sourceNameFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="localizer"></param>
        /// <param name="sourceNameFactory"> </param>
        /// <param name="logger"> </param>
        public ModelMetadataProvider(ILocalizedStringProvider localizer, ITypeNameFactory sourceNameFactory, ILogger logger)
        {
            _localizer = localizer;
            _sourceNameFactory = sourceNameFactory;
            logger.Debug("ModelMetadataProvider: native models culture: {0}.", localizer.NativeCulture.DisplayName);
        }

        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
        {
            var metadata = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);
            if (containerType == null || propertyName == null)
                return metadata;

            metadata.DisplayName = _localizer.Translate(_sourceNameFactory.GetSourceName(containerType), metadata.DisplayName ?? propertyName, CultureInfo.CurrentUICulture, false);

            return metadata;
        }


    }
}
