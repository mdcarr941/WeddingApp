namespace WeddingApp.Lib.Data
{
    /// <summary>
    /// A collection of configuration values for the WeddingApp.Web project.
    /// </summary>
    public class WebConfiguration
    {
        public static short SingletonId => 1;
        public short Id { get; } = SingletonId;

        public string? RsvpPassword { get; init; }

        /// <summary>
        /// The default <see cref="WebConfiguration"/> settings.
        /// </summary>
        public static WebConfiguration Default { get; }
            = new WebConfiguration
            {
                RsvpPassword = "A heart as full as the moon."
            };
    }
}
