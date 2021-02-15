using System;
using System.Collections.Generic;
using Bumblebee.buddy.compiler.model;
using Bumblebee.buddy.compiler.model.instructions.german;
using NUnit.Framework;
using Bumblebee.buddy.compiler.model.evaluation;

namespace Bumblebee.buddy.compiler.tests.model.instructions.german {
    [TestFixture]
    public class AssertInstructionTests {
        
        [TestCaseSource(typeof(TestCaseFactory), nameof(TestCaseFactory.ThrowCases))]
        public void Evaluate_Throws(string instruction, Type exceptionType) {
            Assert.Throws(exceptionType, () => Evaluate(instruction));
        }
        
        [TestCaseSource(typeof(TestCaseFactory), nameof(TestCaseFactory.TestCases))]
        public IInstructionEvaluationResult Evaluate(string instruction) {
            return new InstructionEvaluator().Evaluate(
                instruction,
                new InstructionTranslationInfo(new AssertBuddyTranslationInstruction()),
                new Dictionary<string, IPatternParameter>()
            );
        }

        class TestCaseFactory {
            
            public static IEnumerable<TestCaseData> TestCases {
                get {
                    yield return new TestCaseData("Prüfe <ButtonAnmelden> auswählbar.").Returns(EvaluationResult.Ok);
                    yield return new TestCaseData("Prüfe <Button:Anmelden> sichtbar.").Returns(EvaluationResult.Ok);
                    yield return new TestCaseData("Prüfe <TextBox:Passwort> \"abc\" enthält.").Returns(EvaluationResult.Ok);
                    yield return new TestCaseData("Prüfe ob <ButtonAnmelden> auswählbar.").Returns(new _ErrorEvaluationResult("2nd word of instruction does not match expected token '<'. Word is 'ob'"));
                }
            }

            public static IEnumerable<TestCaseData> ThrowCases {
                get {
                    yield return new TestCaseData((string) null, typeof(ArgumentNullException));
                    yield return new TestCaseData(string.Empty, typeof(ArgumentNullException));
                }
            }
            
        }
    }
}