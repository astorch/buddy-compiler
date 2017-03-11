namespace bumblebee.buddy.compiler2.model {
    public class UnitInvocationParameterReference : AbstractReference {
        /// <inheritdoc />
        public UnitInvocationParameterReference(string value) : base(value) {
        }

        public UnitInvocationParameter[] Parameters { get; set; }
    }
}