namespace Recommendations
{
    using System;
    using System.IO;
    using System.Net.Http;
    using Newtonsoft.Json;
    using System.Net.Http.Formatting;
    using System.Linq;
    using System.Threading;
    

    /// <summary>
    /// A wrapper class to invoke Recommendations REST APIs
    /// </summary>
    public class RecommendationsApiWrapper
    {
        private readonly HttpClient _httpClient;
        public string BaseUri;

        /// <summary>
        /// Constructor that initializes the Http Client.
        /// </summary>
        /// <param name="accountKey">The account key</param>
        public RecommendationsApiWrapper(string accountKey, string baseUri)
        {
            BaseUri = baseUri;

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(BaseUri),
                Timeout = TimeSpan.FromMinutes(15),
                DefaultRequestHeaders =
                {
                    {"Ocp-Apim-Subscription-Key", accountKey}
                }
            };
        }

        /// <summary>
        /// Creates a new model.
        /// </summary>
        /// <param name="modelName">Name for the model</param>
        /// <param name="description">Description for the model</param>
        /// <returns>Model Information.</returns>
        public ModelInfo CreateModel(string modelName, string description = null)
        {
            var uri = BaseUri + "/models/";
            var modelRequestInfo = new ModelRequestInfo { ModelName = modelName, Description = description };
            var response = _httpClient.PostAsJsonAsync(uri, modelRequestInfo).Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(String.Format("Error {0}: Failed to create model {1}, \n reason {2}",
                    response.StatusCode, modelName, ExtractErrorInfo(response)));
            }

