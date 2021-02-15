using System;
using System.Collections.Generic;
using Bumblebee.buddy.compiler.model;
using Bumblebee.buddy.compiler.model.evaluation;
using Bumblebee.buddy.compiler.model.instructions.german;
using NUnit.Framework;

namespace Bumblebee.buddy.compiler.tests.model.instructions.german {
    [TestFixture]
    public class ExecuteInstructionTests {

        [TestCaseSource(typeof(TestCaseFactory), nameof(TestCaseFactory.ThrowCases))]
        public void Evaluate_Throws(string instruction, Type expectedType) {
            Assert.Throws(expectedType, () => Evaluate(instruction));
        }
        
        [TestCaseSource(typeof(TestCaseFactory), nameof(TestCaseFactory.TestCases))]
        public IInstructionEvaluationResult Evaluate(string instruction) {
            return new InstructionEvaluator().Evaluate(instruction,
                new InstructionTranslationInfo(new ExecuteBuddyTranslationInstruction()),
                new Dictionary<string, IPatternParameter>());
        }

        class TestCaseFactory {

            public static IEnumerable<TestCaseData> ThrowCases {
                get {
                    yield return new TestCaseData((string) null, typeof(ArgumentNullException));
                    yield return new TestCaseData(string.Empty, typeof(ArgumentNullException));
                }
            }
            
            public static IEnumerable<TestCaseData> TestCases {
                get {
                    yield return new TestCaseData("Führe [Meine Daten bearbeiten] aus.").Returns(EvaluationResult.Ok);
                    yield return new TestCaseData("Führe [Anmeldung] aus.").Returns(EvaluationResult.Ok);
                    yield return new TestCaseData("Führe [Anmeldung](\"00092\",\"801\",\"Passw801\") aus.").Returns(EvaluationResult.Ok);
                    yield return new TestCaseData("Führe [Anmeldung]($piBuero, $piNr, $piPass) aus.").Returns(EvaluationResult.Ok);
                    yield return new TestCaseData("Führe [Daten zurücksetzen] (\"Administrator\") aus.").Returns(EvaluationResult.Ok);
                    yield return new TestCaseData("Führe [Daten zurücksetzen] ($category) aus.").Returns(EvaluationResult.Ok);

                    yield return new TestCaseData("Führe <Meine Daten> aus.").Returns(new _ErrorEvaluationResult("2nd word of instruction does not match expected token '['. Word is '<'"));
                    yield return new TestCaseData("Tue [Meine Daten] ausführen.").Returns(new _ErrorEvaluationResult("1st word of instruction does not match expected token 'Führe'. Word is 'Tue'"));
                }
            }
            
        }
    }
}