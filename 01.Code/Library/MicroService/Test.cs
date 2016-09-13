using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AustinHarris.JsonRpc;

namespace MicroService.Library
{
    [JsonRpcClass]
    public class CalculatorService
    {
        [JsonRpcMethod]
        private double add(double l, double r)
        {
            return l + r;
        }

        [JsonRpcMethod]
        private int addInt(int l, int r)
        {
            return l + r;
        }

        [JsonRpcMethod]
        public float? NullableFloatToNullableFloat(float? a)
        {
            return a;
        }

        [JsonRpcMethod]
        public decimal? Test2(decimal x)
        {
            return x;
        }

        [JsonRpcMethod]
        public string StringMe(string x)
        {
            return x;
        }
    }
}
