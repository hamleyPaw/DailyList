using System;
using System.ComponentModel;
using System.Linq.Expressions;

using HamleyPaw.DailyList.ViewModels.Contexts;

namespace HamleyPaw.DailyList.ViewModels {
    public abstract class ViewModelBase : INotifyPropertyChanged {
        private readonly IViewModelContext context;

        public ViewModelBase() { }

        public ViewModelBase(IViewModelContext context) {
            this.context = context;
        }

        public IViewModelContext Context {
            get { return context; }
        }

        // ?? Does this need to be at the Base level
        // ?? is anyone other than the Main View going to use it
        public event EventHandler RequestClose;

        protected void OnRequestClose() {
            EventHandler handler = this.RequestClose;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        protected virtual void OnPropertyChanged(string propertyName) {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// Use a lambda expression to ensure we get the correct name
        /// </summary>
        /// <param name="expression">A lambda expression of the propery that has a new value.</param>
        protected void OnPropertyChanged(Expression<Func<object>> expression) {
            if (this.PropertyChanged != null) {
                this.OnPropertyChanged(PropertyNameHelper.GetPropertyName(expression));
            }
        }

        public static class PropertyNameHelper {
            public static string GetPropertyName(Expression<Func<object>> expression) {
                return GetPropertyNameUntyped(expression);
            }

            public static string GetPropertyNameUntyped(LambdaExpression untypedExpression) {
                Expression body = ((LambdaExpression)untypedExpression).Body;

                // if expression if of the form x => (T)x.Property, remove the cast
                if (body.NodeType == ExpressionType.Convert) {
                    body = ((UnaryExpression)body).Operand;
                }

                return ((MemberExpression)body).Member.Name;
            }
        }

        #endregion INotifyPropertyChanged Members
    }
}
