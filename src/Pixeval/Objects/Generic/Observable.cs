﻿// Pixeval - A Strong, Fast and Flexible Pixiv Client
//  Copyright (C) 2019-2020 Dylech30th
// This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as
//  published by the Free Software Foundation, either version 3 of the
//  License, or (at your option) any later version.

using System;
using PropertyChanged;

namespace Pixeval.Objects.Generic
{
    [AddINotifyPropertyChangedInterface]
    public class Observable<T>
    {
        private T _value;

        public Observable(T value)
        {
            _value = value;
        }

        public T Value
        {
            get => _value;
            set
            {
                if (!_value.Equals(value))
                {
                    ValueChanged?.Invoke(Value, new ObservableValueChangedEventArgs<T>(_value, value));
                    _value = value;
                }
            }
        }

        public event EventHandler<ObservableValueChangedEventArgs<T>> ValueChanged;
    }

    public class ObservableValueChangedEventArgs<T> : EventArgs
    {
        public ObservableValueChangedEventArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public T OldValue { get; set; }

        public T NewValue { get; set; }
    }
}