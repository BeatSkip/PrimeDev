namespace PrimeWeb
{
    /// <summary>
    /// Destinations for the Usb file
    /// </summary>
    public enum StorageDestination
    {
        /// <summary>
        /// Physical device
        /// </summary>
        Calculator, 
        /// <summary>
        /// Connectivity Kit folder (if available)
        /// </summary>
        UserFolder,
        /// <summary>
        /// Custom destination
        /// </summary>
        Custom
    }
}