﻿// <copyright file="ClassGenerator.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Testing.CodeGen
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines a class to be generated as part of one or more <see cref="LibraryGenerator"/>s.
    /// </summary>
    public class ClassGenerator
    {
        private readonly List<ConstructorGenerator> constructors = new List<ConstructorGenerator>();
        private readonly List<MethodGenerator> methods = new List<MethodGenerator>();
        private readonly List<PropertyGenerator> properties = new List<PropertyGenerator>();
        private readonly List<FieldGenerator> fields = new List<FieldGenerator>();
        private readonly List<EventGenerator> events = new List<EventGenerator>();

        public ClassGenerator(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets the class's name.
        /// </summary>
        public string Name { get; }

        public IReadOnlyList<MethodGenerator> Methods => this.methods;

        public IReadOnlyList<ConstructorGenerator> Constructors => this.constructors;

        public IReadOnlyList<PropertyGenerator> Properties => this.properties;

        public IReadOnlyList<FieldGenerator> Fields => this.fields;

        public IReadOnlyList<EventGenerator> Events => this.events;

        public void AddMethod(string name, string returnType, params (string Name, string Type)[] parameters)
        {
            this.methods.Add(new MethodGenerator(name, returnType, parameters));
        }

        public void AddConstructor(params (string Name, string Type)[] parameters)
        {
            this.constructors.Add(new ConstructorGenerator(parameters));
        }

        public void AddProperty(string propertyName, string propertyType)
        {
            this.properties.Add(new PropertyGenerator(propertyName, propertyType));
        }

        public void AddEvent(string eventName, string eventType)
        {
            this.events.Add(new EventGenerator(eventName, eventType));
        }

        public void AddField(string fieldName, string fieldType)
        {
            this.fields.Add(new FieldGenerator(fieldName, fieldType));
        }
    }
}