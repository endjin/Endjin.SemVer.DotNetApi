// <copyright file="LibraryComparisonSteps.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.Specs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Endjin.SemVer.DotNetApi.Specs.CodeGeneration;
    using Endjin.SemVer.DotNetApi.Specs.Utils;
    using Endjin.SemVer.DotNetApi.Versioning;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class LibraryComparisonSteps
    {
        private const string AssemblyName = "Endjin.TestLib";
        private readonly ScenarioContext scenarioContext;
        private readonly LibraryGeneratorSource libraryGeneratorSource;
        private readonly LibraryGenerator genOldAssembly;
        private readonly LibraryGenerator genNewAssembly;
        private LibraryChanges reportedChanges;
        private int uniqueSequenceId = 0;

        public LibraryComparisonSteps(ScenarioContext scenarioContext, LibraryGeneratorSource libraryGeneratorSource)
        {
            this.scenarioContext = scenarioContext;
            this.libraryGeneratorSource = libraryGeneratorSource;

            this.genOldAssembly = this.libraryGeneratorSource.AddLibrary(AssemblyName);
            this.genNewAssembly = this.libraryGeneratorSource.AddLibrary(AssemblyName);
        }

        [Given("generator '(.*)' defines a class named '(.*)'")]
        public void GivenGeneratorDefinesAClassNamed(string classGeneratorId, string className)
        {
            this.libraryGeneratorSource.AddClassGenerator(classGeneratorId, className);
        }

        [Given("class '(.*)' has a constructor with signature type '(.*)'")]
        public void GivenClassHasAConstructorWithSignatureType(string classGeneratorId, TestMethodSignatureTypes methodSignatureType)
        {
            ClassGenerator generator = this.libraryGeneratorSource.ClassGenerators[classGeneratorId];

            (string name, string type)[] parameters = GetParametersForSignatureType(methodSignatureType);

            generator.AddConstructor(parameters);
        }

        [Given("class '(.*)' has a method named '(.*)' with signature type '(.*)'")]
        public void GivenClassHasAMethodWithSignatureType(string classGeneratorId, string methodName, TestMethodSignatureTypes methodSignatureType)
        {
            ClassGenerator generator = this.libraryGeneratorSource.ClassGenerators[classGeneratorId];

            string returnType = GetReturnTypeForSignatureType(methodSignatureType);
            (string name, string type)[] parameters = GetParametersForSignatureType(methodSignatureType);

            generator.AddMethod(methodName, returnType, parameters);
        }

        [Given("class '(.*)' has a read/write property named '(.*)' of type '(.*)'")]
        public void GivenClassHasAReadWriteProperty(string classGeneratorId, string propertyName, string propertyType)
        {
            ClassGenerator generator = this.libraryGeneratorSource.ClassGenerators[classGeneratorId];

            generator.AddProperty(propertyName, propertyType);
        }

        [Given("class '(.*)' has an event named '(.*)' of type '(.*)'")]
        public void GivenClassHasAnEvent(string classGeneratorId, string eventName, string eventType)
        {
            ClassGenerator generator = this.libraryGeneratorSource.ClassGenerators[classGeneratorId];

            generator.AddEvent(eventName, eventType);
        }

        [Given("class '(.*)' has a field named '(.*)' of type '(.*)'")]
        public void GivenClassHasAFieldNamedOfType(string classGeneratorId, string fieldName, string fieldType)
        {
            ClassGenerator generator = this.libraryGeneratorSource.ClassGenerators[classGeneratorId];

            generator.AddField(fieldName, fieldType);
        }

        [Given("classes '(.*)' and '(.*)' each have some identical methods, properties, events, and fields")]
        public void GivenClassHasAnEvent(string classGeneratorId1, string classGeneratorId2)
        {
            ClassGenerator generator1 = this.libraryGeneratorSource.ClassGenerators[classGeneratorId1];
            ClassGenerator generator2 = this.libraryGeneratorSource.ClassGenerators[classGeneratorId2];

            string methodName = $"MethodUnchanging{++this.uniqueSequenceId}";
            string propertyName = $"PropertyUnchanging{++this.uniqueSequenceId}";
            string eventName = $"EventUnchanging{++this.uniqueSequenceId}";
            string fieldName = $"FieldUnchanging{++this.uniqueSequenceId}";
            string returnType = GetReturnTypeForSignatureType(TestMethodSignatureTypes.StringReturnStringAndIntParams);
            (string name, string type)[] parameters = GetParametersForSignatureType(TestMethodSignatureTypes.StringReturnStringAndIntParams);

            generator1.AddMethod(methodName, returnType, parameters);
            generator1.AddProperty(propertyName, "byte");
            generator1.AddEvent(eventName, "System.EventHandler");
            generator1.AddField(fieldName, "float");
            generator2.AddMethod(methodName, returnType, parameters);
            generator2.AddProperty(propertyName, "byte");
            generator2.AddEvent(eventName, "System.EventHandler");
            generator2.AddField(fieldName, "float");
        }

        [Given("the old assembly has the class '(.*)'")]
        public void GivenTheOldAssemblyHasAClass(string classGeneratorId)
        {
            this.genOldAssembly.AddClass(this.libraryGeneratorSource.ClassGenerators[classGeneratorId]);
        }

        [Given("the new assembly has the class '(.*)'")]
        public void GivenTheNewAssemblyHasAClass(string classGeneratorId)
        {
            this.genNewAssembly.AddClass(this.libraryGeneratorSource.ClassGenerators[classGeneratorId]);
        }

        [When("I compare the assemblies")]
        public void WhenICompareTheAssemblies()
        {
            string oldAssemblyPath = this.genOldAssembly.Compile();
            string newAssemblyPath = this.genNewAssembly.Compile();

            this.reportedChanges = LibraryComparison.DetectChanges(oldAssemblyPath, newAssemblyPath);
        }

        [Then("the LibraryChanges should report (.*) added type")]
        [Then("the LibraryChanges should report (.*) added types")]
        public void ThenTheLibraryChangesShouldReportAddedType(int addedCount)
        {
            Assert.AreEqual(addedCount, this.reportedChanges.TypesAdded.Count);
        }

        [Then("the LibraryChanges should report (.*) changed type")]
        [Then("the LibraryChanges should report (.*) changed types")]
        public void ThenTheLibraryChangesShouldReportChangedTypes(int changeCount)
        {
            Assert.AreEqual(changeCount, this.reportedChanges.TypesChanged.Count);
        }

        [Then("the LibraryChanges should report (.*) removed type")]
        [Then("the LibraryChanges should report (.*) removed types")]
        public void ThenTheLibraryChangesShouldReportRemovedTypes(int removedCount)
        {
            Assert.AreEqual(removedCount, this.reportedChanges.TypesRemoved.Count);
        }

        [Then(@"LibraryChanges\.TypesAdded should contain type named '(.*)'")]
        public void ThenLibraryChangesTypesAddedShouldContainTypeNamed(string name)
        {
            Assert.IsTrue(this.reportedChanges.TypesAdded.Any(t => t.FullName == name));
        }

        [Then(@"LibraryChanges\.TypesAdded '(.*)' should report (.*) added constructor")]
        [Then(@"LibraryChanges\.TypesAdded '(.*)' should report (.*) added constructors")]
        public void ThenLibraryChangesTypesAddedShouldReportAddedConstructors(string typeName, int addedCount)
        {
            LibraryType type = this.reportedChanges.TypesAdded.Single(t => t.FullName == typeName);
            Assert.AreEqual(addedCount, type.ConstructorsAdded.Count);
        }

        [Then(@"LibraryChanges\.TypesAdded '(.*)' should report (.*) removed constructor")]
        [Then(@"LibraryChanges\.TypesAdded '(.*)' should report (.*) removed constructors")]
        public void ThenLibraryChangesTypesAddedShouldReportRemovedConstructors(string typeName, int removedCount)
        {
            LibraryType type = this.reportedChanges.TypesAdded.Single(t => t.FullName == typeName);
            Assert.AreEqual(removedCount, type.ConstructorsRemoved.Count);
        }

        [Then(@"LibraryChanges\.TypesAdded '(.*)' should report (.*) added method")]
        [Then(@"LibraryChanges\.TypesAdded '(.*)' should report (.*) added methods")]
        public void ThenLibraryChangesTypesAddedShouldReportAddedMethods(string typeName, int addedCount)
        {
            LibraryType type = this.reportedChanges.TypesAdded.Single(t => t.FullName == typeName);
            Assert.AreEqual(addedCount, type.MethodsAdded.Count);
        }

        [Then(@"LibraryChanges\.TypesAdded '(.*)' should report (.*) removed method")]
        [Then(@"LibraryChanges\.TypesAdded '(.*)' should report (.*) removed methods")]
        public void ThenLibraryChangesTypesAddedShouldReportRemovedMethods(string typeName, int removedCount)
        {
            LibraryType type = this.reportedChanges.TypesAdded.Single(t => t.FullName == typeName);
            Assert.AreEqual(removedCount, type.MethodsRemoved.Count);
        }

        [Then(@"LibraryChanges\.TypesAdded '(.*)' should report (.*) added property")]
        [Then(@"LibraryChanges\.TypesAdded '(.*)' should report (.*) added properties")]
        public void ThenLibraryChangesTypesAddedShouldReportAddedProperties(string typeName, int addedCount)
        {
            LibraryType type = this.reportedChanges.TypesAdded.Single(t => t.FullName == typeName);
            Assert.AreEqual(addedCount, type.PropertiesAdded.Count);
        }

        [Then(@"LibraryChanges\.TypesAdded '(.*)' should report (.*) removed property")]
        [Then(@"LibraryChanges\.TypesAdded '(.*)' should report (.*) removed properties")]
        public void ThenLibraryChangesTypesAddedShouldReportRemovedProperties(string typeName, int removedCount)
        {
            LibraryType type = this.reportedChanges.TypesAdded.Single(t => t.FullName == typeName);
            Assert.AreEqual(removedCount, type.PropertiesRemoved.Count);
        }

        [Then(@"LibraryChanges\.TypesAdded '(.*)' should report (.*) added field")]
        [Then(@"LibraryChanges\.TypesAdded '(.*)' should report (.*) added fields")]
        public void ThenLibraryChangesTypesAddedShouldReportAddedFields(string typeName, int addedCount)
        {
            LibraryType type = this.reportedChanges.TypesAdded.Single(t => t.FullName == typeName);
            Assert.AreEqual(addedCount, type.FieldsAdded.Count);
        }

        [Then(@"LibraryChanges\.TypesAdded '(.*)' should report (.*) removed field")]
        [Then(@"LibraryChanges\.TypesAdded '(.*)' should report (.*) removed fields")]
        public void ThenLibraryChangesTypesAddedShouldReportRemovedFields(string typeName, int removedCount)
        {
            LibraryType type = this.reportedChanges.TypesAdded.Single(t => t.FullName == typeName);
            Assert.AreEqual(removedCount, type.FieldsRemoved.Count);
        }

        [Then(@"LibraryChanges\.TypesAdded '(.*)' should report (.*) added event")]
        [Then(@"LibraryChanges\.TypesAdded '(.*)' should report (.*) added events")]
        public void ThenLibraryChangesTypesAddedShouldReportAddedEvents(string typeName, int addedCount)
        {
            LibraryType type = this.reportedChanges.TypesAdded.Single(t => t.FullName == typeName);
            Assert.AreEqual(addedCount, type.EventsAdded.Count);
        }

        [Then(@"LibraryChanges\.TypesAdded '(.*)' should report (.*) removed event")]
        [Then(@"LibraryChanges\.TypesAdded '(.*)' should report (.*) removed events")]
        public void ThenLibraryChangesTypesAddedShouldReportRemovedEvents(string typeName, int removedCount)
        {
            LibraryType type = this.reportedChanges.TypesAdded.Single(t => t.FullName == typeName);
            Assert.AreEqual(removedCount, type.EventsRemoved.Count);
        }

        [Then(@"LibraryChanges\.TypesAdded '(.*)' should have new constructor matching signature '(.*)'")]
        public void ThenLibraryChangesTypesAddedShouldHaveConstructorMatchingSignature(string typeName, TestMethodSignatureTypes signature)
        {
            LibraryType type = this.reportedChanges.TypesAdded.Single(t => t.FullName == typeName);
            HasConstructorWithMatchingSignature(type.ConstructorsAdded, signature);
        }

        [Then(@"LibraryChanges\.TypesAdded '(.*)' should have new method '(.*)' matching signature '(.*)'")]
        public void ThenLibraryChangesTypesAddedShouldHaveMethodMatchingSignature(string typeName, string methodName, TestMethodSignatureTypes signature)
        {
            LibraryType type = this.reportedChanges.TypesAdded.Single(t => t.FullName == typeName);
            HasMethodWithMatchingSignature(type.MethodsAdded, methodName, signature);
        }

        [Then(@"LibraryChanges\.TypesAdded '(.*)' should have new read/write property '(.*)' of type '(.*)'")]
        public void ThenLibraryChangesTypesAddedShouldHaveReadWriteProperty(string typeName, string propertyName, string propertyType)
        {
            LibraryType type = this.reportedChanges.TypesAdded.Single(t => t.FullName == typeName);
            HasReadWritePropertyOfType(type.PropertiesAdded, propertyName, propertyType);
        }

        [Then(@"LibraryChanges\.TypesAdded '(.*)' should have new event '(.*)' of type '(.*)'")]
        public void ThenLibraryChangesTypesAddedShouldHaveEvent(string typeName, string eventName, string eventType)
        {
            LibraryType type = this.reportedChanges.TypesAdded.Single(t => t.FullName == typeName);
            HasEventOfType(type.EventsAdded, eventName, eventType);
        }

        [Then(@"LibraryChanges\.TypesAdded '(.*)' should have new field '(.*)' of type '(.*)'")]
        public void ThenLibraryChangesTypesAddedShouldHaveNewFieldOfType(string typeName, string fieldName, string fieldType)
        {
            LibraryType type = this.reportedChanges.TypesAdded.Single(t => t.FullName == typeName);
            HasFieldOfType(type.FieldsAdded, fieldName, fieldType);
        }

        [Then(@"LibraryChanges\.TypesChanged should contain type named '(.*)'")]
        public void ThenLibraryChangesTypesChangedShouldContainTypeNamed(string name)
        {
            Assert.IsTrue(this.reportedChanges.TypesChanged.Any(t => t.FullName == name));
        }

        [Then(@"LibraryChanges\.TypesChanged '(.*)' should report (.*) added constructor")]
        [Then(@"LibraryChanges\.TypesChanged '(.*)' should report (.*) added constructors")]
        public void ThenLibraryChangesTypesChangedShouldReportAddedConstructors(string typeName, int addedCount)
        {
            LibraryType type = this.reportedChanges.TypesChanged.Single(t => t.FullName == typeName);
            Assert.AreEqual(addedCount, type.ConstructorsAdded.Count);
        }

        [Then(@"LibraryChanges\.TypesChanged '(.*)' should report (.*) removed constructor")]
        [Then(@"LibraryChanges\.TypesChanged '(.*)' should report (.*) removed constructors")]
        public void ThenLibraryChangesTypesChangedShouldReportRemovedConstructors(string typeName, int removedCount)
        {
            LibraryType type = this.reportedChanges.TypesChanged.Single(t => t.FullName == typeName);
            Assert.AreEqual(removedCount, type.ConstructorsRemoved.Count);
        }

        [Then(@"LibraryChanges\.TypesChanged '(.*)' should report (.*) added method")]
        [Then(@"LibraryChanges\.TypesChanged '(.*)' should report (.*) added methods")]
        public void ThenLibraryChangesTypesChangedShouldReportAddedMethods(string typeName, int addedCount)
        {
            LibraryType type = this.reportedChanges.TypesChanged.Single(t => t.FullName == typeName);
            Assert.AreEqual(addedCount, type.MethodsAdded.Count);
        }

        [Then(@"LibraryChanges\.TypesChanged '(.*)' should report (.*) removed method")]
        [Then(@"LibraryChanges\.TypesChanged '(.*)' should report (.*) removed methods")]
        public void ThenLibraryChangesTypesChangedShouldReportRemovedMethods(string typeName, int removedCount)
        {
            LibraryType type = this.reportedChanges.TypesChanged.Single(t => t.FullName == typeName);
            Assert.AreEqual(removedCount, type.MethodsRemoved.Count);
        }

        [Then(@"LibraryChanges\.TypesChanged '(.*)' should report (.*) added property")]
        [Then(@"LibraryChanges\.TypesChanged '(.*)' should report (.*) added properties")]
        public void ThenLibraryChangesTypesChangedShouldReportAddedProperties(string typeName, int addedCount)
        {
            LibraryType type = this.reportedChanges.TypesChanged.Single(t => t.FullName == typeName);
            Assert.AreEqual(addedCount, type.PropertiesAdded.Count);
        }

        [Then(@"LibraryChanges\.TypesChanged '(.*)' should report (.*) removed property")]
        [Then(@"LibraryChanges\.TypesChanged '(.*)' should report (.*) removed properties")]
        public void ThenLibraryChangesTypesChangedShouldReportRemovedProperties(string typeName, int removedCount)
        {
            LibraryType type = this.reportedChanges.TypesChanged.Single(t => t.FullName == typeName);
            Assert.AreEqual(removedCount, type.PropertiesRemoved.Count);
        }

        [Then(@"LibraryChanges\.TypesChanged '(.*)' should report (.*) added field")]
        [Then(@"LibraryChanges\.TypesChanged '(.*)' should report (.*) added fields")]
        public void ThenLibraryChangesTypesChangedShouldReportAddedFields(string typeName, int addedCount)
        {
            LibraryType type = this.reportedChanges.TypesChanged.Single(t => t.FullName == typeName);
            Assert.AreEqual(addedCount, type.FieldsAdded.Count);
        }

        [Then(@"LibraryChanges\.TypesChanged '(.*)' should report (.*) removed field")]
        [Then(@"LibraryChanges\.TypesChanged '(.*)' should report (.*) removed fields")]
        public void ThenLibraryChangesTypesChangedShouldReportRemovedFields(string typeName, int removedCount)
        {
            LibraryType type = this.reportedChanges.TypesChanged.Single(t => t.FullName == typeName);
            Assert.AreEqual(removedCount, type.FieldsRemoved.Count);
        }

        [Then(@"LibraryChanges\.TypesChanged '(.*)' should report (.*) added event")]
        [Then(@"LibraryChanges\.TypesChanged '(.*)' should report (.*) added events")]
        public void ThenLibraryChangesTypesChangedShouldReportAddedEvents(string typeName, int addedCount)
        {
            LibraryType type = this.reportedChanges.TypesChanged.Single(t => t.FullName == typeName);
            Assert.AreEqual(addedCount, type.EventsAdded.Count);
        }

        [Then(@"LibraryChanges\.TypesChanged '(.*)' should report (.*) removed event")]
        [Then(@"LibraryChanges\.TypesChanged '(.*)' should report (.*) removed events")]
        public void ThenLibraryChangesTypesChangedShouldReportRemovedEvents(string typeName, int removedCount)
        {
            LibraryType type = this.reportedChanges.TypesChanged.Single(t => t.FullName == typeName);
            Assert.AreEqual(removedCount, type.EventsRemoved.Count);
        }

        [Then(@"LibraryChanges\.TypesChanged '(.*)' should have new constructor matching signature '(.*)'")]
        public void ThenLibraryChangesTypesChangedShouldHaveNewConstructorMatchingSignature(string typeName, TestMethodSignatureTypes signature)
        {
            LibraryType type = this.reportedChanges.TypesChanged.Single(t => t.FullName == typeName);
            HasConstructorWithMatchingSignature(type.ConstructorsAdded, signature);
        }

        [Then(@"LibraryChanges\.TypesChanged '(.*)' should have removed constructor matching signature '(.*)'")]
        public void ThenLibraryChanges_TypesChangedShouldHaveRemovedConstructorMatchingSignature(string typeName, TestMethodSignatureTypes signature)
        {
            LibraryType type = this.reportedChanges.TypesChanged.Single(t => t.FullName == typeName);
            HasConstructorWithMatchingSignature(type.ConstructorsRemoved, signature);
        }

        [Then(@"LibraryChanges\.TypesChanged '(.*)' should have new method '(.*)' matching signature '(.*)'")]
        public void ThenLibraryChangesTypesChangedShouldHaveNewMethodMatchingSignature(string typeName, string methodName, TestMethodSignatureTypes signature)
        {
            LibraryType type = this.reportedChanges.TypesChanged.Single(t => t.FullName == typeName);
            HasMethodWithMatchingSignature(type.MethodsAdded, methodName, signature);
        }

        [Then(@"LibraryChanges\.TypesChanged '(.*)' should have removed method '(.*)' matching signature '(.*)'")]
        public void ThenLibraryChanges_TypesChangedShouldHaveRemovedMethodMatchingSignature(string typeName, string methodName, TestMethodSignatureTypes signature)
        {
            LibraryType type = this.reportedChanges.TypesChanged.Single(t => t.FullName == typeName);
            HasMethodWithMatchingSignature(type.MethodsRemoved, methodName, signature);
        }

        [Then(@"LibraryChanges\.TypesChanged '(.*)' should have new read/write property '(.*)' of type '(.*)'")]
        public void ThenLibraryChangesTypesChangedShouldHaveNewReadWriteProperty(string typeName, string propertyName, string propertyType)
        {
            LibraryType type = this.reportedChanges.TypesChanged.Single(t => t.FullName == typeName);
            HasReadWritePropertyOfType(type.PropertiesAdded, propertyName, propertyType);
        }

        [Then(@"LibraryChanges\.TypesChanged '(.*)' should have removed read/write property '(.*)' of type '(.*)'")]
        public void ThenLibraryChangesTypesChangedShouldHaveRemovedReadWriteProperty(string typeName, string propertyName, string propertyType)
        {
            LibraryType type = this.reportedChanges.TypesChanged.Single(t => t.FullName == typeName);
            HasReadWritePropertyOfType(type.PropertiesRemoved, propertyName, propertyType);
        }

        [Then(@"LibraryChanges\.TypesChanged '(.*)' should have new event '(.*)' of type '(.*)'")]
        public void ThenLibraryChangesTypesChangedShouldHaveNewEvent(string typeName, string eventName, string eventType)
        {
            LibraryType type = this.reportedChanges.TypesChanged.Single(t => t.FullName == typeName);
            HasEventOfType(type.EventsAdded, eventName, eventType);
        }

        [Then(@"LibraryChanges\.TypesChanged '(.*)' should have removed event '(.*)' of type '(.*)'")]
        public void ThenLibraryChangesTypesChangedShouldHaveRemovedEvent(string typeName, string eventName, string eventType)
        {
            LibraryType type = this.reportedChanges.TypesChanged.Single(t => t.FullName == typeName);
            HasEventOfType(type.EventsRemoved, eventName, eventType);
        }

        [Then(@"LibraryChanges\.TypesChanged '(.*)' should have new field '(.*)' of type '(.*)'")]
        public void ThenLibraryChangesTypesChangedShouldHaveNewFieldOfType(string typeName, string fieldName, string fieldType)
        {
            LibraryType type = this.reportedChanges.TypesChanged.Single(t => t.FullName == typeName);
            HasFieldOfType(type.FieldsAdded, fieldName, fieldType);
        }

        [Then(@"LibraryChanges\.TypesChanged '(.*)' should have removed field '(.*)' of type '(.*)'")]
        public void ThenLibraryChangesTypesChangedShouldHaveRemovedFieldOfType(string typeName, string fieldName, string fieldType)
        {
            LibraryType type = this.reportedChanges.TypesChanged.Single(t => t.FullName == typeName);
            HasFieldOfType(type.FieldsRemoved, fieldName, fieldType);
        }

        [Then(@"LibraryChanges\.TypesRemoved should contain type named '(.*)'")]
        public void ThenLibraryChangesTypesRemovedShouldContainTypeNamed(string name)
        {
            Assert.IsTrue(this.reportedChanges.TypesRemoved.Any(t => t.FullName == name));
        }

        [Then(@"LibraryChanges\.TypesRemoved '(.*)' should report (.*) added constructor")]
        [Then(@"LibraryChanges\.TypesRemoved '(.*)' should report (.*) added constructors")]
        public void ThenLibraryChangesTypesRemovedShouldReportAddedConstructors(string typeName, int addedCount)
        {
            LibraryType type = this.reportedChanges.TypesRemoved.Single(t => t.FullName == typeName);
            Assert.AreEqual(addedCount, type.ConstructorsAdded.Count);
        }

        [Then(@"LibraryChanges\.TypesRemoved '(.*)' should report (.*) removed constructor")]
        [Then(@"LibraryChanges\.TypesRemoved '(.*)' should report (.*) removed constructors")]
        public void ThenLibraryChangesTypesRemovedShouldReportRemovedConstructors(string typeName, int removedCount)
        {
            LibraryType type = this.reportedChanges.TypesRemoved.Single(t => t.FullName == typeName);
            Assert.AreEqual(removedCount, type.ConstructorsRemoved.Count);
        }

        [Then(@"LibraryChanges\.TypesRemoved '(.*)' should report (.*) added method")]
        [Then(@"LibraryChanges\.TypesRemoved '(.*)' should report (.*) added methods")]
        public void ThenLibraryChangesTypesRemovedShouldReportAddedMethods(string typeName, int addedCount)
        {
            LibraryType type = this.reportedChanges.TypesRemoved.Single(t => t.FullName == typeName);
            Assert.AreEqual(addedCount, type.MethodsAdded.Count);
        }

        [Then(@"LibraryChanges\.TypesRemoved '(.*)' should report (.*) removed method")]
        [Then(@"LibraryChanges\.TypesRemoved '(.*)' should report (.*) removed methods")]
        public void ThenLibraryChangesTypesRemovedShouldReportRemovedMethods(string typeName, int removedCount)
        {
            LibraryType type = this.reportedChanges.TypesRemoved.Single(t => t.FullName == typeName);
            Assert.AreEqual(removedCount, type.MethodsRemoved.Count);
        }

        [Then(@"LibraryChanges\.TypesRemoved '(.*)' should report (.*) added property")]
        [Then(@"LibraryChanges\.TypesRemoved '(.*)' should report (.*) added properties")]
        public void ThenLibraryChangesTypesRemovedShouldReportAddedProperties(string typeName, int addedCount)
        {
            LibraryType type = this.reportedChanges.TypesRemoved.Single(t => t.FullName == typeName);
            Assert.AreEqual(addedCount, type.PropertiesAdded.Count);
        }

        [Then(@"LibraryChanges\.TypesRemoved '(.*)' should report (.*) removed property")]
        [Then(@"LibraryChanges\.TypesRemoved '(.*)' should report (.*) removed properties")]
        public void ThenLibraryChangesTypesRemovedShouldReportRemovedProperties(string typeName, int removedCount)
        {
            LibraryType type = this.reportedChanges.TypesRemoved.Single(t => t.FullName == typeName);
            Assert.AreEqual(removedCount, type.PropertiesRemoved.Count);
        }

        [Then(@"LibraryChanges\.TypesRemoved '(.*)' should report (.*) added field")]
        [Then(@"LibraryChanges\.TypesRemoved '(.*)' should report (.*) added fields")]
        public void ThenLibraryChangesTypesRemovedShouldReportAddedFields(string typeName, int addedCount)
        {
            LibraryType type = this.reportedChanges.TypesRemoved.Single(t => t.FullName == typeName);
            Assert.AreEqual(addedCount, type.FieldsAdded.Count);
        }

        [Then(@"LibraryChanges\.TypesRemoved '(.*)' should report (.*) removed field")]
        [Then(@"LibraryChanges\.TypesRemoved '(.*)' should report (.*) removed fields")]
        public void ThenLibraryChangesTypesRemovedShouldReportRemovedFields(string typeName, int removedCount)
        {
            LibraryType type = this.reportedChanges.TypesRemoved.Single(t => t.FullName == typeName);
            Assert.AreEqual(removedCount, type.FieldsRemoved.Count);
        }

        [Then(@"LibraryChanges\.TypesRemoved '(.*)' should report (.*) added event")]
        [Then(@"LibraryChanges\.TypesRemoved '(.*)' should report (.*) added events")]
        public void ThenLibraryChangesTypesRemovedShouldReportAddedEvents(string typeName, int addedCount)
        {
            LibraryType type = this.reportedChanges.TypesRemoved.Single(t => t.FullName == typeName);
            Assert.AreEqual(addedCount, type.EventsAdded.Count);
        }

        [Then(@"LibraryChanges\.TypesRemoved '(.*)' should report (.*) removed event")]
        [Then(@"LibraryChanges\.TypesRemoved '(.*)' should report (.*) removed events")]
        public void ThenLibraryChangesTypesRemovedShouldReportRemovedEvents(string typeName, int removedCount)
        {
            LibraryType type = this.reportedChanges.TypesRemoved.Single(t => t.FullName == typeName);
            Assert.AreEqual(removedCount, type.EventsRemoved.Count);
        }

        [Then(@"LibraryChanges\.TypesRemoved '(.*)' should have removed constructor matching signature '(.*)'")]
        public void ThenLibraryChangesTypesRemovedShouldHaveConstructorMatchingSignature(string typeName, TestMethodSignatureTypes signature)
        {
            LibraryType type = this.reportedChanges.TypesRemoved.Single(t => t.FullName == typeName);
            HasConstructorWithMatchingSignature(type.ConstructorsRemoved, signature);
        }

        [Then(@"LibraryChanges\.TypesRemoved '(.*)' should have removed read/write property '(.*)' of type '(.*)'")]
        public void ThenLibraryChangesTypesRemovedShouldHaveReadWriteProperty(string typeName, string propertyName, string propertyType)
        {
            LibraryType type = this.reportedChanges.TypesRemoved.Single(t => t.FullName == typeName);
            HasReadWritePropertyOfType(type.PropertiesRemoved, propertyName, propertyType);
        }

        [Then(@"LibraryChanges\.TypesRemoved '(.*)' should have removed field '(.*)' of type '(.*)'")]
        public void ThenLibraryChangesTypesRemovedShouldHaveRemovedFieldOfType(string typeName, string fieldName, string fieldType)
        {
            LibraryType type = this.reportedChanges.TypesRemoved.Single(t => t.FullName == typeName);
            HasFieldOfType(type.FieldsRemoved, fieldName, fieldType);
        }

        [Then(@"LibraryChanges\.TypesRemoved '(.*)' should have removed event '(.*)' of type '(.*)'")]
        public void ThenLibraryChangesTypesRemovedShouldHaveRemovedEvent(string typeName, string eventName, string eventType)
        {
            LibraryType type = this.reportedChanges.TypesRemoved.Single(t => t.FullName == typeName);
            HasEventOfType(type.EventsRemoved, eventName, eventType);
        }

        [Then(@"LibraryChanges\.TypesRemoved '(.*)' should have removed method '(.*)' matching signature '(.*)'")]
        public void ThenLibraryChangesTypesRemovedShouldHaveMethodMatchingSignature(string typeName, string methodName, TestMethodSignatureTypes signature)
        {
            LibraryType type = this.reportedChanges.TypesRemoved.Single(t => t.FullName == typeName);
            HasMethodWithMatchingSignature(type.MethodsRemoved, methodName, signature);
        }

        private static void HasMethodWithMatchingSignature(IReadOnlyList<LibraryMethod> methods, string methodName, TestMethodSignatureTypes signature)
        {
            string returnType = GetReturnTypeForSignatureType(signature);
            (string name, string type)[] paramsRequired = GetParametersForSignatureType(signature);
            Assert.IsTrue(methods.Any(m =>
                m.Name == methodName &&
                returnType == (m.ReturnType?.FullName ?? TestMethodsReturnTypes.Void) &&
                paramsRequired.SequenceEqual(m.Parameters.Select(p => (p.Name, p.Type.FullName)))));
        }

        private static void HasConstructorWithMatchingSignature(IReadOnlyList<LibraryConstructor> constructors, TestMethodSignatureTypes signature)
        {
            (string name, string type)[] paramsRequired = GetParametersForSignatureType(signature);
            Assert.IsTrue(constructors.Any(
                c => paramsRequired.SequenceEqual(c.Parameters.Select(p => (p.Name, p.Type.FullName)))));
        }

        private static void HasReadWritePropertyOfType(IReadOnlyList<LibraryProperty> properties, string propertyName, string propertyType)
        {
            Assert.IsTrue(properties.Any(p => p.Name == propertyName && p.Type.FullName == propertyType));
        }

        private static void HasFieldOfType(IReadOnlyList<LibraryField> fields, string fieldName, string fieldType)
        {
            Assert.IsTrue(fields.Any(p => p.Name == fieldName && p.Type.FullName == fieldType));
        }

        private static void HasEventOfType(IReadOnlyList<LibraryEvent> events, string eventName, string eventType)
        {
            Assert.IsTrue(events.Any(p => p.Name == eventName && p.Type.FullName == eventType));
        }

        private static string GetReturnTypeForSignatureType(TestMethodSignatureTypes methodSignatureType)
        {
            switch (methodSignatureType)
            {
                case TestMethodSignatureTypes.VoidNoParams:
                case TestMethodSignatureTypes.VoidReturnStringParams:
                case TestMethodSignatureTypes.VoidReturnStringAndIntParams:
                    return TestMethodsReturnTypes.Void;

                case TestMethodSignatureTypes.StringReturnNoParams:
                case TestMethodSignatureTypes.StringReturnStringAndIntParams:
                    return "System.String";

                default:
                    throw new ArgumentOutOfRangeException(nameof(methodSignatureType));
            }
        }

        private static (string name, string type)[] GetParametersForSignatureType(TestMethodSignatureTypes methodSignatureType)
        {
            (string name, string type)[] parameters = { };

            switch (methodSignatureType)
            {
                case TestMethodSignatureTypes.VoidNoParams:
                case TestMethodSignatureTypes.StringReturnNoParams:
                    break;

                case TestMethodSignatureTypes.VoidReturnStringAndIntParams:
                case TestMethodSignatureTypes.StringReturnStringAndIntParams:
                    parameters = new[] { (name: "p1", type: "System.String"), (name: "p2", type: "System.Int32") };
                    break;

                case TestMethodSignatureTypes.VoidReturnStringParams:
                    parameters = new[] { (name: "p1", type: "System.String") };
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(methodSignatureType));
            }

            return parameters;
        }
    }
}
