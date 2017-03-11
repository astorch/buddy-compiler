using System;
using System.Collections.Generic;
using Bumblebee.buddy.compiler.model;
using Bumblebee.buddy.compiler.model.evaluation;
using Bumblebee.buddy.compiler.model.instructions.german;
using NUnit.Framework;

namespace Bumblebee.buddy.compiler.tests.model.instructions.german {
    [TestFixture]
    public class WaitInstructionTests {

        [Test, TestCaseSource(typeof(TestCaseFactory), "TestCases")]
        public IInstructionEvaluationResult Evaluate(string instruction) {
            return new InstructionEvaluator().Evaluate(instruction,
                new InstructionTranslationInfo(new WaitBuddyTranslationInstruction()),
                new Dictionary<string, IPatternParameter>());
        }

        class TestCaseFactory {
            public IEnumerable<TestCaseData> TestCases {
                get {
                    yield return new TestCaseData("Warte <WindowAnmeldung> sichtbar.").Returns(EvaluationResult.Ok);
                    yield return new TestCaseData("Warte <WindowAnmeldung> verschwunden").Returns(EvaluationResult.Ok);
                    yield return new TestCaseData("Warte <Anwendung> bereit").Returns(EvaluationResult.Ok);

                    yield return new TestCaseData((string)null).Throws(typeof(ArgumentNullException));
                    yield return new TestCaseData(string.Empty).Throws(typeof(ArgumentNullException));

                    yield return new TestCaseData("Warte Hauptfenster sichtbar.").Returns(new _ErrorEvaluationResult("2nd word of instruction does not match expected token '<'. Word is 'Hauptfenster'"));
                    yield return new TestCaseData(@"Starte ""C:\EGUB\EGUB.exe"".").Returns(new _ErrorEvaluationResult("1st word of instruction does not match expected token 'Warte'. Word is 'Starte'"));
                }
            }
        }
    }
}