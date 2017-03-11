namespace bumblebee.buddy.compiler2.model {
    public abstract class AbstractReference {
        /// <inheritdoc />
        protected AbstractReference(string value) {
            Value = value;
        }

        public string Value { get; set; }
    }
}