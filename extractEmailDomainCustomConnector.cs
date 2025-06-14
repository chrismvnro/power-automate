public class Script : ScriptBase
{
public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        // Check if the operation ID matches what is specified in the OpenAPI definition of the connector
        switch (this.Context.OperationId)
        {
            case "extractEmailDomain":
            return await this.HandleExtractDomainRegexOperation().ConfigureAwait(false);
            break;
        }

        // Handle an invalid operation ID
        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
        response.Content = new StringContent("{\"error\": \"Unknown operation ID\"}", System.Text.Encoding.UTF8, "application/json");
        return response;
    }

private async Task<HttpResponseMessage> HandleExtractDomainRegexOperation()
    {
        var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var contentAsJson = JObject.Parse(contentAsString);
        string email = (string)contentAsJson["emailaddress"];
     

        int indexOfAt = email.IndexOf('@');
        string domain = email.Substring(indexOfAt + 1);


        JObject output = new JObject
        {
            ["originalEmailAddress"] = email,
            ["emailDomain"] = domain
        };

        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
        response.Content = CreateJsonContent(output.ToString());
        return response;
    }

    

    private HttpResponseMessage CreateErrorResponse(HttpStatusCode statusCode, string errorMessage)
    {
        HttpResponseMessage errorResponse = new HttpResponseMessage(statusCode);
        errorResponse.Content = new StringContent($"{{\"error\": \"{errorMessage}\"}}", System.Text.Encoding.UTF8, "application/json");
        return errorResponse;
    }

}
