using System;
using System.Collections.Generic;
using System.Text;

namespace RclPinesIntranet.Config
{
    public class AuthExtra
    {
        public string? TokenName { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }

        public string? TokenNameJwt { get=> GetJwtTokenName(); }
        public string? TokenNameRefresh { get => GetRefreshTokenName(); }


        //For now until we can get the JWT validation working properly, this will be set to false. Once we can validate the JWT properly, we can set this to true and remove the old logic in the CustomAuthenticationStateProvider.
        //This will eventually be removed!!!!
        public bool ApplyIssueAuthLogic { get; set; }=false;



        private string GetJwtTokenName()
        {
            return string.IsNullOrEmpty(TokenName) ? "ERROR_JWT" : $"{TokenName}_jwt";
        }

        private string GetRefreshTokenName()
        {
            return string.IsNullOrEmpty(TokenName) ? "ERROR_REFRESH" : $"{TokenName}_refresh";
        }
    }
}