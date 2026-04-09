using System;

namespace ConfigGlobal.Interface
{
    public interface IResource
    {
        string ResourceGroup { get; set; }
        string ResourceName { get; set; }
        string ResourceValue { get; set; }
    }

    public interface IResourceInput : IResource
    {
        string DataFieldValue { get; set; }
    }

        [Serializable()]
    public class Resource : IResource
    {
        public string ResourceGroup { get; set; }
        public string ResourceName { get; set; }
        public string ResourceValue { get; set; }
    }

    [Serializable()]
    public class ResourceError : IResource
    {
        public string ResourceGroup { get; set; }
        public string ResourceName { get; set; }
        public string ResourceValue { get; set; }
        public string ResourceDefault { get; set; }
    }
}
