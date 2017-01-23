/***************************************************************************************
 * 
 * This file contains several class that simplify the serialization / deserialization
 * of the requests/responses to the RESTful API.
 * 
 ***************************************************************************************/
namespace Recommendations
{
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using Newtonsoft.Json.Converters;
    #region response classes
    public enum OperationType
    {
        BuildModel = 0, // build is in queue waiting for execution
    }

    public enum OperationStatus
    {
        NotStarted = 0, // build is in queue waiting for execution
        Running = 1,  // build is in progress
        Cancelling = 2, // build is in the process of cancellation
        Cancelled = 3, // build was cancelled
        Succeeded = 4, // build ended with success
        Failed = 5, // build ended with error
    }


    [DataContract]
    public class BuildInfo
    {
        [DataMember]
        [JsonProperty("id")]
        public long Id { get; set; }

        [DataMember]
        [JsonProperty("description")]
        public string Description { get; set; }

        [DataMember]
        [JsonProperty("type")]
        public BuildType Type { get; set; }

        [DataMember]
        [JsonProperty("modelName")]
        public string ModelName { get; set; }

        [DataMember]
        [JsonProperty("modelId")]
        public string ModelId { get; set; }

        [DataMember]
        [JsonProperty("status")]
        public string Status { get; set; }

        [DataMember]
        [JsonProperty("statusMessage")]
        public string StatusMessage { get; set; }

        [DataMember]
        [JsonProperty("startDateTime")]
        public string StartDateTime { get; set; }

        [DataMember]
        [JsonProperty("endDateTime")]
        public string EndDateTime { get; set; }

        [DataMember]
        [JsonProperty("modifiedDateTime")]
        public string ModifiedDateTime { get; set; }

    }


    [DataContract]
    public class BuildInfoList
    {
        [DataMember]
        [JsonProperty("builds")]
        public IEnumerable<BuildInfo> Builds { get; set; }
    }


    [DataContract]
    public class BuildModelResponse
    {
        [DataMember]
        [JsonProperty("buildId")]
        public long BuildId { get; set; }
    }

    [DataContract]
    public class CatalogImportStats
    {
        [DataMember]
        [JsonProperty("processedLineCount")]
        public int ProcessedLineCount { get; set; }

        [DataMember]
        [JsonProperty("errorLineCount")]
        public int ErrorLineCount { get; set; }

        [DataMember]
        [JsonProperty("importedLineCount")]
        public int ImportedLineCount { get; set; }

        [DataMember]
        [JsonProperty("errorSummary")]
        public IEnumerable<ImportErrorStats> ErrorSummary { get; set; }

    }

    [DataContract]
    public class ImportErrorStats
    {
        [DataMember]
        [JsonProperty("errorCode")]
        public string ErrorCode { get; set; }

        [DataMember]
        [JsonProperty("errorCodeCount")]
        public int ErrorCodeCount { get; set; }
    }


    [DataContract]
    public class ModelInfo
    {
        [DataMember]
        [JsonProperty("id")]
        public string Id { get; set; }

        [DataMember]
        [JsonProperty("name")]
        public string Name { get; set; }

        [DataMember]
        [JsonProperty("description")]
        public string Description { get; set; }

        [DataMember]
        [JsonProperty("createdDateTime")]
        public string CreatedDateTime { get; set; }

        [DataMember]
        [JsonProperty("activeBuildId")]
        public long ActiveBuildId { get; set; }

    }

    [DataContract]
    public class ModelInfoList
    {
        [DataMember]
        [JsonProperty("models")]
        public IEnumerable<ModelInfo> Models { get; set; }
    }


    public class OperationInfo<T>
    {
        [DataMember]
        [JsonProperty("type")]
        public string Type { get; set; }

        [DataMember]
        [JsonProperty("status")]
        public string Status { get; set; }

