// Pixeval - A Strong, Fast and Flexible Pixiv Client
//  Copyright (C) 2019-2020 Dylech30th
// This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as
//  published by the Free Software Foundation, either version 3 of the
//  License, or (at your option) any later version.

using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Pixeval.Data.ViewModel;

namespace Pixeval.Objects.ValueConverters
{
    public class MultiCultureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s) return AppContext.AvailableCultures.FirstOrDefault(cul => cul.Name == s) ?? I18NOption.ChineseSimplified;

            return I18NOption.ChineseSimplified;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is I18NOption option) return option.Name;

            return I18NOption.ChineseSimplified.Name;
        }
    }
}