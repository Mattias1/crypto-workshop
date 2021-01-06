using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecureServer.Algorithms;
using SecureServer.Token;
using System;
using System.Net;

namespace SecureServer.Controllers {
    public class TokenController : Controller {
        [HttpPost("token/new")]
        public IActionResult GetToken([FromBody]TokenEmail input) {
            if (input.Email is null) {
                return BadRequest("No email provided.");
            }
            var token = JsonInternetToken.CreateAesEcb(input.Email, "user", "TODO");
            string jsonHexString = token.ToJsonHexString();
            return Ok(jsonHexString);
        }

        public class TokenEmail {
            public string? Email { get; set; }
        }
    }
}
