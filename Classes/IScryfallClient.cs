// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Scryfall.API
{
    using Models;
    using Newtonsoft.Json;

    /// <summary>
    /// </summary>
    public partial interface IScryfallClient : System.IDisposable
    {
        /// <summary>
        /// The base URI of the service.
        /// </summary>
        System.Uri BaseUri { get; set; }

        /// <summary>
        /// Gets or sets json serialization settings.
        /// </summary>
        JsonSerializerSettings SerializationSettings { get; }

        /// <summary>
        /// Gets or sets json deserialization settings.
        /// </summary>
        JsonSerializerSettings DeserializationSettings { get; }


        /// <summary>
        /// Gets the ISets.
        /// </summary>
        ISets Sets { get; }

        /// <summary>
        /// Gets the ICards.
        /// </summary>
        ICards Cards { get; }

        /// <summary>
        /// Gets the IRulings.
        /// </summary>
        IRulings Rulings { get; }

        /// <summary>
        /// Gets the ISymbology.
        /// </summary>
        ISymbology Symbology { get; }

        /// <summary>
        /// Gets the ICatalogOperations.
        /// </summary>
        ICatalogOperations Catalog { get; }

    }
}
