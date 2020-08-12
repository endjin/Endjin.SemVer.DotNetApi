// <copyright file="MethodGeneratorBase.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Testing.CodeGen
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
