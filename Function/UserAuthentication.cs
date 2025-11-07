using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using CSM_UserAuthentication.Interface;
using CSM_UserAuthentication.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using CSM_UserAuthentication.Model;
using System.Text.Json;

namespace CSM_UserAuthentication.Function;

public class UserAuthentication
{
    private readonly ILogger<UserAuthentication> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly AuthRepositoryFactory authRepositoryFactory;


    public UserAuthentication(ILogger<UserAuthentication> logger,IHttpClientFactory httpClientFactory,IConfiguration configuration,AuthRepositoryFactory repositoryFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        authRepositoryFactory = repositoryFactory;
    }

    [Function("UserAuthentication")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get",Route = "activedirectory/authenticateactivedirectoryuser")] HttpRequestData req)
    {
        var query= System.Web.HttpUtility.ParseQueryString(req.Url.Query);
        var request = new ApiRequest
        {
            Domain = query["domainName"],
            Username = query["username"],
            Password = query["password"],
            AuthType = query["authtype"] ?? "lldap"
        };


        var repo = authRepositoryFactory.createfactory(request.AuthType);

        bool result = await repo.AuthenticateAsync(request);

        var responseObj = new ApiResponse
        {
            IsUserAuthenticated = result,
            Message = result ? "Authenticated Successfully" : "Authentication failed"
        };
        var statuscode = result ? HttpStatusCode.OK : HttpStatusCode.Unauthorized;

        var response = req.CreateResponse(statuscode);
        response.Headers.Add("Content-Type", "application/json");
        await response.WriteStringAsync(JsonSerializer.Serialize(responseObj));
        return response;
    }
}