﻿// Pixeval - A Strong, Fast and Flexible Pixiv Client
//  Copyright (C) 2019-2020 Dylech30th
// This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as
//  published by the Free Software Foundation, either version 3 of the
//  License, or (at your option) any later version.

using System.Net.Http;
using System.Threading.Tasks;
using Pixeval.Data.Web.Request;
using Pixeval.Data.Web.Response;
using Refit;

namespace Pixeval.Data.Web.Protocol
{
    [Headers("User-Agent: PixivAndroidApp/5.0.64 (Android 6.0)", "Content-Type: application/x-www-form-urlencoded")]
    public interface IWebApiProtocol
    {
        [Post("/setting_user.php")]
        Task<HttpResponseMessage> ToggleR18State([Body(BodySerializationMethod.UrlEncoded)]
                                                 ToggleR18StateRequest toggleR18StateRequest);

        [Get("/ajax/showcase/article")]
        Task<SpotlightArticleResponse> GetSpotlightArticles([AliasAs("article_id")]
                                                            string articleId);

        [Get("/touch/ajax/user/details")]
        Task<WebApiUserDetailResponse> GetWebApiUserDetail(string id);
    }
}