            var jsonString = ExtractReponse(response);
            var modelInfo = JsonConvert.DeserializeObject<ModelInfo>(jsonString);
            return modelInfo;
        }

        /// <summary>
        /// Upload catalog items to a model.
        /// </summary>
        /// <param name="modelId">Unique identifier of the model</param>
        /// <param name="catalogFilePath">Catalog file path</param>
        /// <param name="catalogDisplayName">Name for the catalog</param>
        /// <returns>Statistics about the catalog import operation.</returns>
        public CatalogImportStats UploadCatalog(string modelId, string catalogFilePath, string catalogDisplayName)
        {
            Console.WriteLine("Uploading " + catalogDisplayName + " ...");
            string uri = BaseUri + "/models/" + modelId + "/catalog?catalogDisplayName=" + catalogDisplayName;
            using (var filestream = new FileStream(catalogFilePath, FileMode.Open, FileAccess.Read))
            {
                var response = _httpClient.PostAsync(uri, new StreamContent(filestream)).Result;

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(
                        String.Format("Error {0}: Failed to import catalog items {1}, for model {2} \n reason {3}",
                            response.StatusCode, catalogFilePath, modelId, ExtractErrorInfo(response)));
                }

                var jsonString = ExtractReponse(response);
                var catalogImportStats = JsonConvert.DeserializeObject<CatalogImportStats>(jsonString);
                return catalogImportStats;
            }
        }

        /// <summary>
        /// Upload usage data to a model.
        /// Usage files must be less than 200 MB.
        /// If you need to upload more than 200 MB, you may call this function multiple times.
        /// </summary>
        /// <param name="modelId">Unique identifier of the model</param>
        /// <param name="usageFilePath">Usage file path</param>
        /// <param name="usageDisplayName">Name for the usage data being uploaded</param>
        /// <returns>Statistics about the usage upload operation.</returns>
        public UsageImportStats UploadUsage(string modelId, string usageFilePath, string usageDisplayName)
        {
            Console.WriteLine("Uploading " + usageDisplayName + " ...");

            string uri = BaseUri + "/models/" + modelId + "/usage?usageDisplayName=" + usageDisplayName;

            using (var filestream = new FileStream(usageFilePath, FileMode.Open, FileAccess.Read))
            {
                var response = _httpClient.PostAsync(uri, new StreamContent(filestream)).Result;
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(
                        String.Format("Error {0}: Failed to import usage data {1}, for model {2} \n reason {3}",
                            response.StatusCode, usageFilePath, modelId, ExtractErrorInfo(response)));
                }

                var jsonString = ExtractReponse(response);
                var usageImportStats = JsonConvert.DeserializeObject<UsageImportStats>(jsonString);
                return usageImportStats;
            }
        }


        /// <summary>
        /// Submit a model build, with passed build parameters.
        /// </summary>
        /// <param name="modelId">Unique identifier of the model</param>
        /// <param name="buildRequestInfo">Build parameters</param>
        /// <param name="operationLocationHeader">Build operation location</param>
        /// <returns>The build id.</returns>
        public long BuildModel(string modelId, BuildRequestInfo buildRequestInfo, out string operationLocationHeader)
        {
            string uri = BaseUri + "/models/" + modelId + "/builds";
            var response = _httpClient.PostAsJsonAsync(uri, buildRequestInfo).Result;
            var jsonString = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(String.Format("Error {0}: Failed to start build for model {1}, \n reason {2}",
                    response.StatusCode, modelId, ExtractErrorInfo(response)));
            }

            operationLocationHeader = response.Headers.GetValues("Operation-Location").FirstOrDefault();
            var buildModelResponse = JsonConvert.DeserializeObject<BuildModelResponse>(jsonString);
            return buildModelResponse.BuildId;
        }


        /// <summary>
        /// Delete a certain build of a model.
        /// </summary>
        /// <param name="modelId">Unique identifier of the model</param>
        /// <param name="buildId">Unique identifier of the build</param>
        public void DeleteBuild(string modelId, long buildId)
        {
            string uri = BaseUri + "/models/" + modelId + "/builds/" + buildId;
            var response = _httpClient.DeleteAsync(uri).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    String.Format("Error {0}: Failed to delete buildId {1} for modelId {2}, \n reason {3}",
                        response.StatusCode, buildId, modelId, ExtractErrorInfo(response)));
            }
        }

        /// <summary>
        /// Delete a model, also the associated catalog/usage data and any builds.
        /// </summary>
        /// <param name="modelId">Unique identifier of the model</param>
        public void DeleteModel(string modelId)
        {
            string uri = BaseUri + "/models/" + modelId;
            var response = _httpClient.DeleteAsync(uri).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    String.Format("Error {0}: Failed to delete modelId {1}, \n reason {2}",
                        response.StatusCode, modelId, ExtractErrorInfo(response)));
            }
        }


        /// <summary>
        /// Trigger a recommendation build for the given model.
        /// Note: unless configured otherwise the u2i (user to item/user based) recommendations are enabled too.
        /// </summary>
        /// <param name="modelId">the model id</param>
        /// <param name="buildDescription">a description for the build</param>
        /// <param name="enableModelInsights"> true to enable modeling insights, selects "LastEventSplitter" as the splitting strategy by default. </param>
        /// <param name="operationLocationHeader">operation location header, can be used to cancel the build operation and to get status.</param>
        /// <returns>Unique indentifier of the build initiated.</returns>
        public long CreateRecommendationsBuild(string modelId,
                                               string buildDescription,
                                               bool enableModelInsights,
                                               out string operationLocationHeader)
        {
            // only used if splitter strategy is set to RandomSplitter
            var randomSplitterParameters = new RandomSplitterParameters()
            {
                RandomSeed = 0,
                TestPercent = 10
            };

            var parameters = new RecommendationBuildParameters()
            {
                NumberOfModelIterations = 10,
                NumberOfModelDimensions = 20,
                ItemCutOffLowerBound = 1,
                EnableModelingInsights = enableModelInsights,
                SplitterStrategy = SplitterStrategy.LastEventSplitter,
                RandomSplitterParameters = randomSplitterParameters,
                EnableU2I = true,
                UseFeaturesInModel = false,
                AllowColdItemPlacement = false,
            };

            var requestInfo = new BuildRequestInfo()
            {
                BuildType = BuildType.Recommendation,
                BuildParameters = new BuildParameters()
                {
                    Recommendation = parameters
                },
                Description = buildDescription
            };

            return BuildModel(modelId, requestInfo, out operationLocationHeader);
        }

        /// <summary>
        /// Trigger a recommendation build for the given model.
        /// Note: unless configured otherwise the u2i (user to item/user based) recommendations are enabled too.
        /// </summary>
        /// <param name="modelId">the model id</param>
        /// <param name="buildDescription">a description for the build</param>
        /// <param name="operationLocationHeader">operation location header, can be used to cancel the build operation and to get status.</param>
        /// <returns>Unique indentifier of the build initiated.</returns>
        public long CreateFbtBuild(string modelId, string buildDescription, bool enableModelInsights, out string operationLocationHeader)
        {

            // only used if splitter strategy is set to RandomSplitter
            var randomSplitterParameters = new RandomSplitterParameters()
            {
                RandomSeed = 0,
                TestPercent = 10
            };

            var parameters = new FbtBuildParameters()
            {
                MinimalScore = 0,
                SimilarityFunction = FbtSimilarityFunction.Lift,
                SupportThreshold = 3,
                MaxItemSetSize = 2,
                EnableModelingInsights = enableModelInsights,
                SplitterStrategy = SplitterStrategy.LastEventSplitter,
                RandomSplitterParameters = randomSplitterParameters,
            };

            var requestInfo = new BuildRequestInfo()
            {
                BuildType = BuildType.Fbt,
                BuildParameters = new BuildParameters()
                {
                    Fbt = parameters,
                },
                Description = buildDescription
            };

            return BuildModel(modelId, requestInfo, out operationLocationHeader);
        }

        /// <summary>
        /// Monitor operation status and wait for completion.
        /// </summary>
        /// <param name="operationId">The operation id</param>
        /// <returns>Build status</returns>
        public OperationInfo<T> WaitForOperationCompletion<T>(string operationId)
        {
            OperationInfo<T> operationInfo;

            string uri = BaseUri + "/operations";

            while (true)
            {
                var response = _httpClient.GetAsync(uri + "/" + operationId).Result;
                var jsonString = response.Content.ReadAsStringAsync().Result;
                operationInfo = JsonConvert.DeserializeObject<OperationInfo<T>>(jsonString);

                // Operation status {NotStarted, Running, Cancelling, Cancelled, Succeded, Failed}
                Console.WriteLine(" Operation Status: {0}. \t Will check again in 10 seconds.", operationInfo.Status);

                if (OperationStatus.Succeeded.ToString().Equals(operationInfo.Status) ||
                    OperationStatus.Failed.ToString().Equals(operationInfo.Status) ||
                    OperationStatus.Cancelled.ToString().Equals(operationInfo.Status))
                {
                    break;
                }

                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
            return operationInfo;
        }

        /// <summary>
        /// Extract the operation id from the operation header
        /// </summary>
        /// <param name="operationLocationHeader"></param>
        /// <returns></returns>
        public static string GetOperationId(string operationLocationHeader)
        {
            int index = operationLocationHeader.LastIndexOf('/');
            var operationId = operationLocationHeader.Substring(index + 1);
            return operationId;
        }

        /// <summary>
        /// Set an active build for the model.
        /// </summary>
        /// <param name="modelId">Unique idenfier of the model</param>
        /// <param name="updateActiveBuildInfo"></param>
        public void SetActiveBuild(string modelId, UpdateActiveBuildInfo updateActiveBuildInfo)
        {
            string uri = BaseUri + "/models/" + modelId;
            var content = new ObjectContent<UpdateActiveBuildInfo>(updateActiveBuildInfo, new JsonMediaTypeFormatter());
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), uri) { Content = content };
            var response = _httpClient.SendAsync(request).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error HTTP Status Code");
            }
        }

        /// <summary>
        /// Get Item to Item (I2I) Recommendations or Frequently-Bought-Together (FBT) recommendations
        /// </summary>
        /// <param name="modelId">The model identifier.</param>
        /// <param name="buildId">The build identifier. Set to null if you want to use active build</param>
        /// <param name="itemIds"></param>
        /// <param name="numberOfResults"></param>
        /// <returns>
        /// The recommendation sets. Note that I2I builds will only return one item per set.
        /// FBT builds will return more than one item per set.
        /// </returns>
        public RecommendedItemSetInfoList GetRecommendations(string modelId, long? buildId, string itemIds, int numberOfResults)
        {

            string uri = BaseUri + "/models/" + modelId + "/recommend/item?itemIds=" + itemIds +
                "&numberOfResults=" + numberOfResults + "&minimalScore=0";

            //Set active build if passed.
            if (buildId != null)
            {
                uri = uri + "&buildId=" + buildId;
            }

            var response = _httpClient.GetAsync(uri).Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    String.Format("Error {0}: Failed to get recommendations for modelId {1}, buildId {2}, Reason: {3}",
                    response.StatusCode, modelId, buildId, ExtractErrorInfo(response)));
            }

            var jsonString = response.Content.ReadAsStringAsync().Result;
            var recommendedItemSetInfoList = JsonConvert.DeserializeObject<RecommendedItemSetInfoList>(jsonString);
            return recommendedItemSetInfoList;
        }

        /// <summary>
        /// Use historical transaction data to provide personalized recommendations for a user.
        /// The user history is extracted from the usage files used to train the model.
        /// </summary>
        /// <param name="modelId">The model identifier.</param>
        /// <param name="buildId">The build identifier. Set to null to use active build.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="numberOfResults">Desired number of recommendation results.</param>
        /// <returns>The recommendations for the user.</returns>
        public RecommendedItemSetInfoList GetUserRecommendations(string modelId, long? buildId, string userId, int numberOfResults)
        {
            string uri = BaseUri + "/models/" + modelId + "/recommend/user?userId=" + userId + "&numberOfResults=" + numberOfResults;

            //Set active build if passed.
            if (buildId != null)
            {
                uri = uri + "&buildId=" + buildId;
            }

            var response = _httpClient.GetAsync(uri).Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    String.Format("Error {0}: Failed to get user recommendations for modelId {1}, buildId {2}, Reason: {3}",
                    response.StatusCode, modelId, buildId, ExtractErrorInfo(response)));
            }

            var jsonString = response.Content.ReadAsStringAsync().Result;
            var recommendedItemSetInfoList = JsonConvert.DeserializeObject<RecommendedItemSetInfoList>(jsonString);
            return recommendedItemSetInfoList;
        }

        /// <summary>
        /// Update model information
        /// </summary>
        /// <param name="modelId">the id of the model</param>
        /// <param name="activeBuildId">the id of the build to be active (optional)</param>
        public void SetActiveBuild(string modelId, long activeBuildId)
        {
            var info = new UpdateActiveBuildInfo()
            {
                ActiveBuildId = activeBuildId
            };

            SetActiveBuild(modelId, info);
        }


        /// <summary>
        /// Extract error message from the httpResponse, (reason phrase + body)
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private static string ExtractErrorInfo(HttpResponseMessage response)
        {
            string detailedReason = null;
            if (response.Content != null)
            {
                detailedReason = response.Content.ReadAsStringAsync().Result;
            }
            var errorMsg = detailedReason == null ? response.ReasonPhrase : response.ReasonPhrase + "->" + detailedReason;
            return errorMsg;

        }

        /// <summary>
        /// Extract error information from HTTP response message.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        internal static string ExtractReponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsStringAsync().Result;
            }
            string detailedReason = null;
            if (response.Content != null)
            {
                detailedReason = response.Content.ReadAsStringAsync().Result;
            }
            var errorMsg = detailedReason == null ? response.ReasonPhrase : response.ReasonPhrase + "->" + detailedReason;

            string error = String.Format("Status code: {0}\nDetail information: {1}", (int)response.StatusCode, errorMsg);
            throw new Exception("Response: " + error);
        }

        /// <summary>
        /// Request the batch job
        /// </summary>
        /// <param name="batchJobsRequestInfo">The batch job request information</param>
        /// <param name="operationLocationHeader">Batch operation location</param>
        /// <returns></returns>
        public string StartBatchJob(BatchJobsRequestInfo batchJobsRequestInfo, out string operationLocationHeader)
        {
            string uri = BaseUri + "/batchjobs";
            var response = _httpClient.PostAsJsonAsync(uri, batchJobsRequestInfo).Result;
            var jsonString = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(String.Format("Error {0}: Failed to submit the batch job for model {1}, Reason: {2}",
                    response.StatusCode, batchJobsRequestInfo.Job.ModelId, ExtractErrorInfo(response)));
            }

            operationLocationHeader = response.Headers.GetValues("Operation-Location").FirstOrDefault();
            var batchJobResponse = JsonConvert.DeserializeObject<BatchJobsResponse>(jsonString);
            return batchJobResponse.BatchId;
        }
    }

    /// <summary>
    /// Utility class holding the result of import operation
    /// </summary>
    internal class ImportReport
    {
        public string Info { get; set; }
        public int ErrorCount { get; set; }
        public int LineCount { get; set; }

        public override string ToString()
        {
            return string.Format("successfully imported {0}/{1} lines for {2}", LineCount - ErrorCount, LineCount,
                Info);
        }
    }
}
