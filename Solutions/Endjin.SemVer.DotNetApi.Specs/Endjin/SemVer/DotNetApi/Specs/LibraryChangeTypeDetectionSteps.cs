// <copyright file="LibraryChangeTypeDetectionSteps.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.Specs
{
    using System.Collections.Generic;
    using Endjin.SemVer.DotNetApi.Specs.Utils;
    using Endjin.SemVer.DotNetApi.Versioning;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class LibraryChangeTypeDetectionSteps
    {
        private readonly ScenarioContext scenarioContext;
        private readonly LibraryChangeTestSource changeSource;
        private readonly Dictionary<ChangeTypes, SemanticVersionChange> comparisonResults = new Dictionary<ChangeTypes, SemanticVersionChange>();

        public LibraryChangeTypeDetectionSteps(ScenarioContext scenarioContext, LibraryChangeTestSource changeSource)
        {
            this.scenarioContext = scenarioContext;
            this.changeSource = changeSource;
        }

        [Given("the library neither adds nor removes anything")]
        public void GivenTheLibraryNeitherAddsNorRemovesAnything()
        {
            // No action required. This step exists just so that we can state intent clearly in
            // tests, but it's actually the default.
        }

        [Given("the new library adds a type")]
        public void GivenTheNewLibraryAddsAType()
        {
            this.changeSource.Require.AddPublicType();
        }

        [Given("the new library adds a constructor")]
        public void GivenTheNewLibraryAddsAConstructor()
        {
            this.changeSource.Require.AddPublicConstructor();
        }

        [Given("the new library adds a method")]
        public void GivenTheNewLibraryAddsAMethod()
        {
            this.changeSource.Require.AddPublicMethod();
        }

        [Given("the new library adds a property")]
        public void GivenTheNewLibraryAddsAProperty()
        {
            this.changeSource.Require.AddPublicProperty();
        }

        [Given("the new library adds a field")]
        public void GivenTheNewLibraryAddsAField()
        {
            this.changeSource.Require.AddPublicField();
        }

        [Given("the new library removes a type")]
        public void GivenTheNewLibraryRemovesAType()
        {
            this.changeSource.Require.RemovePublicType();
        }

        [Given("the new library removes a constructor")]
        public void GivenTheNewLibraryRemovesAConstructor()
        {
            this.changeSource.Require.RemovePublicConstructor();
        }

        [Given("the new library removes a method")]
        public void GivenTheNewLibraryRemovesAMethod()
        {
            this.changeSource.Require.RemovePublicMethod();
        }

        [Given("the new library removes a property")]
        public void GivenTheNewLibraryRemovesAProperty()
        {
            this.changeSource.Require.RemovePublicProperty();
        }

        [Given("the new library removes a field")]
        public void GivenTheNewLibraryRemovesAField()
        {
            this.changeSource.Require.RemovePublicField();
        }

        [Given("whether or not types are added")]
        public void WhetherOrNotTypesAreAdded()
        {
            this.changeSource.Permutations.AddPublicType();
        }

        [Given("whether or not constructors are added")]
        public void GivenWhetherOrNotConstructorsAreAdded()
        {
            this.changeSource.Permutations.AddPublicConstructor();
        }

        [Given("whether or not methods are added")]
        public void WhetherOrNotMethodsAreAdded()
        {
            this.changeSource.Permutations.AddPublicMethod();
        }

        [Given("whether or not properties are added")]
        public void WhetherOrNotPropertiesAreAdded()
        {
            this.changeSource.Permutations.AddPublicProperty();
        }

        [Given("whether or not fields are added")]
        public void WhetherOrNotFieldsAreAdded()
        {
            this.changeSource.Permutations.AddPublicField();
        }

        [Given("whether or not types are removed")]
        public void WhetherOrNotTypesAreRemoved()
        {
            this.changeSource.Permutations.RemovePublicType();
        }

        [Given("whether or not methods are removed")]
        public void WhetherOrNotMethodsAreRemoved()
        {
            this.changeSource.Permutations.RemovePublicMethod();
        }

        [Given("whether or not properties are removed")]
        public void WhetherOrNotPropertiesAreRemoved()
        {
            this.changeSource.Permutations.RemovePublicProperty();
        }

        [Given("whether or not constructors are removed")]
        public void GivenWhetherOrNotConstructorsAreRemoved()
        {
            this.changeSource.Permutations.RemovePublicConstructor();
        }

        [Given("whether or not fields are removed")]
        public void WhetherOrNotFieldsAreRemoved()
        {
            this.changeSource.Permutations.RemovePublicField();
        }

        [When("I compare libaries")]
        public void WhenICompareLibaries()
        {
            foreach ((ChangeTypes changeType, LibraryChanges changes) in this.changeSource.GetChanges())
            {
                SemanticVersionChange minimumAcceptableChange =
                    LibrarySemanticVersionChangeAnalyzer.GetMinimumAcceptableChange(changes);
                this.comparisonResults.Add(changeType, minimumAcceptableChange);
            }
        }

        [Then("the minimum acceptable change type is '(.*)'")]
        public void ThenTheMinimumAcceptableChangeTypeIs(SemanticVersionChange changeType)
        {
            foreach (KeyValuePair<ChangeTypes, SemanticVersionChange> entry in this.comparisonResults)
            {
                Assert.AreEqual(changeType, entry.Value, "For changes: " + entry.Key);
            }
        }
    }
}
