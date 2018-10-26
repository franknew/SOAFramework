﻿using System;
using System.Linq;
using System.Reflection;

namespace SOAFramework.Library
{
    public partial class ParameterReflector : ICustomAttributeReflectorProvider
    {
        private readonly ParameterInfo _reflectionInfo;
        private readonly CustomAttributeReflector[] _customAttributeReflectors;

        public CustomAttributeReflector[] CustomAttributeReflectors => _customAttributeReflectors;

        public string Name => _reflectionInfo.Name;

        public bool HasDeflautValue { get; }

        public object DefalutValue { get; }

        public int Position { get; }

        public Type ParameterType { get; }

        private ParameterReflector(ParameterInfo reflectionInfo)
        {
            _reflectionInfo = reflectionInfo ?? throw new ArgumentNullException(nameof(reflectionInfo));
            _customAttributeReflectors = _reflectionInfo.GetCustomAttributesData().Select(data => CustomAttributeReflector.Create(data)).ToArray();
            HasDeflautValue = reflectionInfo.DefaultValue != null;
            if (HasDeflautValue)
            {
                DefalutValue = reflectionInfo.DefaultValue;
            }
            Position = reflectionInfo.Position;
            ParameterType = reflectionInfo.ParameterType;
        }

        public ParameterInfo GetParameterInfo()
        {
            return _reflectionInfo;
        }

        public override string ToString() => $"Parameter : {_reflectionInfo}  ParameterType : {ParameterType}";
    }
}