using SimpleGame.Permutation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Metrics
{
    public class GenAlgTestingStartParamaters
    {
        public int TimesToTestEachConfiguration;
        public int IncrementToRecord;

        private PermutationMechanism _permutator;

        public GenAlgTestingStartParamaters
        (
            int timesToTestEachConfiguration,
            int incrementToRecord,
            List<int> numGenerationParamaters,
            List<int> percentToKillParamaters,
            List<int> generationSizeParamaters,
            List<int> iterationsOfTestingPerSpeciesParamaters,
            List<int> mutationPercentParamaters,
            List<int> deciderTypeParamaters
        )
        {
            _permutator = new PermutationMechanism(new List<List<int>>()
            {
                numGenerationParamaters,
                percentToKillParamaters,
                generationSizeParamaters,
                iterationsOfTestingPerSpeciesParamaters,
                mutationPercentParamaters,
                deciderTypeParamaters
            });

            TimesToTestEachConfiguration = timesToTestEachConfiguration;
            IncrementToRecord = incrementToRecord;
        }

        public List<int[]> GetAllPermutations() => _permutator.GetAllPermutations();
    }
}
