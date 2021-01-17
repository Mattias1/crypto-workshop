using System;
using Microsoft.AspNetCore.Mvc;
using SecureServer.Token;

namespace SecureServer.Controllers {
    public class TokenController : Controller {
        [HttpPost("token/new")]
        public IActionResult NewToken([FromBody]TokenEmail input) {
            if (input.Email is null) {
                return BadRequest("No email provided.");
            }

            try {
                // Not adding the secret message here, in case you accidentally see it.
                var token = InternetToken.CreateAesEcb(input.Email, "user");
                string jsonHexString = token.ToTokenHexString();
                return Ok(jsonHexString);
            }
            catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        public class TokenEmail {
            public string? Email { get; set; }
        }
    }
}
