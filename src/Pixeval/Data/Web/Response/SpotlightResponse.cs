﻿// Pixeval - A Strong, Fast and Flexible Pixiv Client
//  Copyright (C) 2019-2020 Dylech30th
// This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as
//  published by the Free Software Foundation, either version 3 of the
//  License, or (at your option) any later version.

using System.Collections.Generic;
using Newtonsoft.Json;
using Pixeval.Data.ViewModel;

namespace Pixeval.Data.Web.Response
{
    public class SpotlightResponse
    {
        [JsonProperty("spotlight_articles")]
        public List<SpotlightArticle> SpotlightArticles { get; set; }

        [JsonProperty("next_url")]
        public string NextUrl { get; set; }
    }
}