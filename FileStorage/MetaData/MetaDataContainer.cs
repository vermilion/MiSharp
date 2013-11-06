using System;
using DynamiteXml;

namespace FileStorage.MetaData
{
    [Serializable]
    public class MetaDataContainer : IDynamiteXml
    {
        /// <summary>
        ///     Short version to prevent the metadata growing rapidly
        /// </summary>
        private Int64 _bs;

        /// <summary>
        ///     Short version to prevent the metadata to become very big
        /// </summary>
        private string _c;

        /// <summary>
        ///     Short version to prevent the metadata to become very big
        /// </summary>
        private DateTime _ct;

        public MetaDataContainer()
        {
            // empty constructor is required for serialization    
        }

        public MetaDataContainer(ICustomMetaData customMetaData, DateTime creationDateUtc, Int64 binaryDataSizeInBytes)
        {
            CustomMetaDataString = DynamiteXmlLogic.Serialize(customMetaData);
            CreationDateUtc = creationDateUtc;
            BinarySizeInBytes = binaryDataSizeInBytes;
        }

        public string CustomMetaDataString
        {
            get { return _c; }
            set { _c = value; }
        }

        public DateTime CreationDateUtc
        {
            get { return _ct; }
            set { _ct = value; }
        }

        public Int64 BinarySizeInBytes
        {
            get { return _bs; }
            set { _bs = value; }
        }

        public ICustomMetaData CustomMetaData
        {
            get { return DynamiteXmlLogic.Deserialize(CustomMetaDataString) as ICustomMetaData; }
        }
    }
}