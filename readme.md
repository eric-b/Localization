# Localization API

## What is it ?

It's a fairly simple and lazy API for string localization for any .NET application.

**Important note:**  
This API is not ready to use. You have at least to implement a datalayer (`ILocalizedRepository`). Other interfaces a extension points.  
 For a ready to use API, you can look at another open-source project: [Griffin.MvcContrib](https://github.com/jgauffin/griffin.mvccontrib).

## Synopsis


	@* // Razor view: *@
	<p>@Html.Translate("Texte à traduire...")</p>

	// Legacy class:
    private readonly StringProvider<OwnerClass> _localizer;
    public void Method()
    {
		string localizedString = _localizer.Translate("Texte à traduire...");
    }

## Why lazy ?

The typical approach to deal with localization is to use some sort of key/value pair dictionnary (like local & global resource files in ASP.NET).

This API enables a more naturaly way to provide localized strings: the key is the string itself. So you can hard-code strings and translate them later.

## Setup

The API is currently composed of 2 assemblies:

* **Localization.Core**: core implementation (portable class library for .NET 4, Silverlight 4 & Windows Store apps).
* **Localization.MvcProviders**: provides `HtmlHelper.Translate()` extension method and the `ModelMetadataProvider` class (MVC 4).

These assemblies are published on Nuget: [Localization.Core](http://nuget.org/packages/Localization.Core/) and [Localization.MvcProviders](http://nuget.org/packages/Localization.MvcProviders/).

This API contains providers for string localization from:

* Legacy class (.NET 4)
* MVC 4 view model (property name or `DisplayAttribute`)
* MVC 4 view (via HtmlHelper extension method).

Each string is stored with the following metadata:

* Native string
* Source (identifies the origin of the native string)
* Key (identifies all versions of a native string)
* Translated string
* Culture of translated string

*Hint*: the underlying datalayer is free to add other metadata, like 'applicationName' and 'updatedOn' fields.

The main services are:

* **ILocalizedRepository**: represents the data layer (lower layer)
* **ILocalizedStringProvider**: main logic layer (middle layer)
* **StringProvider< T >**: localized string provider for legacy classes (higher layer)
* **ModelMetaDataProvider**: localized property names provider for view models (higher layer)

The other services are all extension points:

* **ITextKeyFactory**: responsible for the key generation based on source and native string.
* **ITypeNameFactory**: responsible for the source name generation based on a type (provider specific).
* **IViewNameFactory**: responsible for the source name generation based on route data (provider specific).
* **IMissingLocalizedStringExtensionPoint**: extension point to add special treatment for missing localized strings (special formatting to bring out missing culture for example, or to log some useful informations...).

Setup depends on the arcitecture of your application. The recommanded way is using an IoC container like [SimpleInjector](http://simpleinjector.codeplex.com/) :

	// Pre-requisites:
    container.RegisterSingle<ILogger, EmptyLogger>();
    container.RegisterSingle<ITextKeyFactory, DefaultTextKeyFactory>();
    container.RegisterSingle<ITypeNameFactory, DefaultTypeNameFactory>();

	//
    // Xml file repository (replace this by your repository):
	//
    //   var translationsPath = HttpContext.Current.Server.MapPath("~/App_Data/translations.xml");
    //   container.RegisterSingle<ILocalizedRepository>(() => new XmlFileRepository(translationsPath));
	// 

	var fallbackCulture = CultureInfo.GetCultureInfo("en-US"); // fallback culture is english

    // View localization:
    container.RegisterSingle<IViewNameFactory, DefaultViewNameFactory>();
    container.RegisterSingle<ILocalizedStringProvider>(
        () =>
        new DefaultLocalizedStringProvider( 
            // provider used for views and legacy classes localization
            container.GetInstance<ILocalizedRepository>(), 
            container.GetInstance<ITextKeyFactory>(),
            container.GetInstance<ILogger>(), 
            CultureInfo.GetCultureInfo("fr-FR"), // native text is in french...
			fallbackCulture, 
			DefaultMissingLocalizedStringExtensionPoint.Instance
            ));

    // Legacy class localization:
    container.RegisterManyForOpenGeneric(typeof(StringProvider<>), typeof(StringProvider<>).Assembly);

    // Model metadata localization:
    container.RegisterSingle<Localization.MvcProviders.ModelMetadataProvider>(() => new Localization.MvcProviders.ModelMetadataProvider(
                                                                    new DefaultLocalizedStringProvider(
                                                                        container.GetInstance<ILocalizedRepository>(),
                                                                        container.GetInstance<ITextKeyFactory>(),
                                                                        container.GetInstance<ILogger>(),
                                                                        CultureInfo.GetCultureInfo("en-US"), // Names of model properties are in english...
																		fallbackCulture,
																		DefaultMissingLocalizedStringExtensionPoint.Instance
																		), 
                                                                    container.GetInstance<ITypeNameFactory>(),
                                                                    container.GetInstance<ILogger>()));

The default model metadata provider should be replaced accordingly (usually in global.asax after dependency resolver initialization) :

	ModelMetadataProviders.Current = DependencyResolver.Current.GetService<Localization.MvcProviders.ModelMetadataProvider>();

## Typical usage

See TestMvcApp demo for typical use cases.

### Usage from a Razor view (ASP.NET MVC 4)

	@Html.Translate("Texte à traduire")

### Model metadata

Localization.MvcProviders.ModelMetadataProvider provides localized property names. Example :

	@Html.DisplayForModel()

The default metadata provider must be replaced (usually in global.asax) :

	ModelMetadataProviders.Current = DependencyResolver.Current.GetService<Localization.MvcProviders.ModelMetadataProvider>();

### Usage from a legacy class (MVC Controller or anything else)

Use the generic StringProvider< T > class. The generic parameter type T represents the owner class. Example of an MVC controller which uses implicitly the dependency resolver :

	public class HomeController : Controller
    {
        private readonly StringProvider<HomeController> _localizer;

        public HomeController(StringProvider<HomeController> localizer)
        {
            _localizer = localizer;
        }
        
        public ActionResult Index()
        {
            string localizedString = _localizer.Translate("Texte à traduire");

			// [...]
        }

    }

### Limitations, caveats, known bugs

This API does not provide any administration UI. You have to implement it if needed.  
Furthermore, you need to implement your own datalayer (`ILocalizedRepository`). Typically, this will be an SQL datalayer with the same database for all projects in the same scope (web sites, etc.).

[Let me know](https://github.com/eric-b/Localization/issues) if you have troubles with use of this library.


## See also

An another API which inspired me: [Localization in ASP.NET MVC with Griffin.MvcContrib](http://www.codeproject.com/Articles/352583/Localization-in-ASP-NET-MVC-with-Griffin-MvcContri)

