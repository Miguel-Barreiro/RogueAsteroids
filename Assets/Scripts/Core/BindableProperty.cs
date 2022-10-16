using System;
using System.Collections.Generic;

namespace Core
{
	
	// MVVM style properties to avoid having unnecessary events
	public sealed class BindableProperty<T>
	{
		public event Action<T> OnChanged;
        
		private T _value;

		public T Value
		{
			get => _value;
			set
			{
				if (!EqualityComparer<T>.Default.Equals(_value, value))
				{
					_value = value;
					InvokeOnChanged();
				}
			} 
		}

		public BindableProperty(T value = default(T))
		{
			_value = value;
		}
        
		public static implicit operator BindableProperty<T>(T value) => new BindableProperty<T>(value);

		private void InvokeOnChanged()
		{
			OnChanged?.Invoke(_value);
		}

		public void SubscribeToChanged(Action<T> listener)
		{
			OnChanged += listener;
			listener.Invoke(_value);
		}

		public void UnsubscribeFromChanged(Action<T> listener)
		{
			OnChanged -= listener;
		}
	}
}