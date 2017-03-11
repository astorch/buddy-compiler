namespace bumblebee.buddy.compiler2.model {
    public class SubselectReference : AbstractReference {
        /// <inheritdoc />
        public SubselectReference(string value) : base(value) {
        }

        public AbstractReference[] References { get; set; }
    }
}