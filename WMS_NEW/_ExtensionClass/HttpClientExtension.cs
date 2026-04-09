using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;


public class HttpClientExtension : IDisposable
{

    #region IDisposable Members

    public void Dispose()
    {
        if (client != null)
            client.Dispose();

        GC.SuppressFinalize(this);
    }

    #endregion



    const string FORMAT_TYPE = "application/json";

    HttpClient client = null;
    HttpResponseMessage response;

    public void CreateHttpClient(string _requestAddr)
    {
        CreateHttpClient(_requestAddr, null, null);
    }
    public void CreateHttpClient(string _requestAddr, string _authScheme, string _authCode)
    {
        if (client != null)
            client = null;

        client = new HttpClient();
        client.BaseAddress = new Uri(_requestAddr);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(FORMAT_TYPE));

        if (!string.IsNullOrEmpty(_authScheme) && !string.IsNullOrEmpty(_authCode))
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_authScheme, _authCode);
    }

    public TResult PostByJson<TRequest, TResult>(string _requestURI, TRequest _content)
    {
        try
        {
            var content = JsonConvert.SerializeObject(_content);
            var req_content = new StringContent(content, System.Text.Encoding.UTF8, FORMAT_TYPE);

            response = client.PostAsync(_requestURI, req_content).Result;
            if (response.IsSuccessStatusCode) //Code = 200
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<TResult>(data);

                return result;
            }
            else //Code = Other
            {
                throw new HttpRequestException(response.ReasonPhrase);
            }
        }
        catch (HttpRequestException httpEx)
        {
            throw httpEx;
        }
    }
}