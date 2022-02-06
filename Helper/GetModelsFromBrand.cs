    static void GetModelsFromBrand()
    {
        foreach (var brand in brands)
        {
            var links = _autoScoutFetcher.GetHtmlLinks(brand!, pages: 1);
            var htmls = _autoScoutFetcher.GetHtmlDocuments(links);
            if (htmls.Count is 0)
            {
                File.WriteAllText($@"C:\Users\RIG\source\repos\AutoScout\AutoScout.Dev\Data\BrandModels\NotFound_{brand}.txt", "no models found");
                continue;
            }

            var content = htmls[0].DocumentNode.InnerHtml;

            var split1 = content.Split("\"availableModelModelLines\" : ")[1];
            var split2 = split1.Split("\"topModelIds\"")[0];
            var newJson = split2.Substring(0, split2.Length - 12);

            var models = JsonConvert.DeserializeObject<List<ModelModel>>(newJson)!
                .Select(x => x.Name)
                .Select(x => x!.Replace(' ', '-'))
                .Select(x => x!.ToLower())
                .ToList();

            var serialized = JsonConvert.SerializeObject(models, Formatting.Indented);

            File.WriteAllText(@$"C:\Users\RIG\source\repos\AutoScout\AutoScout.Dev\Data\BrandModels\{brand}Models.json", serialized);
        }
    }
