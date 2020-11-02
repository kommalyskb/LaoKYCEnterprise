using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shared.Configs;
using Shared.DTOs;
using Shared.Entities;
using Shared.Helpers;
using Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Portal.Repositories
{
    public class APILaoKYC : IAPILaoKYC
    {
        private JsonSerializerOptions defaultOptions => new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        public async Task<bool> CreateApiResource(ApiResourceApiDto resourceApiDto)
        {
            // discover endpoints from metadata
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(IdentityEndpoint.Discovery);
            if (disco.IsError)
            {
                return false;
            }

            // request token
            var req = new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = IdentityEndpoint.ClientID,
                ClientSecret = IdentityEndpoint.Secret,
                Scope = IdentityEndpoint.Scopes,
                UserName = IdentityEndpoint.UserName,
                Password = IdentityEndpoint.Password
            };
            var tokenResponse = await client.RequestPasswordTokenAsync(req);

            if (tokenResponse.IsError)
            {
                return false;
            }

            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var dataJson = JsonSerializer.Serialize(resourceApiDto);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");

            var response = await apiClient.PostAsync(IdentityEndpoint.ResourceUri, stringContent);

            if (response.IsSuccessStatusCode)
            {
                var resDeserialize = await JsonHelper.Deserialize<ApiResourceApiDto>(response, defaultOptions);
                resourceApiDto.Id = resDeserialize.Id;
            }
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CreateClient(ClientApiDto clientApi)
        {
            // discover endpoints from metadata
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(IdentityEndpoint.Discovery);
            if (disco.IsError)
            {
                return false;
            }

            // request token
            var req = new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = IdentityEndpoint.ClientID,
                ClientSecret = IdentityEndpoint.Secret,
                Scope = IdentityEndpoint.Scopes,
                UserName = IdentityEndpoint.UserName,
                Password = IdentityEndpoint.Password
            };
            var tokenResponse = await client.RequestPasswordTokenAsync(req);

            if (tokenResponse.IsError)
            {
                return false;
            }

            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var dataJson = JsonSerializer.Serialize(clientApi);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");

            var response = await apiClient.PostAsync(IdentityEndpoint.ClientUri, stringContent);

            if (response.IsSuccessStatusCode)
            {
                var resDeserialize = await JsonHelper.Deserialize<ClientApiDto>(response, defaultOptions);
                clientApi.Id = resDeserialize.Id;
            }
            return response.IsSuccessStatusCode;

        }

        public async Task<bool> CreateClientClaim(int? id, ClientClaimApiDto claim)
        {
            // discover endpoints from metadata
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(IdentityEndpoint.Discovery);
            if (disco.IsError)
            {
                return false;
            }

            // request token
            var req = new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = IdentityEndpoint.ClientID,
                ClientSecret = IdentityEndpoint.Secret,
                Scope = IdentityEndpoint.Scopes,
                UserName = IdentityEndpoint.UserName,
                Password = IdentityEndpoint.Password
            };
            var tokenResponse = await client.RequestPasswordTokenAsync(req);

            if (tokenResponse.IsError)
            {
                return false;
            }

            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var dataJson = JsonSerializer.Serialize(claim);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");

            var response = await apiClient.PostAsync(IdentityEndpoint.ClientUri + $"/{id.Value}/Claims", stringContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CreateClientProperty(int? id, ClientPropertyApiDto property)
        {
            // discover endpoints from metadata
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(IdentityEndpoint.Discovery);
            if (disco.IsError)
            {
                return false;
            }

            // request token
            var req = new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = IdentityEndpoint.ClientID,
                ClientSecret = IdentityEndpoint.Secret,
                Scope = IdentityEndpoint.Scopes,
                UserName = IdentityEndpoint.UserName,
                Password = IdentityEndpoint.Password
            };
            var tokenResponse = await client.RequestPasswordTokenAsync(req);

            if (tokenResponse.IsError)
            {
                return false;
            }

            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var dataJson = JsonSerializer.Serialize(property);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");

            var response = await apiClient.PostAsync(IdentityEndpoint.ClientUri + $"/{id.Value}/Properties", stringContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CreateClientSecret(int? id, ClientSecretDto secret)
        {
            // discover endpoints from metadata
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(IdentityEndpoint.Discovery);
            if (disco.IsError)
            {
                return false;
            }

            // request token
            var req = new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = IdentityEndpoint.ClientID,
                ClientSecret = IdentityEndpoint.Secret,
                Scope = IdentityEndpoint.Scopes,
                UserName = IdentityEndpoint.UserName,
                Password = IdentityEndpoint.Password
            };
            var tokenResponse = await client.RequestPasswordTokenAsync(req);

            if (tokenResponse.IsError)
            {
                return false;
            }

            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var dataJson = JsonSerializer.Serialize(secret);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");

            var response = await apiClient.PostAsync(IdentityEndpoint.ClientUri + $"/{id.Value}/Secrets", stringContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CreateResourceProperty(int? id, ApiResourcePropertyApiDto property)
        {
            // discover endpoints from metadata
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(IdentityEndpoint.Discovery);
            if (disco.IsError)
            {
                return false;
            }

            // request token
            var req = new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = IdentityEndpoint.ClientID,
                ClientSecret = IdentityEndpoint.Secret,
                Scope = IdentityEndpoint.Scopes,
                UserName = IdentityEndpoint.UserName,
                Password = IdentityEndpoint.Password
            };
            var tokenResponse = await client.RequestPasswordTokenAsync(req);

            if (tokenResponse.IsError)
            {
                return false;
            }

            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var dataJson = JsonSerializer.Serialize(property);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");

            var response = await apiClient.PostAsync(IdentityEndpoint.ResourceUri + $"/{id.Value}/Properties", stringContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CreateResourceScope(int? id, ApiScopeApiDto scope)
        {
            // discover endpoints from metadata
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(IdentityEndpoint.Discovery);
            if (disco.IsError)
            {
                return false;
            }

            // request token
            var req = new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = IdentityEndpoint.ClientID,
                ClientSecret = IdentityEndpoint.Secret,
                Scope = IdentityEndpoint.Scopes,
                UserName = IdentityEndpoint.UserName,
                Password = IdentityEndpoint.Password
            };
            var tokenResponse = await client.RequestPasswordTokenAsync(req);

            if (tokenResponse.IsError)
            {
                return false;
            }

            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var dataJson = JsonSerializer.Serialize(scope);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");

            var response = await apiClient.PostAsync(IdentityEndpoint.ResourceUri + $"/{id.Value}/Scopes", stringContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CreateResourceSecret(int? id, ApiSecretApiDto secret)
        {
            // discover endpoints from metadata
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(IdentityEndpoint.Discovery);
            if (disco.IsError)
            {
                return false;
            }

            // request token
            var req = new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = IdentityEndpoint.ClientID,
                ClientSecret = IdentityEndpoint.Secret,
                Scope = IdentityEndpoint.Scopes,
                UserName = IdentityEndpoint.UserName,
                Password = IdentityEndpoint.Password
            };
            var tokenResponse = await client.RequestPasswordTokenAsync(req);

            if (tokenResponse.IsError)
            {
                return false;
            }

            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var dataJson = JsonSerializer.Serialize(secret);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");

            var response = await apiClient.PostAsync(IdentityEndpoint.ResourceUri + $"/{id.Value}/Secrets", stringContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<APIResource> QueryAPI(int? id)
        {
            // discover endpoints from metadata
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(IdentityEndpoint.Discovery);
            if (disco.IsError)
            {
                return new APIResource();
            }

            // request token
            var req = new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = IdentityEndpoint.ClientID,
                ClientSecret = IdentityEndpoint.Secret,
                Scope = IdentityEndpoint.Scopes,
                UserName = IdentityEndpoint.UserName,
                Password = IdentityEndpoint.Password
            };
            var tokenResponse = await client.RequestPasswordTokenAsync(req);

            if (tokenResponse.IsError)
            {
                return new APIResource();
            }

            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.GetAsync($"{IdentityEndpoint.ResourceUri}/{id}");

            if (response.IsSuccessStatusCode)
            {
                var resDeserialize = await JsonHelper.Deserialize<APIResource>(response, defaultOptions);
                return resDeserialize;
            }
            return new APIResource();
        }

        public async Task<ClientApiDto> QueryClient(int? id)
        {
            // discover endpoints from metadata
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(IdentityEndpoint.Discovery);
            if (disco.IsError)
            {
                return new ClientApiDto();
            }

            // request token
            var req = new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = IdentityEndpoint.ClientID,
                ClientSecret = IdentityEndpoint.Secret,
                Scope = IdentityEndpoint.Scopes,
                UserName = IdentityEndpoint.UserName,
                Password = IdentityEndpoint.Password
            };
            var tokenResponse = await client.RequestPasswordTokenAsync(req);

            if (tokenResponse.IsError)
            {
                return new ClientApiDto();
            }

            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.GetAsync($"{IdentityEndpoint.ClientUri}/{id}");

            if (response.IsSuccessStatusCode)
            {
                var resDeserialize = await JsonHelper.Deserialize<ClientApiDto>(response, defaultOptions);
                return resDeserialize;
            }
            return new ClientApiDto();
        }

        public async Task<IdentityResourcesDto> QueryIdentityResource(string searchText, int? page, int? pageSize)
        {
            // discover endpoints from metadata
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(IdentityEndpoint.Discovery);
            if (disco.IsError)
            {
                return new IdentityResourcesDto();
            }

            // request token
            var req = new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = IdentityEndpoint.ClientID,
                ClientSecret = IdentityEndpoint.Secret,
                Scope = IdentityEndpoint.Scopes,
                UserName = IdentityEndpoint.UserName,
                Password = IdentityEndpoint.Password
            };
            var tokenResponse = await client.RequestPasswordTokenAsync(req);

            if (tokenResponse.IsError)
            {
                return new IdentityResourcesDto();
            }

            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            string Uri = "";
            if (searchText != "")
            {
                Uri = $"{IdentityEndpoint.IdentityResourceUri}?searchText={searchText}&page={page}&pageSize={pageSize}";
            }
            else
            {
                Uri = $"{IdentityEndpoint.IdentityResourceUri}?page={page}&pageSize={pageSize}";
            }

            var response = await apiClient.GetAsync(Uri);

            if (response.IsSuccessStatusCode)
            {
                var resDeserialize = await JsonHelper.Deserialize<IdentityResourcesDto>(response, defaultOptions);
                return resDeserialize;
            }
            return new IdentityResourcesDto();
        }

        public async Task<bool> RemoveClient(int? id)
        {
            // discover endpoints from metadata
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(IdentityEndpoint.Discovery);
            if (disco.IsError)
            {
                return false;
            }

            // request token
            var req = new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = IdentityEndpoint.ClientID,
                ClientSecret = IdentityEndpoint.Secret,
                Scope = IdentityEndpoint.Scopes,
                UserName = IdentityEndpoint.UserName,
                Password = IdentityEndpoint.Password
            };
            var tokenResponse = await client.RequestPasswordTokenAsync(req);

            if (tokenResponse.IsError)
            {
                return false;
            }

            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.DeleteAsync(IdentityEndpoint.ClientUri + $"/{id.Value}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveClientClaim(int? id)
        {
            // discover endpoints from metadata
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(IdentityEndpoint.Discovery);
            if (disco.IsError)
            {
                return false;
            }

            // request token
            var req = new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = IdentityEndpoint.ClientID,
                ClientSecret = IdentityEndpoint.Secret,
                Scope = IdentityEndpoint.Scopes,
                UserName = IdentityEndpoint.UserName,
                Password = IdentityEndpoint.Password
            };
            var tokenResponse = await client.RequestPasswordTokenAsync(req);

            if (tokenResponse.IsError)
            {
                return false;
            }

            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.DeleteAsync(IdentityEndpoint.ClientUri + $"/Claims/{id.Value}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveClientProperty(int? id)
        {
            // discover endpoints from metadata
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(IdentityEndpoint.Discovery);
            if (disco.IsError)
            {
                return false;
            }

            // request token
            var req = new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = IdentityEndpoint.ClientID,
                ClientSecret = IdentityEndpoint.Secret,
                Scope = IdentityEndpoint.Scopes,
                UserName = IdentityEndpoint.UserName,
                Password = IdentityEndpoint.Password
            };
            var tokenResponse = await client.RequestPasswordTokenAsync(req);

            if (tokenResponse.IsError)
            {
                return false;
            }

            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.DeleteAsync(IdentityEndpoint.ClientUri + $"/Properties/{id.Value}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveClientSecret(int? id)
        {
            // discover endpoints from metadata
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(IdentityEndpoint.Discovery);
            if (disco.IsError)
            {
                return false;
            }

            // request token
            var req = new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = IdentityEndpoint.ClientID,
                ClientSecret = IdentityEndpoint.Secret,
                Scope = IdentityEndpoint.Scopes,
                UserName = IdentityEndpoint.UserName,
                Password = IdentityEndpoint.Password
            };
            var tokenResponse = await client.RequestPasswordTokenAsync(req);

            if (tokenResponse.IsError)
            {
                return false;
            }

            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.DeleteAsync(IdentityEndpoint.ClientUri + $"/Secrets/{id.Value}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveResource(int? id)
        {
            // discover endpoints from metadata
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(IdentityEndpoint.Discovery);
            if (disco.IsError)
            {
                return false;
            }

            // request token
            var req = new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = IdentityEndpoint.ClientID,
                ClientSecret = IdentityEndpoint.Secret,
                Scope = IdentityEndpoint.Scopes,
                UserName = IdentityEndpoint.UserName,
                Password = IdentityEndpoint.Password
            };
            var tokenResponse = await client.RequestPasswordTokenAsync(req);

            if (tokenResponse.IsError)
            {
                return false;
            }

            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.DeleteAsync(IdentityEndpoint.ResourceUri + $"/{id.Value}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveResourceProperty(int? id)
        {
            // discover endpoints from metadata
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(IdentityEndpoint.Discovery);
            if (disco.IsError)
            {
                return false;
            }

            // request token
            var req = new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = IdentityEndpoint.ClientID,
                ClientSecret = IdentityEndpoint.Secret,
                Scope = IdentityEndpoint.Scopes,
                UserName = IdentityEndpoint.UserName,
                Password = IdentityEndpoint.Password
            };
            var tokenResponse = await client.RequestPasswordTokenAsync(req);

            if (tokenResponse.IsError)
            {
                return false;
            }

            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.DeleteAsync(IdentityEndpoint.ResourceUri + $"/Properties/{id.Value}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveResourceScope(int? id, int? scopeid)
        {
            // discover endpoints from metadata
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(IdentityEndpoint.Discovery);
            if (disco.IsError)
            {
                return false;
            }

            // request token
            var req = new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = IdentityEndpoint.ClientID,
                ClientSecret = IdentityEndpoint.Secret,
                Scope = IdentityEndpoint.Scopes,
                UserName = IdentityEndpoint.UserName,
                Password = IdentityEndpoint.Password
            };
            var tokenResponse = await client.RequestPasswordTokenAsync(req);

            if (tokenResponse.IsError)
            {
                return false;
            }

            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.DeleteAsync(IdentityEndpoint.ResourceUri + $"/{id.Value}/Scopes/{scopeid.Value}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveResourceSecret(int? id)
        {
            // discover endpoints from metadata
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(IdentityEndpoint.Discovery);
            if (disco.IsError)
            {
                return false;
            }

            // request token
            var req = new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = IdentityEndpoint.ClientID,
                ClientSecret = IdentityEndpoint.Secret,
                Scope = IdentityEndpoint.Scopes,
                UserName = IdentityEndpoint.UserName,
                Password = IdentityEndpoint.Password
            };
            var tokenResponse = await client.RequestPasswordTokenAsync(req);

            if (tokenResponse.IsError)
            {
                return false;
            }

            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.DeleteAsync(IdentityEndpoint.ResourceUri + $"/Secrets/{id.Value}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateClient(ClientApiDto clientApi)
        {
            // discover endpoints from metadata
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(IdentityEndpoint.Discovery);
            if (disco.IsError)
            {
                return false;
            }

            // request token
            var req = new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = IdentityEndpoint.ClientID,
                ClientSecret = IdentityEndpoint.Secret,
                Scope = IdentityEndpoint.Scopes,
                UserName = IdentityEndpoint.UserName,
                Password = IdentityEndpoint.Password
            };
            var tokenResponse = await client.RequestPasswordTokenAsync(req);

            if (tokenResponse.IsError)
            {
                return false;
            }

            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var dataJson = JsonSerializer.Serialize(clientApi);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");

            var response = await apiClient.PutAsync(IdentityEndpoint.ClientUri, stringContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateResource(ApiResourceApiDto resourceApiDto)
        {
            // discover endpoints from metadata
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(IdentityEndpoint.Discovery);
            if (disco.IsError)
            {
                return false;
            }

            // request token
            var req = new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = IdentityEndpoint.ClientID,
                ClientSecret = IdentityEndpoint.Secret,
                Scope = IdentityEndpoint.Scopes,
                UserName = IdentityEndpoint.UserName,
                Password = IdentityEndpoint.Password
            };
            var tokenResponse = await client.RequestPasswordTokenAsync(req);

            if (tokenResponse.IsError)
            {
                return false;
            }

            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var dataJson = JsonSerializer.Serialize(resourceApiDto);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");

            var response = await apiClient.PutAsync(IdentityEndpoint.ResourceUri, stringContent);

            return response.IsSuccessStatusCode;
        }
    }
    public class UpdateClientModel
    {
        public ClientApiDto ClientApiDto { get; set; }
        public AppClientDto AppClientDto { get; set; }
        public List<SelectListItem> allowedScopes { get; set; }
        public List<SelectListItem> redirectUris { get; set; }
        public List<SelectListItem> allowedGrantTypes { get; set; }
    }
    public class UpdateApiResource
    {
        public ApiResourceApiDto apiResourceApiDto { get; set; }
        public APIResourceDto aPIResource { get; set; }
    }
}
