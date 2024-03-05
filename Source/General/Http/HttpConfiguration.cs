namespace Utilities.General.Http
{
    public class HttpConfiguration
    {
        public string BaseUri { get; set; }
        public uint? AttempsRequest { get; set; }
        public long? MaxResponseContentBufferSize { get; set; }
    }
}