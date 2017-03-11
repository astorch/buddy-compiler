using System;
using Bumblebee.buddy.compiler.packaging;
using NUnit.Framework;

namespace Bumblebee.buddy.compiler.tests.packaging {
    [TestFixture]
    public class UnitRoutineInfoTests {
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void ParseFromFile_ArgumentNullException_on_NULL() {
            UnitRoutineInfo.ParseFromFile(null);
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void ParseFromFile_ArgumentNullException_on_empty() {
            UnitRoutineInfo.ParseFromFile(string.Empty);
        }

        [Test]
        public void ParseFromFile_1Routine() {
            // Arrange
            string fileContent = TestTool.GetResourceFileContent("Bumblebee.buddy.compiler.tests.resources.UnitRoutineInfoTestFile1.tdil");

            // Act
            UnitRoutineInfo[] unitRoutines = UnitRoutineInfo.ParseFromFile(fileContent);

            // Assert
            Assert.IsNotNull(unitRoutines, "unitRoutines != null");
            Assert.IsNotEmpty(unitRoutines, "unitRoutines are empty");

            Assert.AreEqual(1, unitRoutines.Length, "unitRoutines.Length != 1");

            UnitRoutineInfo routine = unitRoutines[0];
            AssertRoutine("Anmeldung_an_allen_Modulen", new[] { "buero", "piNr", "piPass" }, routine);
        }

        [Test]
        public void ParseFromFile_3Routines() {
            // Arrange
            string fileContent = TestTool.GetResourceFileContent("Bumblebee.buddy.compiler.tests.resources.UnitRoutineInfoTestFile2.tdil");

            // Act
            UnitRoutineInfo[] unitRoutines = UnitRoutineInfo.ParseFromFile(fileContent);

            // Assert
            Assert.IsNotNull(unitRoutines, "unitRoutines != null");
            Assert.IsNotEmpty(unitRoutines, "unitRoutines are empty");

            Assert.AreEqual(3, unitRoutines.Length, "unitRoutines.Length != 3");

            UnitRoutineInfo routine1 = unitRoutines[0];
            UnitRoutineInfo routine2 = unitRoutines[1];
            UnitRoutineInfo routine3 = unitRoutines[2];

            AssertRoutine("Anmeldung_an_allen_Modulen", new[] { "buero", "piNr", "piPass" }, routine1);
            AssertRoutine("Routine_without_arguments", new string[0], routine2);
            AssertRoutine("Routine_with_one_argument", new []{"arg"}, routine3);
        }

        private void AssertRoutine(string expName, string[] expArgs, UnitRoutineInfo routine) {
            Assert.AreEqual(expName, routine.Name, "Unexpected routine name");

            UnitRoutineArgumentInfo[] argumentSet = routine.Arguments;
            Assert.IsNotNull(argumentSet, "argumentSet != null");

            if (expArgs.Length != 0)
                Assert.IsNotEmpty(argumentSet, "argumentSet is empty");

            Assert.AreEqual(expArgs.Length, argumentSet.Length, "Unexpected argument count");
            for (int i = -1; ++i != expArgs.Length;) 
                AssertArgument(i, expArgs[i], argumentSet[i]);
        }

        private void AssertArgument(int index, string name, UnitRoutineArgumentInfo argument) {
            Assert.AreEqual(index, argument.Index, "Unexpected argument index");
            Assert.AreEqual(name, argument.Name, "Unexpected argument name");
        }
    }
}