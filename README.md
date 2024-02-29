# Geoapify Autocomplete Proxy

This is an AWS Lambda function that acts as a proxy for the [Geoapify Autocomplete API](https://apidocs.geoapify.com/docs/geocoding/address-autocomplete/#autocomplete). This is a unifunctional solution that can be used to hide the API key from the client. It is written in C# - which is especially notable because this is the first thing I have ever written in C#!

## Requests

The function is deployed to AWS Lambda and is triggered via a public Function URL.

- Function URL: https://yj2r6zi3kfusl5di2itd2wqxqu0quclp.lambda-url.us-east-2.on.aws
- Method: GET
- One query parameter `a` (the search query)

## Responses

The function returns the response from the Geoapify Autocomplete API:

```typescript
{
	"features": [
		{
			"properties": {
				"country": string,
				"country_code": string,
				"region": string,
				"state": string,
				"county": string,
				"city": string,
				"municipality": string,
				"postcode": string,
				"district": string,
				"datasource": {
					"sourcename": string,
					"attribution": string,
					"license": string,
					"url": string
				},
				"state_code": string,
				"result_type": string,
				"lon": number,
				"lat": number,
				"parent_as_place_id": boolean,
				"formatted": string,
				"address_line1": string,
				"address_line2": string,
				"category": string,
				"timezone": {
					"name": string,
					"offset_STD": string,
					"offset_STD_seconds": number,
					"offset_DST": string,
					"offset_DST_seconds": number
				},
				"plus_code": string,
				"plus_code_short": string,
				"rank": {
					"confidence": number,
					"confidence_city_level": number,
					"match_type": string
				},
				"place_id": string
			}
		}, ...
	]
}
```