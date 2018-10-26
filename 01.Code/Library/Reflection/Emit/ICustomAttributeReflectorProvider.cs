namespace SOAFramework.Library
{
    public interface ICustomAttributeReflectorProvider
    {
        CustomAttributeReflector[] CustomAttributeReflectors { get; }
    }
}