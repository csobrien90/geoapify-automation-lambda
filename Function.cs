using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace geoapify_autocomplete;

public class Function
{
    public string FunctionHandler(Event input, ILambdaContext context)
	{
		// Start the log with the function name
		LambdaLogger.Log($"Calling function name: {context.FunctionName}\n");

		// Get GEOAPIFY_API_KEY from environment variables
		var geoapifyApiKey = Environment.GetEnvironmentVariable("GEOAPIFY_API_KEY");
		if (string.IsNullOrEmpty(geoapifyApiKey))
		{
			throw new ArgumentNullException("GEOAPIFY_API_KEY is required");
		}

		// Get query string parameters from raw query string ("a=123+Fake+St")
		var queryStringParameters = input.rawQueryString;
		LambdaLogger.Log($"queryStringParameters: {queryStringParameters}\n");

		// Get the address from the query string parameters
		var address = queryStringParameters.Split("=")[1];

		// Validate the address
		if (string.IsNullOrEmpty(address))
		{
			throw new ArgumentNullException("Address is required");
		}

		// Parse the address from URL encoding to a string
		address = System.Web.HttpUtility.UrlDecode(address);
		LambdaLogger.Log($"address: {address}\n");

		// Build the URL for the Geoapify API
		var url = new Uri("https://api.geoapify.com/v1/geocode/autocomplete");
		var urlBuilder = new UriBuilder(url);
		var query = System.Web.HttpUtility.ParseQueryString(urlBuilder.Query);
		query["text"] = address;
		query["apiKey"] = geoapifyApiKey;
		query["filter"] = "countrycode:us";
		query["limit"] = "20";
		urlBuilder.Query = query.ToString();
		url = new Uri(urlBuilder.ToString());
		LambdaLogger.Log($"url: {url}\n");

		// Fetch the location data from the Geoapify API
		var response = new HttpClient().GetAsync(url).Result;
		response.EnsureSuccessStatusCode();
		var data = response.Content.ReadAsStringAsync().Result;
		LambdaLogger.Log($"data: {data}\n");

		// Return data as a JSON string
		return data;
	}
}
