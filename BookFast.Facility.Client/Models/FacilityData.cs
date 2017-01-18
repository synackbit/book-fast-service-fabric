// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace BookFast.Facility.Client.Models
{
    using System.Linq;

    public partial class FacilityData
    {
        /// <summary>
        /// Initializes a new instance of the FacilityData class.
        /// </summary>
        public FacilityData() { }

        /// <summary>
        /// Initializes a new instance of the FacilityData class.
        /// </summary>
        /// <param name="name">Facility name</param>
        /// <param name="streetAddress">Facility street address</param>
        /// <param name="description">Facility description</param>
        /// <param name="longitude">Latitude</param>
        /// <param name="latitude">Longitude</param>
        /// <param name="images">Facility images</param>
        public FacilityData(string name, string streetAddress, string description = default(string), double? longitude = default(double?), double? latitude = default(double?), System.Collections.Generic.IList<string> images = default(System.Collections.Generic.IList<string>))
        {
            Name = name;
            Description = description;
            StreetAddress = streetAddress;
            Longitude = longitude;
            Latitude = latitude;
            Images = images;
        }

        /// <summary>
        /// Gets or sets facility name
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets facility description
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets facility street address
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "streetAddress")]
        public string StreetAddress { get; set; }

        /// <summary>
        /// Gets or sets latitude
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "longitude")]
        public double? Longitude { get; set; }

        /// <summary>
        /// Gets or sets longitude
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "latitude")]
        public double? Latitude { get; set; }

        /// <summary>
        /// Gets or sets facility images
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "images")]
        public System.Collections.Generic.IList<string> Images { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (Name == null)
            {
                throw new Microsoft.Rest.ValidationException(Microsoft.Rest.ValidationRules.CannotBeNull, "Name");
            }
            if (StreetAddress == null)
            {
                throw new Microsoft.Rest.ValidationException(Microsoft.Rest.ValidationRules.CannotBeNull, "StreetAddress");
            }
            if (this.Name != null)
            {
                if (this.Name.Length > 100)
                {
                    throw new Microsoft.Rest.ValidationException(Microsoft.Rest.ValidationRules.MaxLength, "Name", 100);
                }
                if (this.Name.Length < 3)
                {
                    throw new Microsoft.Rest.ValidationException(Microsoft.Rest.ValidationRules.MinLength, "Name", 3);
                }
            }
            if (this.Description != null)
            {
                if (this.Description.Length > 1000)
                {
                    throw new Microsoft.Rest.ValidationException(Microsoft.Rest.ValidationRules.MaxLength, "Description", 1000);
                }
                if (this.Description.Length < 0)
                {
                    throw new Microsoft.Rest.ValidationException(Microsoft.Rest.ValidationRules.MinLength, "Description", 0);
                }
            }
            if (this.StreetAddress != null)
            {
                if (this.StreetAddress.Length > 100)
                {
                    throw new Microsoft.Rest.ValidationException(Microsoft.Rest.ValidationRules.MaxLength, "StreetAddress", 100);
                }
                if (this.StreetAddress.Length < 0)
                {
                    throw new Microsoft.Rest.ValidationException(Microsoft.Rest.ValidationRules.MinLength, "StreetAddress", 0);
                }
            }
        }
    }
}