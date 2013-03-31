namespace TestMvcApp.App_Start
{
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc;
    using SimpleInjector;
    using SimpleInjector.Extensions;
    using SimpleInjector.Integration.Web.Mvc;
    using System.Globalization;

    using Localization;
    using Localization.Core;
    using Localization.MvcProviders;    
    
    public static class SimpleInjectorInitializer
    {
        /// <summary>Initialize the container and register it as MVC3 Dependency Resolver.</summary>
        public static void Initialize()
        {
            // Did you know the container can diagnose your configuration? Go to: http://bit.ly/YE8OJj.
            var container = new Container();
            
            InitializeContainer(container);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            
            container.RegisterMvcAttributeFilterProvider();
       
            //container.Verify();
            
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
     
        private static void InitializeContainer(Container container)
        {
            // Pre-requisites:
            container.RegisterSingle<ILogger, EmptyLogger>();
            container.RegisterSingle<ITextKeyFactory, DefaultTextKeyFactory>();
            container.RegisterSingle<ITypeNameFactory, DefaultTypeNameFactory>();

            // Xml file repository:
            var translationsPath = HttpContext.Current.Server.MapPath("~/App_Data/translations.xml");
            container.RegisterSingle<ILocalizedRepository>(() => new XmlFileRepository(translationsPath));

            // View localization:
            container.RegisterSingle<IViewNameFactory, DefaultViewNameFactory>();
            container.RegisterSingle<ILocalizedStringProvider>(
                () =>
                new DefaultLocalizedStringProvider( 
                    // provider used for views and legacy classes localization
                    container.GetInstance<ILocalizedRepository>(), 
                    container.GetInstance<ITextKeyFactory>(),
                    container.GetInstance<ILogger>(), 
                    CultureInfo.GetCultureInfo("fr-FR") // native text is in french...
                    ));

            // Legacy class localization:
            container.RegisterManyForOpenGeneric(typeof(StringProvider<>), typeof(StringProvider<>).Assembly);

            // Model metadata localization:
            container.RegisterSingle<Localization.MvcProviders.ModelMetadataProvider>(() => new Localization.MvcProviders.ModelMetadataProvider(
                                                                           new DefaultLocalizedStringProvider(
                                                                               container.GetInstance<ILocalizedRepository>(),
                                                                               container.GetInstance<ITextKeyFactory>(),
                                                                               container.GetInstance<ILogger>(),
                                                                               CultureInfo.GetCultureInfo("en-US")), // Names of model properties are in english...
                                                                            container.GetInstance<ITypeNameFactory>(),
                                                                            container.GetInstance<ILogger>()));
        }
    }
}