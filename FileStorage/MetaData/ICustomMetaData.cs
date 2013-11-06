using DynamiteXml;

namespace FileStorage.MetaData
{
    public interface ICustomMetaData : IDynamiteXml
    {
        string GetInfo();
    }
}