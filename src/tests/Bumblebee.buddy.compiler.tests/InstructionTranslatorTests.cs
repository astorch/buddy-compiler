using System;
using System.Collections;
using System.Collections.Generic;
using Bumblebee.buddy.compiler.model;
using Bumblebee.buddy.compiler.model.instructions.german;
using NUnit.Framework;

namespace Bumblebee.buddy.compiler.tests {
    [TestFixture]
    public class InstructionTranslatorTests {

        [TestCaseSource(typeof(TestCaseFactory), nameof(TestCaseFactory.ThrowCases))]
        public void GetTranslationInstruction_Throws(string instruction, Type expectedType) {
            Assert.Throws(expectedType, () => GetTranslationInstruction(instruction));
        }
        
        [TestCaseSource(typeof(TestCaseFactory), nameof(TestCaseFactory.TestCasesGetTranslationInstruction))]
        public IBuddyTranslationInstruction GetTranslationInstruction(string instruction) {
            return new InstructionTranslator().GetTranslationInstruction(instruction);
        }

        [TestCaseSource(typeof(TestCaseFactory), nameof(TestCaseFactory.TestCasesToDirective))]
        public string ToDirective(string line) {
            InstructionTranslator instructionTranslator = new InstructionTranslator();
            string directive = instructionTranslator.ToDirective(line);
            return directive;
        }

        class TestCaseFactory {
            public static IEnumerable<TestCaseData> TestCasesToDirective {
                get {
                    yield return new TestCaseData(@"Starte ""C:\EGUB\EGUB.exe"".").Returns(@"processHandle1 = start(,, ""C:\EGUB\EGUB.exe"")");
                    yield return new TestCaseData(@"Wechsle ""HIT"".").Returns(@"setsut(,, ""HIT"")");
                    yield return new TestCaseData(@"Warte <WindowAnmeldung> sichtbar.").Returns(@"wait(WindowAnmeldung, IsVisible, True, 60000)");
                    yield return new TestCaseData(@"Warte <WindowAnmeldung> verschwunden").Returns(@"wait(WindowAnmeldung, IsVisible, False, 60000)");
                    yield return new TestCaseData(@"Warte <Anwendung> bereit").Returns(@"wait(_Application, Idle, True, 60000)");
                    yield return new TestCaseData(@"Wähle <Liste:Mitarbeiter> ""Mitarbeiter"" ""Verwaltungskraft"" ""801 - Stefan E."" aus.").Returns(@"select(Liste:Mitarbeiter, Value, ""Mitarbeiter"", ""Verwaltungskraft"", ""801 - Stefan E."")");
                    yield return new TestCaseData(@"Wähle ""Formular"" ""§29"" in <Menu:Main> aus.").Returns(@"select(Menu:Main, Value, ""Formular"", ""§29"")");
//                    yield return new TestCaseData(@"Klicke <Tabelle:UArten> ""HU29"", ""Wiederholung"" ""Nachprüfung"" doppelt.").Returns(@"click(Tabelle:UArten, , ""HU29"", ""Wiederholung"", ""Nachprüfung"", Double");
                }
            }

            public static IEnumerable<TestCaseData> TestCasesGetTranslationInstruction {
                get {
                    yield return new TestCaseData("Wähle aus der Mehrzahl aus").Returns(new InstanceOfResult<SelectBuddyTranslationInstruction>());
                    yield return new TestCaseData("Rutsche auf und davon").Returns(null);
                    yield return new TestCaseData(" was geht ab").Returns(null);
                }
            }

            public static IEnumerable<TestCaseData> ThrowCases {
                get {
                    yield return new TestCaseData(null, typeof(ArgumentNullException));
                }
            }
        }

        class InstanceOfResult<TResultObject> where TResultObject : class, IBuddyTranslationInstruction {
            /// <summary>Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />.</summary>
            /// <returns>true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />; otherwise, false.</returns>
            /// <param name="obj">The object to compare with the current object. </param>
            public override bool Equals(object obj) {
                TResultObject resultObject = obj as TResultObject;
                if (resultObject == null) return false;
                return true;
            }
        }
        
    }
}
