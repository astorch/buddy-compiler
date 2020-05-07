using System;
using System.Collections;
using System.Collections.Generic;
using Bumblebee.buddy.compiler.model;
using Bumblebee.buddy.compiler.model.evaluation;
using Bumblebee.buddy.compiler.model.instructions.german;
using NUnit.Framework;

namespace Bumblebee.buddy.compiler.tests.model.instructions.german {
    [TestFixture]
    public class SwitchInstructionTests {

        [TestCaseSource(typeof(TestCaseFactory))]
        public IInstructionEvaluationResult Evaluate(string instruction) {
            return new InstructionEvaluator().Evaluate(
                instruction,
                new InstructionTranslationInfo(new SwitchBuddyTranslationInstruction()),
                new Dictionary<string, IPatternParameter>()
            );
        }

        class TestCaseFactory : IEnumerable {
            /// <inheritdoc />
            public IEnumerator GetEnumerator() {
                yield return new TestCaseData(@"Wechsle ""EGUB"".")
                    .Returns(EvaluationResult.Ok);
                
                yield return new TestCaseData((string)null)
                    .Throws(typeof(ArgumentNullException));
                
                yield return new TestCaseData(string.Empty)
                    .Throws(typeof(ArgumentNullException));
                
                yield return new TestCaseData("Wechsle einfach.")
                    .Returns(
                        new _ErrorEvaluationResult("2nd word of instruction does not match expected token 'Parameter reference 'path' (string)'. Word is 'einfach'")
                    );
            }
        }
        
    }
}