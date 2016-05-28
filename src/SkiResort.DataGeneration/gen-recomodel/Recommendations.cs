using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml;

namespace gen_recomodel
{
    // This file taken from: https://datamarket.azure.com/receipt/62744063-a50e-4b8b-9ce0-352c04b14fae?ctpa=False
    // and only minimally modified

    /// <summary>
    /// Sample app to show usage of part of the cloudML recommendation API 
    /// The application will create a model container, add catalog and usage data, 
    /// trigger a recommendation model build, monitor the build execution and get recommendations 
    /// The application also show the usage of updating model information. 
    /// 
    /// The scenario above is a full loop when you don't have anything, usually you will create a container once and invoke 
    /// other API according to your need.
    ///  
    /// </summary>
    public class AzureMLRecommendations
    {
        /// <summary>
        /// Initialize the sample app
        /// </summary>
        /// <param name="username">the username (email)</param>
        /// <param name="accountKey"></param>
        public void Init(string username, string accountKey)
        {
            _httpClient = new HttpClient();
            var pass = GeneratePass(username, accountKey);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", pass);
            _httpClient.BaseAddress = new Uri(RootUri);
        }

        /// <summary>
        /// create the model with the given name.
        /// </summary>
        /// <returns>The model id</returns>
        public string CreateModel(string modelName)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, String.Format(Uris.CreateModelUrl, modelName));
            var response = _httpClient.SendAsync(request).Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(String.Format("Error {0}: Failed to create model {1}, \n reason {2}",
                    response.StatusCode, modelName, ExtractErrorInfo(response)));
            }

            //process response if success
            string modelId = null;

            var node = XmlUtils.ExtractXmlElement(response.Content.ReadAsStreamAsync().Result, "//a:entry/a:content/m:properties/d:Id");
            if (node != null)
                modelId = node.InnerText;

            return modelId;
        }

        /// <summary>
        /// Extract error message from the httpResponse, (reason phrase + body)
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private static string ExtractErrorInfo(HttpResponseMessage response)
        {
            //DM send the error message in body so need to extract the info from there
            string detailedReason = null;
            if (response.Content != null)
            {
                detailedReason = response.Content.ReadAsStringAsync().Result;
            }
            var errorMsg = detailedReason == null ? response.ReasonPhrase : response.ReasonPhrase + "->" + detailedReason;
            return errorMsg;

        }


        /// <summary>
        /// Trigger a recommendation build for the given model.
        /// Note: unless configured otherwise the u2i (user to item/user based) recommendations are enabled too.
        /// </summary>
        /// <param name="modelId">the model id</param>
        /// <param name="buildDescription">a description for the build</param>
        /// <returns>the id of the triggered build</returns>
        public string BuildModel(string modelId, string buildDescription)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, String.Format(Uris.BuildModel, modelId, buildDescription));

            //setup the build parameters here we use a simple build without feature usage, for a complete list and 
            //explanation check the API document AT
            //http://azure.microsoft.com/en-us/documentation/articles/machine-learning-recommendation-api-documentation/#1113-recommendation-build-parameters
            request.Content = new StringContent("<BuildParametersList>" +
                                                "<NumberOfModelIterations>10</NumberOfModelIterations>" +
                                                "<NumberOfModelDimensions>20</NumberOfModelDimensions>" +
                                                "<ItemCutOffLowerBound>1</ItemCutOffLowerBound>" +
                                                "<EnableModelingInsights>false</EnableModelingInsights>" +
                                                "<EnableU2I>true</EnableU2I>" +
                                                "<UseFeaturesInModel>false</UseFeaturesInModel>" +
                                                "<ModelingFeatureList></ModelingFeatureList>" +
                                                "<AllowColdItemPlacement>true</AllowColdItemPlacement>" +
                                                "<EnableFeatureCorrelation>false</EnableFeatureCorrelation>" +
                                                "<ReasoningFeatureList></ReasoningFeatureList>" +
                                                "</BuildParametersList>", Encoding.UTF8, "Application/xml");
            var response = _httpClient.SendAsync(request).Result;


            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(String.Format("Error {0}: Failed to start build for model {1}, \n reason {2}",
                    response.StatusCode, modelId, ExtractErrorInfo(response)));
            }
            string buildId = null;
            //process response if success
            var node = XmlUtils.ExtractXmlElement(response.Content.ReadAsStreamAsync().Result, "//a:entry/a:content/m:properties/d:Id");
            if (node != null)
                buildId = node.InnerText;

            return buildId;

        }

        /// <summary>
        /// Retrieve the build status for the given build
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="buildId"></param>
        /// <returns></returns>
        public BuildStatus GetBuildStatus(string modelId, string buildId)
        {

            var request = new HttpRequestMessage(HttpMethod.Get, String.Format(Uris.BuildStatuses, modelId, false));
            var response = _httpClient.SendAsync(request).Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(String.Format("Error {0}: Failed to retrieve build for status for model {1} and build id {2}, \n reason {3}",
                    response.StatusCode, modelId, buildId, ExtractErrorInfo(response)));
            }
            string buildStatusStr = null;
            var node = XmlUtils.ExtractXmlElement(response.Content.ReadAsStreamAsync().Result, string.Format("//a:entry/a:content/m:properties[d:BuildId='{0}']/d:Status", buildId));
            if (node != null)
                buildStatusStr = node.InnerText;

            BuildStatus buildStatus;
            if (!Enum.TryParse(buildStatusStr, true, out buildStatus))
            {
                throw new Exception(string.Format("Failed to parse build status for value {0} of build {1} for model {2}", buildStatusStr, buildId, modelId));
            }

            return buildStatus;
        }


        /// <summary>
        /// Update model information
        /// </summary>
        /// <param name="modelId">the id of the model</param>
        /// <param name="description">the model description (optional)</param>
        /// <param name="activeBuildId">the id of the build to be active (optional)</param>
        public void UpdateModel(string modelId, string description, string activeBuildId)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, String.Format(Uris.UpdateModel, modelId));

            var sb = new StringBuilder("<ModelUpdateParams xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
            if (!string.IsNullOrEmpty(description))
            {
                sb.AppendFormat("<Description>{0}</Description>", description);
            }
            if (!string.IsNullOrEmpty(activeBuildId))
            {
                sb.AppendFormat("<ActiveBuildId>{0}</ActiveBuildId>", activeBuildId);
            }
            sb.Append("</ModelUpdateParams>");

            request.Content = new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString())));
            request.Content.Headers.Add("Content-Type", "Application/xml");

            var response = _httpClient.SendAsync(request).Result;


            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(String.Format("Error {0}: Failed to update model for model {1}, \n reason {2}",
                    response.StatusCode, modelId, ExtractErrorInfo(response)));
            }

        }
        /// <summary>
        /// Retrieve recommendation for the given item(s)
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="itemIdList"></param>
        /// <param name="numberOfResult">the number of result to include in the response</param>
        /// <param name="includeMetadata">true, means meta data will be returned too</param>
        /// <returns>a collection of recommended items</returns>
        public IEnumerable<RecommendedItem> GetRecommendation(string modelId, List<string> itemIdList, int numberOfResult,
            bool includeMetadata = false)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                String.Format(Uris.GetRecommendation, modelId, string.Join(",", itemIdList), numberOfResult,
                    includeMetadata));
            var response = _httpClient.SendAsync(request).Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    String.Format(
                        "Error {0}: Failed to retrieve recommendation for item list {1} and model {2}, \n reason {3}",
                        response.StatusCode, string.Join(",", itemIdList), modelId, ExtractErrorInfo(response)));
            }

            var nodeList = XmlUtils.ExtractXmlElementList(response.Content.ReadAsStreamAsync().Result, "//a:entry/a:content/m:properties");

            return XmlUtils.ExtractRecommendedItems(nodeList);
        }


        /// <summary>
        /// Invoke some recommendation on 
        /// </summary>
        /// <param name="modelId">the model id</param>
        /// <param name="seedItems">a list of item to get recommendation</param>
        /// <param name="useList">if true all the item are used to get a recommendation on the whole set,
        ///  if false use each item of the list to get resommendations</param>
        private void InvokeRecommendations(string modelId, List<CatalogItem> seedItems, bool useList)
        {
            if (useList)
            {
                var recoItems = GetRecommendation(modelId, seedItems.Select(i => i.Id).ToList(), 10);
                Console.WriteLine("\tRecommendations for [{0}]", string.Join("],[", seedItems));
                foreach (var recommendedItem in recoItems)
                {
                    Console.WriteLine("\t  {0}", recommendedItem);
                }
            }
            else
            {
                foreach (var item in seedItems)
                {
                    var recoItems = GetRecommendation(modelId, new List<string> { item.Id }, 10);
                    Console.WriteLine("Recommendation for '{0}'", item);
                    foreach (var recommendedItem in recoItems)
                    {
                        Console.WriteLine("\t  {0}", recommendedItem);
                    }
                    Console.WriteLine("\n");
                }
            }
        }
        /// <summary>
        /// call the u2i recommendation. 
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="userId"></param>
        private void InvokeUserRecommendations(string modelId, string userId)
        {
            var recoItems = GetUserRecommendation(modelId, userId, 10);
            Console.WriteLine("Recommendation for user '{0}'", userId);
            foreach (var recommendedItem in recoItems)
            {
                Console.WriteLine("\t  {0}", recommendedItem);
            }
            Console.WriteLine("\n");
        }


        /// <summary>
        /// Retrieve recommendation for the given user, this is possible as long as the EnableU2I parameter (which is enabled by default) 
        /// was not disable when triggering a recommendation build. The system will automatically fetch the user history and recommend items
        /// according to his history.
        /// More complex scenario is also to use item in conjuction of the history, meaning that you request recommendation for the given user
        /// and a set of item that he currently chooses.
        /// </summary>
        /// <param name="modelId">the model id from which the recommendations are requested</param>
        /// <param name="userId">the id of the user for which the recommendations are requested</param>
        /// <param name="numberOfResult">the number of result to include in the response</param>
        /// <param name="includeMetadata">true, means meta data will be returned too</param>
        /// <returns>a collection of recommended items</returns>
        public IEnumerable<RecommendedItem> GetUserRecommendation(string modelId, string userId, int numberOfResult,
            bool includeMetadata = false)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                String.Format(Uris.GetUserRecommendation, modelId, userId, numberOfResult, includeMetadata));

            var response = _httpClient.SendAsync(request).Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    String.Format(
                        "Error {0}: Failed to retrieve recommendation for user id {1} and model {2}, \n reason {3}",
                        response.StatusCode, userId, modelId, ExtractErrorInfo(response)));
            }

            var nodeList = XmlUtils.ExtractXmlElementList(response.Content.ReadAsStreamAsync().Result, "//a:entry/a:content/m:properties");

            return XmlUtils.ExtractRecommendedItems(nodeList);
        }


        /// <summary>
        /// Generate the key to allow accessing DM API
        /// </summary>
        /// <param name="email">the user email</param>
        /// <param name="accountKey">the user account key</param>
        /// <returns></returns>
        private string GeneratePass(string email, string accountKey)
        {
            var byteArray = Encoding.ASCII.GetBytes(string.Format("{0}:{1}", email, accountKey));
            return Convert.ToBase64String(byteArray);
        }

        /// <summary>
        /// Import the given file (catalog/usage) to the given model. 
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="filePath"></param>
        /// <param name="importUri"></param>
        /// <returns></returns>
        public ImportReport ImportFile(string modelId, string filePath, string importUri)
        {
            var filestream = new FileStream(filePath, FileMode.Open);
            var fileName = Path.GetFileName(filePath);
            return ImportFile(modelId, fileName, filestream, importUri);
        }

        public ImportReport ImportFile(string modelId, string fileName, Stream stream, string importUri)
        { 
            var request = new HttpRequestMessage(HttpMethod.Post, String.Format(importUri, modelId, fileName));

            request.Content = new StreamContent(stream);
            var response = _httpClient.SendAsync(request).Result;


            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    String.Format("Error {0}: Failed to import file {1}, for model {2} \n reason {3}",
                        response.StatusCode, fileName, modelId, ExtractErrorInfo(response)));
            }

            //process response if success
            var nodeList = XmlUtils.ExtractXmlElementList(response.Content.ReadAsStreamAsync().Result,
                "//a:entry/a:content/m:properties/*");

            var report = new ImportReport { Info = fileName };
            foreach (XmlNode node in nodeList)
            {
                if ("LineCount".Equals(node.LocalName))
                {
                    report.LineCount = int.Parse(node.InnerText);
                }
                if ("ErrorCount".Equals(node.LocalName))
                {
                    report.ErrorCount = int.Parse(node.InnerText);
                }
            }
            return report;
        }

        private HttpClient _httpClient;

        private const string RootUri = "https://api.datamarket.azure.com/amla/recommendations/v3/";


        /// <summary>
        /// holds the API uri.
        /// </summary>
        public static class Uris
        {

            public const string CreateModelUrl = "CreateModel?modelName=%27{0}%27&apiVersion=%271.0%27";

            public const string ImportCatalog =
                "ImportCatalogFile?modelId=%27{0}%27&filename=%27{1}%27&apiVersion=%271.0%27";


            public const string ImportUsage =
                "ImportUsageFile?modelId=%27{0}%27&filename=%27{1}%27&apiVersion=%271.0%27";

            public const string BuildModel =
                "BuildModel?modelId=%27{0}%27&userDescription=%27{1}%27&apiVersion=%271.0%27";

            public const string BuildStatuses = "GetModelBuildsStatus?modelId=%27{0}%27&onlyLastBuild={1}&apiVersion=%271.0%27";

            public const string GetRecommendation =
                "ItemRecommend?modelId=%27{0}%27&itemIds=%27{1}%27&numberOfResults={2}&includeMetadata={3}&apiVersion=%271.0%27";

            public const string GetUserRecommendation =
                "UserRecommend?modelId=%27{0}%27&userId=%27{1}%27&numberOfResults={2}&includeMetadata={3}&apiVersion=%271.0%27";

            public const string UpdateModel = "UpdateModel?id=%27{0}%27&apiVersion=%271.0%27";

        }

        /// <summary>
        /// represent the build status
        /// </summary>
        public enum BuildStatus
        {
            Create,
            Queued,
            Building,
            Success,
            Error,
            Cancelling,
            Cancelled
        }

        /// <summary>
        /// Utility class holding the result of import operation
        /// </summary>
        public class ImportReport
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

        /// <summary>
        /// hold catalog item info  (partial)
        /// </summary>
        public class CatalogItem
        {
            public string Id { get; set; }
            public string Name { get; set; }

            public override string ToString()
            {
                return string.Format("Id: {0}, Name: {1}", Id, Name);
            }
        }

        /// <summary>
        /// Utility class holding a recommended item information.
        /// </summary>
        public class RecommendedItem
        {
            public string Name { get; set; }
            public string Rating { get; set; }
            public string Reasoning { get; set; }
            public string Id { get; set; }

            public override string ToString()
            {
                return string.Format("Name: {0}, Id: {1}, Rating: {2}, Reasoning: {3}", Name, Id, Rating, Reasoning);
            }
        }


        private class XmlUtils
        {
            /// <summary>
            /// extract a single xml node from the given stream, given by the xPath
            /// </summary>
            /// <param name="xmlStream"></param>
            /// <param name="xPath"></param>
            /// <returns></returns>
            internal static XmlNode ExtractXmlElement(Stream xmlStream, string xPath)
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlStream);
                //Create namespace manager
                var nsmgr = CreateNamespaceManager(xmlDoc);

                var node = xmlDoc.SelectSingleNode(xPath, nsmgr);
                return node;
            }

            private static XmlNamespaceManager CreateNamespaceManager(XmlDocument xmlDoc)
            {
                var nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
                nsmgr.AddNamespace("a", "http://www.w3.org/2005/Atom");
                nsmgr.AddNamespace("m", "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata");
                nsmgr.AddNamespace("d", "http://schemas.microsoft.com/ado/2007/08/dataservices");
                return nsmgr;
            }

            /// <summary>
            /// extract the xml nodes from the given stream, given by the xPath
            /// </summary>
            /// <param name="xmlStream"></param>
            /// <param name="xPath"></param>
            /// <returns></returns>
            internal static XmlNodeList ExtractXmlElementList(Stream xmlStream, string xPath)
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlStream);
                var nsmgr = CreateNamespaceManager(xmlDoc);
                var nodeList = xmlDoc.SelectNodes(xPath, nsmgr);
                return nodeList;
            }

            /// <summary>
            /// Utility method to extract the recommended item from a xml recommendation result 
            /// </summary>
            /// <param name="nodeList">the xml element containing the recommended items.</param>
            /// <returns>a collection of recommended item or empty list id sh</returns>
            internal static IEnumerable<RecommendedItem> ExtractRecommendedItems(XmlNodeList nodeList)
            {
                var recoList = new List<RecommendedItem>();
                foreach (var node in (nodeList))
                {
                    var item = new RecommendedItem();
                    //cycle through the recommended items
                    foreach (var child in ((XmlElement)node).ChildNodes)
                    {
                        //cycle through properties
                        var nodeName = ((XmlNode)child).LocalName;
                        switch (nodeName)
                        {
                            case "Id":
                                item.Id = ((XmlNode)child).InnerText;
                                break;
                            case "Name":
                                item.Name = ((XmlNode)child).InnerText;
                                break;
                            case "Rating":
                                item.Rating = ((XmlNode)child).InnerText;
                                break;
                            case "Reasoning":
                                item.Reasoning = ((XmlNode)child).InnerText;
                                break;
                        }
                    }
                    recoList.Add(item);
                }
                return recoList;
            }
        }
    }
}
