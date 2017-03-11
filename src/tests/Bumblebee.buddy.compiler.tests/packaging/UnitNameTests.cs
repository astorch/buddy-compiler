using System;
using System.Collections.Generic;
using Bumblebee.buddy.compiler.exceptions;
using Bumblebee.buddy.compiler.packaging;
using NUnit.Framework;

namespace Bumblebee.buddy.compiler.tests.packaging {
    [TestFixture]
    public class UnitNameTests {

        [Test, TestCaseSource(typeof(TestCaseFactory), "TestCasesFullQualified")]
        public UnitName NewInstanceFullQualified(string fullQualifiedName) {
            return new UnitName(fullQualifiedName);
        }

        [Test, TestCaseSource(typeof(TestCaseFactory), "TestCasesFragments")]
        public UnitName NewInstanceFragments(string applicationName, string version, string useCaseName, string scenarioName) {
            return new UnitName(applicationName, version, useCaseName, scenarioName);
        }

        [Test, TestCaseSource(typeof(TestCaseFactory), "TestCasesQualifiedStrings")]
        public string ToQualifiedString(UnitName unitName) {
            return unitName.ToQualifiedString();
        }

        [Test, TestCaseSource(typeof(TestCaseFactory), "TestCasesParseFromFile")]
        public UnitName ParseFromFile(string fileContent) {
            return UnitName.ParseFromFile(fileContent);
        }

        class TestCaseFactory {
            public IEnumerable<TestCaseData> TestCasesFullQualified {
                get {
                    yield return new TestCaseData("untitled.s.test1.file1").Returns(new UnitName("untitled", "*", "test1", "file1"));
                    yield return new TestCaseData("Application_V2.5d507us.Test_Data.My_First_Test").Returns(new UnitName("Application V2", "5.507-*", "Test Data", "My First Test"));

                    yield return new TestCaseData((string)null).Throws(typeof(ArgumentNullException));
                    yield return new TestCaseData(string.Empty).Throws(typeof(ArgumentNullException));

                    yield return new TestCaseData("segment.final").Throws(typeof(UnitNameFormatException));
                    yield return new TestCaseData(".segment.final").Throws(typeof(UnitNameFormatException));

                    yield return new TestCaseData("un titled.5212.frag1.frag2").Throws(typeof(UnitNameFormatException));
                    yield return new TestCaseData("untitled.*.frag1.frag2").Throws(typeof(UnitNameFormatException));
                }
            }

            public IEnumerable<TestCaseData> TestCasesFragments {
                get {
                    yield return new TestCaseData("untitled", "*", "untitled", "simpleTest").Returns(new UnitName("untitled.s.untitled.simpleTest"));
                    yield return new TestCaseData("untitled", "9.507-*", "untitled", "simpleTest").Returns(new UnitName("untitled.9d507us.untitled.simpleTest"));
                    yield return new TestCaseData("Verwaltung U9", "9.510-9.511", "Mitarbeiter bearbeiten", "Rechte ändern").Returns(new UnitName("Verwaltung_U9.9d510u9d511.Mitarbeiter_bearbeiten.Rechte_ändern"));

                    yield return new TestCaseData(null, null, string.Empty, "untitled").Throws(typeof(ArgumentNullException));
                }
            }

            public IEnumerable<TestCaseData> TestCasesQualifiedStrings {
                get {
                    yield return new TestCaseData(new UnitName("app", "*", "useCase", "scene")).Returns("app.s.useCase.scene");
                    yield return new TestCaseData(new UnitName("f1", "9.507-9.510", "f3", "f4")).Returns("f1.9d507u9d510.f3.f4");

                    yield return new TestCaseData(new UnitName("app.9d600.test.scene")).Returns("app.9d600.test.scene");
                    yield return new TestCaseData(new UnitName("app.s.f3.f4")).Returns("app.s.f3.f4");
                }
            }

            public IEnumerable<TestCaseData> TestCasesParseFromFile {
                get {
                    yield return new TestCaseData("Unit untitled.9d507us.untitled.simpleTest").Returns(new UnitName("untitled", "9.507-*", "untitled", "simpleTest"));
                    yield return new TestCaseData("Unit app_base.s.un_titled.simpleTest").Returns(new UnitName("app base", "*", "un titled", "simpleTest"));
                }
            }
        }
    }
}