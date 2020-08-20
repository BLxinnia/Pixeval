﻿#region Copyright (C) 2019-2020 Dylech30th. All rights reserved.

// Pixeval - A Strong, Fast and Flexible Pixiv Client
// Copyright (C) 2019-2020 Dylech30th
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

#endregion

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Pixeval.Wpf.Data.Web;
using Pixeval.Wpf.Data.Web.Delegation;
using Pixeval.Wpf.Data.Web.Response;
using Pixeval.Wpf.Objects.Exceptions;
using Pixeval.Wpf.Objects.Generic;
using Pixeval.Wpf.Objects.Primitive;
using Pixeval.Wpf.ViewModel;

namespace Pixeval.Wpf.Core
{
    public class SpotlightQueryAsyncEnumerable : AbstractPixivAsyncEnumerable<SpotlightArticle>
    {
        private readonly int _start;

        public SpotlightQueryAsyncEnumerable(int start)
        {
            this._start = start < 1 ? 1 : start;
        }

        public override int RequestedPages { get; protected set; }

        public override IAsyncEnumerator<SpotlightArticle> GetAsyncEnumerator(
            CancellationToken cancellationToken = default)
        {
            return new SpotlightArticleAsyncEnumerator(this, _start);
        }

        private class SpotlightArticleAsyncEnumerator : AbstractPixivAsyncEnumerator<SpotlightArticle>
        {
            private int _current;

            private SpotlightResponse _entity;

            private IEnumerator<SpotlightArticle> _spotlightArticleEnumerator;

            public SpotlightArticleAsyncEnumerator(IPixivAsyncEnumerable<SpotlightArticle> enumerable, int current) :
                base(enumerable)
            {
                this._current = current;
            }

            public override SpotlightArticle Current => _spotlightArticleEnumerator.Current;

            protected override void UpdateEnumerator()
            {
                _spotlightArticleEnumerator = _entity.SpotlightArticles.NonNull().GetEnumerator();
            }

            public override async ValueTask<bool> MoveNextAsync()
            {
                if (_entity == null)
                {
                    if (await TryGetResponse() is (true, var model))
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

                if (_spotlightArticleEnumerator.MoveNext()) return true;

                if (_entity.NextUrl.IsNullOrEmpty()) return false;

                if (await TryGetResponse() is (true, var res))
                {
                    _entity = res;
                    UpdateEnumerator();
                    Enumerable.ReportRequestedPages();
                    return true;
                }

                return false;
            }

            private async Task<HttpResponse<SpotlightResponse>> TryGetResponse()
            {
                var res = await HttpClientFactory.AppApiService().GetSpotlights(_current++ * 10);

                if (res is { } response && !response.SpotlightArticles.IsNullOrEmpty()) return HttpResponse<SpotlightResponse>.Wrap(true, response);

                return HttpResponse<SpotlightResponse>.Wrap(false);
            }
        }
    }
}
