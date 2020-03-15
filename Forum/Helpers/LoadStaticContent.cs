using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Forum.Helpers
{
    public class LoadStaticContent
    {
        public LoadStaticContent()
        {
        }

        public List<Tags> LoadTags()
        {
            var pathToJson = Path.Combine("StaticData", "Tags.json");
            using (StreamReader tags = new StreamReader(pathToJson))
            {
                return JsonConvert.DeserializeObject<List<Tags>>(tags.ReadToEnd());
            }
        }

        //public List<SelectListItem> GetSecretQuestions()
        //{
        //    var pathToJson = Path.Combine("Helpers", "SecretQuestion.json");
        //    using (StreamReader secretQuestionData = new StreamReader(pathToJson))
        //    {
        //        return JsonConvert.DeserializeObject<List<SelectListItem>>(secretQuestionData.ReadToEnd());
        //    }
        //}
    }
}
