using System;
using System.Collections.Generic;
using System.Linq;
using FileStorage.Enums.Features;

namespace FileStorage.Factories
{
    public static class FileStorageFeatureFactory
    {
        public static List<FileStorageFeatureEnum> GetDefaultFeatures()
        {
            var result = new List<FileStorageFeatureEnum>
                {
                    FileStorageFeatureEnum.StoreMetaData
                };
            return result;
        }

        public static int CreateFileStorageFeatureValue(List<FileStorageFeatureEnum> features)
        {
            //
            // ensure the list is distinct
            //
            features = features.Distinct().ToList();

            return features.Sum(feature => (int) feature);
        }

        public static List<FileStorageFeatureEnum> CreateFileStorageFeatureList(int fileStorageFeatureAsValue)
        {
            var result = new List<FileStorageFeatureEnum>();

            Type type = typeof (FileStorageFeatureEnum);
            foreach (FileStorageFeatureEnum currentFileStorageFeature in Enum.GetValues(type))
            {
                var currentValue = (int) currentFileStorageFeature;
                bool isCurrentFeatureEnabled = (fileStorageFeatureAsValue & currentValue) == currentValue;
                if (isCurrentFeatureEnabled)
                {
                    result.Add(currentFileStorageFeature);
                }
            }

            return result;
        }
    }
}