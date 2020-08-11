// <copyright file="MethodGeneratorBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.Specs.CodeGeneration
{
    public abstract class MethodGeneratorBase
    {
        protected MethodGeneratorBase(params (string name, string type)[] parameters)
        {
            this.Parameters = parameters;
        }

        public (string name, string type)[] Parameters { get; }
    }
}
