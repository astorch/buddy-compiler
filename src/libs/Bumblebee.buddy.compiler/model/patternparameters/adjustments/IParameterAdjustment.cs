namespace Bumblebee.buddy.compiler.model.patternparameters.adjustments {
    public interface IParameterAdjustment {

        /// <summary>
        /// Returns the id of the adjustment.
        /// </summary>
        string Id { get; }

        bool Process(string instruction, out string processedInstruction, out string value);
    }
}