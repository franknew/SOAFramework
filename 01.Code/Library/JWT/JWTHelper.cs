using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace SOAFramework.Library
{
    public class JWTHelper
    {
        public static string Encode(string secret, IDictionary<string, object> payload, JwtHashAlgorithm type = JwtHashAlgorithm.HS256)
        {
            HMACSHAAlgorithmFactory factory = new HMACSHAAlgorithmFactory();
            IJwtAlgorithm algorithm = factory.Create(type);
            JWT.IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            var builder = new JwtBuilder()
                .WithAlgorithm(algorithm)
                .WithSecret(secret);
            if (payload != null)
            {
                foreach (var key in payload.Keys)
                {
                    builder = builder.AddClaim(key, payload[key]);
                }
            }
            var token = builder.Build();
            return token;
        }

        public static string Decode(string token, string secret)
        {
            var json = new JwtBuilder()
                .WithSecret(secret)
                .MustVerifySignature()
                .Decode(token);
            return json;
        }

        public static T Decode<T>(string token, string secret)
        {var json = Decode(token, secret);
            T t = JsonConvert.DeserializeObject<T>(json);
            return t;
        }
    }
}
