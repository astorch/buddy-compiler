using System;
using System.Collections.Generic;

namespace Bumblebee.buddy.compiler.model.patternparameters.adjustments {
    public class ParameterAdjustmentTable : IDisposable {

        private static readonly IParameterAdjustment[] Adjustments = {
            new FormatParameterAdjustment(), 
        };

        private Dictionary<IParameterAdjustment, string> Values = new Dictionary<IParameterAdjustment, string>();

        private ParameterAdjustmentTable() {
            // Prevent from public initialization
        }

        public string Process(string instruction) {
            for (int i = -1; ++i != Adjustments.Length;) {
                IParameterAdjustment adjustment = Adjustments[i];
                string processedInstruction;
                string value;
                if (!adjustment.Process(instruction, out processedInstruction, out value)) continue;

                Values.Add(adjustment, value);
                instruction = processedInstruction;
            }

            return instruction;
        }

        public string GetValue(string adjustmentId) {
            if (string.IsNullOrEmpty(adjustmentId)) return null;
            using (Dictionary<IParameterAdjustment, string>.Enumerator itr = Values.GetEnumerator()) {
                while (itr.MoveNext()) {
                    KeyValuePair<IParameterAdjustment, string> entry = itr.Current;
                    string entryId = entry.Key.Id;
                    if (entryId == adjustmentId) return entry.Value;
                }
            }
            return null;
        }

        /// <inheritdoc />
        public void Dispose() {
            Current = null;
        }

        public static ParameterAdjustmentTable Current;

        public static ParameterAdjustmentTable Create() {
            return Current = new ParameterAdjustmentTable();
        }
    }
}