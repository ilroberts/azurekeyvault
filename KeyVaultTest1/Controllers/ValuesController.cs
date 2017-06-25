using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KeyVaultTest1.Models;
using System.Security.Cryptography;
using KeyVaultTest1.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace KeyVaultTest1.Controllers
{


    [Route("api/[controller]")]
    public class ValuesController : Controller
    {

        private VaultHelper vh = null;


        private IConfiguration Configuration { get; set; }
        public ValuesController(IConfiguration configuration)
        {
            Configuration = configuration;
            vh = new VaultHelper(Configuration["VaultURL"], Configuration["ClientId"], Configuration["ClientSecret"]);

            var secret = vh.GetSecret("myFirstSecret");
            secret.Wait();
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]DataDto dataDto)
        {
            if(dataDto.data.Length > 0)
            {
                using (var random = new RNGCryptoServiceProvider())
                {
                    var key = new byte[16];
                    random.GetBytes(key);
                    byte[] result = Encryption.EncryptStringToBytes_Aes(dataDto.data, key);
                    string decryptedResult = Encryption.DecryptStringFromBytes_Aes(result, key);
                }
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
