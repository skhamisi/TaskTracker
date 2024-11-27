
public class AuthenticatedHttpClientHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorageService;

    public AuthenticatedHttpClientHandler(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
        InnerHandler = new HttpClientHandler();
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _localStorageService.GetItemAsync<string>("authToken");

        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