        [DataMember]
        [JsonProperty("createdDateTime")]
        public string CreatedDateTime { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [JsonProperty("lastActionDateTime", NullValueHandling = NullValueHandling.Ignore)]
        public string LastActionDateTime { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [JsonProperty("percentComplete", NullValueHandling = NullValueHandling.Ignore)]
        public int PercentComplete { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [JsonProperty("resourceLocation", NullValueHandling = NullValueHandling.Ignore)]
        public string ResourceLocation { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [JsonProperty("result", NullValueHandling = NullValueHandling.Ignore)]
        public T Result { get; set; }
    }


    [DataContract]
    public class RecommendedItemInfo
    {
        [DataMember]
        [JsonProperty("id")]
        public string Id { get; set; }

        [DataMember]
        [JsonProperty("name")]
        public string Name { get; set; }

        [DataMember]
        [JsonProperty("metadata")]
        public string Metadata { get; set; }
    }

    /// <summary>
    /// Holds a recommendation result, which is a set of recommended items with reasoning and rating/score.
    /// </summary>
    [DataContract]
    public class RecommendedItemSetInfo
    {
        public RecommendedItemSetInfo()
        {
            Items = new List<RecommendedItemInfo>();
        }

        [DataMember]
        [JsonProperty("items")]
        public IEnumerable<RecommendedItemInfo> Items { get; set; }

        [DataMember]
        [JsonProperty("rating")]
        public double Rating { get; set; }

        [DataMember]
        [JsonProperty("reasoning")]
        public IEnumerable<string> Reasoning { get; set; }
    }


    [DataContract]
    public class RecommendedItemSetInfoList
    {
        [DataMember]
        [JsonProperty("recommendedItems")]
        public IEnumerable<RecommendedItemSetInfo> RecommendedItemSetInfo { get; set; }
    }

    [DataContract]
    public class UsageImportStats : CatalogImportStats
    {
        [DataMember]
        [JsonProperty("fileId")]
        public string FileId { get; set; }

    }

    #endregion

    #region request classes

    [DataContract]
    public class BuildParameters
    {
        [DataMember]
        [JsonProperty("ranking")]
        public RankingBuildParameters Ranking { get; set; }

        [DataMember]
        [JsonProperty("recommendation")]
        public RecommendationBuildParameters Recommendation { get; set; }

        [DataMember]
        [JsonProperty("fbt")]
        public FbtBuildParameters Fbt { get; set; }
    }


    [DataContract]
    public class BuildRequestInfo
    {
        [JsonProperty("description")]
        public string Description { get; set; }
        
        [JsonProperty("buildType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public BuildType BuildType { get; set; }

        [JsonProperty("buildParameters")]
        public BuildParameters BuildParameters { get; set; }
    }


    [DataContract]
    public class UpdateActiveBuildInfo
    {
        [JsonProperty("activeBuildId")]
        public long ActiveBuildId { get; set; }
    }

    [DataContract]
    public class BatchJobInfo
    {
        /// <summary>
        /// Unique batch identifier
        /// </summary>
        [DataMember]
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Description of the batch request
        /// </summary>
        [DataMember]
        [JsonProperty("requestinfo")]
        public BatchJobsRequestInfo RequestInfo { get; set; }

        /// <summary>
        /// Status of the batch job (it is actually the job status: Registered, Ready, InProgress, Canceling, Canceled, Succeeded, Failed)
        /// </summary>
        [DataMember]
        [JsonProperty("status")]
        public string Status { get; set; }
    }

    [DataContract]
    public class BatchJobsRequestInfo
    {
        /// <summary>
        /// The input storage blob info
        /// </summary>
        [JsonProperty("input")]
        public StorageBlobInfo Input { get; set; }

        /// <summary>
        /// The output storage blob info
        /// </summary>
        [JsonProperty("output")]
        public StorageBlobInfo Output { get; set; }

        /// <summary>
        /// The error storage blob info
        /// </summary>
        [JsonProperty("error")]
        public StorageBlobInfo Error { get; set; }

        /// <summary>
        /// The job info
        /// </summary>
        [JsonProperty("job")]
        public JobInfo Job { get; set; }
    }

    [DataContract]
    public class StorageBlobInfo
    {
        /// <summary>
        /// Authentication Type
        /// value "PublicOrSas"
        /// </summary>
        [JsonProperty("authenticationType")]
        public string AuthenticationType { get; set; }

        /// <summary>
        /// Base Location
        /// ex: "https://{storage name}.blob.core.windows.net/"
        /// </summary>
        [JsonProperty("baseLocation")]
        public string BaseLocation { get; set; }

        /// <summary>
        /// The relative location, including the container name
        /// </summary>
        [JsonProperty("relativeLocation")]
        public string RelativeLocation { get; set; }

        /// <summary>
        /// The sasToken to access the file
        /// </summary>
        [JsonProperty("sasBlobToken")]
        public string SasBlobToken { get; set; }
    }

    [DataContract]
    public class JobInfo
    {
        /// <summary>
        /// Api Name
        /// The ApiName is internally an enum (see SupportedApis in BatchScoringManager)
        /// The valid values should be: ItemRecommend, UserRecommend, ItemFbtRecommend
        /// </summary>
        [JsonProperty("apiName")]
        public string ApiName { get; set; }

        /// <summary>
        /// The Model Id
        /// ModelId is the model id which batch scoring is requested to
        /// </summary>
        [JsonProperty("modelId")]
        public string ModelId { get; set; }

        /// <summary>
        /// The build Id
        /// BuildId is the build id which batch scoring is requested to
        /// It is optional. If it is not provided, the active build id will be used
        /// </summary>
        [JsonProperty("buildId")]
        public long BuildId { get; set; }

        /// <summary>
        /// Number of recommendations
        /// It indicates the number of results (recommended items) each request should return
        /// It is optional. The default value is 10
        /// </summary>
        [JsonProperty("numberOfResults")]
        public int NumberOfResults { get; set; }

        /// <summary>
        /// Include Metadata
        /// it indicates whether the result should include metadata or not
        /// It is optional. The default value is false
        /// </summary>
        [JsonProperty("includeMetadata")]
        public bool IncludeMetadata { get; set; }

        /// <summary>
        /// The minimum score. Currently only supported for FbtBuilds
        /// It indicates the minimal score to return
        /// It is optional. The default value is 0.1
        /// </summary>
        [JsonProperty("minimalScore")]
        public double MinimalScore { get; set; }
    }

    [DataContract]
    public class BatchJobsResponse
    {
        /// <summary>
        /// Unique batch job identifier 
        /// </summary>
        [DataMember]
        [JsonProperty("batchId")]
        public string BatchId { get; set; }
    }

    /// <summary>
    /// Types of build
    /// </summary>
    [DataContract]
    public enum BuildType
    {
        /// <summary>
        /// Build that will create a model for recommendation
        /// </summary>
        Recommendation = 1,

        /// <summary>
        /// A build that creates a model to score features.
        /// </summary>
        Ranking = 2,

        /// <summary>
        /// A build that creates a model for fbt
        /// </summary>
        Fbt = 3,
    }

    /// <summary>
    /// FBT similarity functions
    /// </summary>
    [DataContract]
    public enum FbtSimilarityFunction
    {
        /// <summary>
        /// Count of co-occurrences, favors predictability.
        /// </summary>
        Cooccurrence,
        /// <summary>
        /// Lift favors serendipity
        /// </summary>
        Lift,
        /// <summary>
        /// Jaccard is a compromise between co-occurrences and lift.
        /// </summary>
        Jaccard 
    }



    /// <summary>
    /// FBT similarity functions
    /// </summary>
    [DataContract]
    public enum SplitterStrategy
    {
        /// <summary>
        /// Takes last transaction of users as the test set to use for evaluation.
        /// </summary>
        LastEventSplitter,
        /// <summary>
        /// Takes a random set of transactions as the test set to use for evaluation.
        /// </summary>
        RandomSplitter
    }


    [DataContract]
    public class RandomSplitterParameters
    {
        [DataMember]
        [JsonProperty("testPercent")]
        public int? TestPercent { get; set; }

        [DataMember]
        [JsonProperty("randomSeed")]
        public int? RandomSeed { get; set; }
    }

    [DataContract]
    public class FbtBuildParameters
    {
        [DataMember]
        [JsonProperty("supportThreshold")]
        public int? SupportThreshold { get; set; }

        [DataMember]
        [JsonProperty("maxItemSetSize")]
        public int? MaxItemSetSize { get; set; }

        [DataMember]
        [JsonProperty("minimalScore")]
        public double? MinimalScore { get; set; }

        [DataMember]
        [JsonProperty("similarityFunction")]
        [JsonConverter(typeof(StringEnumConverter))]
        public FbtSimilarityFunction SimilarityFunction { get; set; }

        [DataMember]
        [JsonProperty("enableModelingInsights")]
        public bool? EnableModelingInsights { get; set; }

        [DataMember]
        [JsonProperty("splitterStrategy")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SplitterStrategy SplitterStrategy { get; set; }

        [DataMember]
        [JsonProperty("randomSplitterParameters")]
        public RandomSplitterParameters RandomSplitterParameters { get; set; }

    }



    [DataContract]
    public class RecommendationBuildParameters
    {
        [DataMember]
        [JsonProperty("numberOfModelIterations")]
        public int? NumberOfModelIterations { get; set; }

        [DataMember]
        [JsonProperty("numberOfModelDimensions")]
        public int? NumberOfModelDimensions { get; set; }

        [DataMember]
        [JsonProperty("itemCutOffLowerBound")]
        public int? ItemCutOffLowerBound { get; set; }

        [DataMember]
        [JsonProperty("itemCutOffUpperBound")]
        public int? ItemCutOffUpperBound { get; set; }

        [DataMember]
        [JsonProperty("userCutOffLowerBound")]
        public int? UserCutOffLowerBound { get; set; }

        [DataMember]
        [JsonProperty("userCutOffUpperBound")]
        public int? UserCutOffUpperBound { get; set; }

        [DataMember]
        [JsonProperty("enableModelingInsights")]
        public bool? EnableModelingInsights { get; set; }

        [DataMember]
        [JsonProperty("splitterStrategy")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SplitterStrategy SplitterStrategy { get; set; }


        [DataMember]
        [JsonProperty("randomSplitterParameters")]
        public RandomSplitterParameters RandomSplitterParameters { get; set; }

        [DataMember]
        [JsonProperty("useFeaturesInModel")]
        public bool? UseFeaturesInModel { get; set; }

        [DataMember]
        [JsonProperty("modelingFeatureList")]
        public string ModelingFeatureList { get; set; }

        [DataMember]
        [JsonProperty("allowColdItemPlacement")]
        public bool? AllowColdItemPlacement { get; set; }

        [DataMember]
        [JsonProperty("enableFeatureCorrelation")]
        public bool? EnableFeatureCorrelation { get; set; }

        [DataMember]
        [JsonProperty("reasoningFeatureList")]
        public string ReasoningFeatureList { get; set; }

        [DataMember]
        [JsonProperty("enableU2I")]
        public bool? EnableU2I { get; set; }

    }

    [DataContract]
    public class ModelRequestInfo
    {
        [DataMember]
        [JsonProperty("modelName")]
        public string ModelName { get; set; }

        [DataMember]
        [JsonProperty("description")]
        public string Description { get; set; }
    }

    [DataContract]
    public class RankingBuildParameters
    {
        [DataMember]
        [JsonProperty("numberOfModelIterations")]
        public int? NumberOfModelIterations { get; set; }

        [DataMember]
        [JsonProperty("numberOfModelDimensions")]
        public int? NumberOfModelDimensions { get; set; }

        [DataMember]
        [JsonProperty("itemCutOffLowerBound")]
        public int? ItemCutOffLowerBound { get; set; }

        [DataMember]
        [JsonProperty("itemCutOffUpperBound")]
        public int? ItemCutOffUpperBound { get; set; }

        [DataMember]
        [JsonProperty("userCutOffLowerBound")]
        public int? UserCutOffLowerBound { get; set; }

        [DataMember]
        [JsonProperty("userCutOffUpperBound")]
        public int? UserCutOffUpperBound { get; set; }


        [DataContract]
        public class RecommendationBuildParameters
        {
            [DataMember]
            [JsonProperty("numberOfModelIterations")]
            public int? NumberOfModelIterations { get; set; }

            [DataMember]
            [JsonProperty("numberOfModelDimensions")]
            public int? NumberOfModelDimensions { get; set; }

            [DataMember]
            [JsonProperty("itemCutOffLowerBound")]
            public int? ItemCutOffLowerBound { get; set; }

            [DataMember]
            [JsonProperty("itemCutOffUpperBound")]
            public int? ItemCutOffUpperBound { get; set; }

            [DataMember]
            [JsonProperty("userCutOffLowerBound")]
            public int? UserCutOffLowerBound { get; set; }

            [DataMember]
            [JsonProperty("userCutOffUpperBound")]
            public int? UserCutOffUpperBound { get; set; }

            [DataMember]
            [JsonProperty("enableModelingInsights")]
            public bool? EnableModelingInsights { get; set; }

            [DataMember]
            [JsonProperty("useFeaturesInModel")]
            public bool? UseFeaturesInModel { get; set; }

            [DataMember]
            [JsonProperty("modelingFeatureList")]
            public string ModelingFeatureList { get; set; }

            [DataMember]
            [JsonProperty("allowColdItemPlacement")]
            public bool? AllowColdItemPlacement { get; set; }

            [DataMember]
            [JsonProperty("enableFeatureCorrelation")]
            public bool? EnableFeatureCorrelation { get; set; }

            [DataMember]
            [JsonProperty("reasoningFeatureList")]
            public string ReasoningFeatureList { get; set; }

            [DataMember]
            [JsonProperty("enableU2I")]
            public bool? EnableU2I { get; set; }

        }


        [DataContract]
        public class UpdateModelRequestInfo
        {
            [DataMember]
            [JsonProperty("description")]
            public string Description { get; set; }

            [DataMember]
            [JsonProperty("activeBuildId")]
            public long? ActiveBuildId { get; set; }
        }
    }

    #endregion

}

