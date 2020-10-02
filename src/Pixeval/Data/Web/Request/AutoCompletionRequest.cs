﻿// Pixeval - A Strong, Fast and Flexible Pixiv Client
//  Copyright (C) 2019-2020 Dylech30th
// This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as
//  published by the Free Software Foundation, either version 3 of the
//  License, or (at your option) any later version.

using Refit;

namespace Pixeval.Data.Web.Request
{
    public class AutoCompletionRequest
    {
        [AliasAs("merge_plain_keyword_results=true")]
        public bool MergePlainKeywordResult { get; set; } = true;

        [AliasAs("word")]
        public string Word { get; set; }
    }
}