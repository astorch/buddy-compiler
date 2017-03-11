namespace bumblebee.buddy.compiler2.model {
    public class Version {
        public Version(string min, string max) {
            Min = min;
            Max = max;
        }

        public string Min { get; set; }

        public string Max { get; set; }

    }
}