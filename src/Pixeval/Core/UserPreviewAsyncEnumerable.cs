﻿// Pixeval - A Strong, Fast and Flexible Pixiv Client
//  Copyright (C) 2019-2020 Dylech30th
// This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as
//  published by the Free Software Foundation, either version 3 of the
//  License, or (at your option) any later version.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Pixeval.Data.ViewModel;
using Pixeval.Data.Web;
using Pixeval.Data.Web.Delegation;
using Pixeval.Data.Web.Response;
using Pixeval.Objects.Exceptions;
using Pixeval.Objects.Generic;
using Pixeval.Objects.Primitive;

namespace Pixeval.Core
{
    public class UserPreviewAsyncEnumerable : AbstractPixivAsyncEnumerable<User>
    {
        private readonly string _keyword;

        public UserPreviewAsyncEnumerable(string keyword)
        {
            _keyword = keyword;
        }

        public override int RequestedPages { get; protected set; }

        public override IAsyncEnumerator<User> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new UserPreviewAsyncEnumerator(this, _keyword);
        }

        private class UserPreviewAsyncEnumerator : AbstractPixivAsyncEnumerator<User>
        {
            private readonly string _keyword;
            private UserNavResponse _entity;

            private IEnumerator<User> _userPreviewEnumerator;

            public UserPreviewAsyncEnumerator(IPixivAsyncEnumerable<User> enumerable, string keyword) : base(enumerable)
            {
                _keyword = keyword;
            }

            public override User Current => _userPreviewEnumerator.Current;

            protected override void UpdateEnumerator()
            {
                _userPreviewEnumerator = _entity.UserPreviews.NonNull().Select(u => new User {Avatar = u.User.ProfileImageUrls.Medium, Thumbnails = u.Illusts.NonNull().Select(_ => _.ImageUrl.SquareMedium).ToArray(), Id = u.User.Id.ToString(), Name = u.User.Name}).GetEnumerator();
            }

            public override async ValueTask<bool> MoveNextAsync()
            {
                if (_entity == null)
                {
                    if (await TryGetResponse($"https://app-api.pixiv.net/v1/search/user?filter=for_android&word={_keyword}") is (true, var model))
                    {
                        _entity = model;
                        UpdateEnumerator();
                    }
                    else
                    {
                        throw new QueryNotRespondingException();
                    }

                    Enumerable.ReportRequestedPages();
                }

                if (_userPreviewEnumerator.MoveNext()) return true;

                if (_entity.NextUrl.IsNullOrEmpty()) return false;

                if (await TryGetResponse(_entity.NextUrl) is (true, var res))
                {
                    _entity = res;
                    UpdateEnumerator();
                    Enumerable.ReportRequestedPages();
                    return true;
                }

                return false;
            }

            private static async Task<HttpResponse<UserNavResponse>> TryGetResponse(string url)
            {
                var res = (await HttpClientFactory.AppApiHttpClient().GetStringAsync(url)).FromJson<UserNavResponse>();
                if (res is { } response && !response.UserPreviews.IsNullOrEmpty()) return HttpResponse<UserNavResponse>.Wrap(true, response);

                return HttpResponse<UserNavResponse>.Wrap(false);
            }
        }
    }
}