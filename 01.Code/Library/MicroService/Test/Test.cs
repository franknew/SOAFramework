﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AustinHarris.JsonRpc;

namespace MicroService.Library
{
    [DemoFilter]
    [JsonRpcClass]
    public class CalculatorService
    {
        [JsonRpcMethod]
        public double add(double l, double r)
        {
            return l + r;
        }

        [DemoNoneFilter]
        public decimal? Test2(decimal x)
        {
            return x;
        }

        [DemoNoneFilter]
        [JsonRpcMethod]
        public string ping()
        {
            return "hello";
        }
    }
}