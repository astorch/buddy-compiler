namespace bumblebee.buddy.compiler2.model {
    public class TestcaseContext {
        public ApplicationContext Application { get; set; }

        public UsecaseContext Usecase { get; set; }

        public ScenarioContext Scenario { get; set; }

        public Import[] Imports { get; set; }

        public StepsContext Steps { get; set; }
    }
}