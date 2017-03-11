namespace Bumblebee.buddy.compiler.model {
    /// <summary>
    /// Extends <see cref="IInstructionPatternToken"/> to indicate that the instruction pattern token is parametrized.
    /// </summary>
    public interface IParametrizedInstructionPatternToken : IInstructionPatternToken {
        /// <summary>
        /// Returns the associated pattern parameter.
        /// </summary>
        IPatternParameter Value { get; }
    }
}