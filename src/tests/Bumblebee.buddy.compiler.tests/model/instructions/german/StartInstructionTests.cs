using System;
using System.Collections.Generic;
using Bumblebee.buddy.compiler.model;
using Bumblebee.buddy.compiler.model.evaluation;
using Bumblebee.buddy.compiler.model.instructions.german;
using NUnit.Framework;

namespace Bumblebee.buddy.compiler.tests.model.instructions.german {
    [TestFixture]
    public class StartInstructionTests {

        [TestCaseSource(typeof(TestCaseFactory), nameof(TestCaseFactory.ThrowCases))]
        public void Evaluate_Throws(string instruction, Type expectedResult) {
            Assert.Throws(expectedResult, () => Evaluate(instruction));
        }
        
        [TestCaseSource(typeof(TestCaseFactory), nameof(TestCaseFactory.TestCases))]
        public IInstructionEvaluationResult Evaluate(string instruction) {
            return new InstructionEvaluator().Evaluate(
                instruction,
                new InstructionTranslationInfo(new StartBuddyTranslationInstruction()),
                new Dictionary<string, IPatternParameter>()
            );
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
                    yield return new TestCaseData(@"Starte ""C:\EGUB\EGUB.exe"".")
                        .Returns(EvaluationResult.Ok);

                    yield return new TestCaseData("Starte {Verwaltung}.")
                        .Returns(
                            new _ErrorEvaluationResult("2nd word of instruction does not match expected token 'Parameter reference 'path' (string)'. Word is '{Verwaltung}'")
                        );
                }
            }
            
        }
    }
}