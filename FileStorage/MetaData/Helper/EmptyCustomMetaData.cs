using System;

namespace FileStorage.MetaData.Helper
{
    /// <summary>
    ///     Empty custom meta data helper class, will be used as the default customer metadata, when the
    ///     code specifies 'null' as custom meta data.
    /// </summary>
    [Serializable]
    public class EmptyCustomMetaData : ICustomMetaData
    {
        #region ICustomMetaData Members

        public string GetInfo()
        {
            return "{Empty; no meta data}";
        }

        #endregion
    }
}