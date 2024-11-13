using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PersonalDeskThing.App.Core.Cache;
using PersonalDeskThing.App.Core.Models.Options;
using SpotifyAPI.Web;
using static SpotifyAPI.Web.Scopes;

namespace PersonalDeskThing.App.Core.Spotify
{
    class SpotifyAuthService
    {
        SpotifyService? _service;
        SpotifyOptions _options;
        Uri _callbackUri;
        string _latestVerifier = string.Empty;
        CacheService _cache;

        public SpotifyAuthService(IOptions<SpotifyOptions> options, CacheService cache)
        {
            _options = options.Value;
            _callbackUri = new Uri(_options.CallbackUrl);
            _cache = cache;
        }
        public Uri CreateLoginRequestUri()
        {
            var(verifier, challenge) = PKCEUtil.GenerateCodes();
            _latestVerifier = verifier;
            var request = new LoginRequest(_callbackUri, _options.ClientId, LoginRequest.ResponseType.Code)
            {
                CodeChallenge = challenge,
                CodeChallengeMethod = "S256",
                Scope = new List<string> {
                    UserReadEmail,
                    UserReadPrivate,
                    PlaylistReadPrivate,
                    PlaylistReadCollaborative,
                    AppRemoteControl,
                    UserReadPlaybackPosition,
                    UserReadRecentlyPlayed,
                    UserTopRead,
                    UserLibraryRead,
                    UserLibraryModify,
                    UserFollowRead,
                    UserFollowModify,
                    UserReadCurrentlyPlaying,
                    UserReadPlaybackState,
                    UserModifyPlaybackState,
                }
            };
            return request.ToUri();
        }
        public async Task<SpotifyService?> CreateFromCache()
        {
            if (_service != null)
            {
                return _service;
            }

            string? json = (string?)await _cache.GetValue(CacheKeys.SpotifyCredentials).ConfigureAwait(true);
            if (json == null)
            {
                return null;
            }
            var token = JsonConvert.DeserializeObject<PKCETokenResponse>(json);

            var authenticator = new PKCEAuthenticator(_options.ClientId!, token!);
            authenticator.TokenRefreshed += async(sender, token) => await _cache.SetValue(CacheKeys.SpotifyCredentials, JsonConvert.SerializeObject(token));

            var config = SpotifyClientConfig.CreateDefault()
              .WithAuthenticator(authenticator);

            return new SpotifyService(new SpotifyClient(config));
        }
        public async Task<SpotifyService?> CreateFromCode(string code)
        {
            var token = await new OAuthClient().RequestToken(
            new PKCETokenRequest(_options.ClientId, code, _callbackUri, _latestVerifier)
                ).ConfigureAwait(true);

            string json = JsonConvert.SerializeObject(token);
            await _cache.SetValue(CacheKeys.SpotifyCredentials, json).ConfigureAwait(true);
            // Clear the service so it will be recreated with the new token
            _service = null;
            return await CreateFromCache().ConfigureAwait(true);
        }
    }
}
