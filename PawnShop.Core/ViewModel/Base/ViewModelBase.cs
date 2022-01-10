using FluentValidation;
using FluentValidation.Internal;
using PawnShop.Validator.Base;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace PawnShop.Core.ViewModel.Base
{
    public abstract class ViewModelBase<T> : BindableBase, IDataErrorInfo, IViewModelBase, IRaiseCanExecuteChanged where T : class
    {
        #region private members

        private readonly ValidatorBase<T> _validator;


        #endregion private members

        #region constructor

        protected ViewModelBase(ValidatorBase<T> dialogValidator)
        {
            _validator = dialogValidator;
            Commands = new List<DelegateCommand>();
        }



        #endregion constructor

        #region Protected methods

        protected abstract T GetInstance();

        #endregion Protected methods

        #region IDataErrorInfo

        public string this[string columnName]
        {
            get
            {
                RaisePropertyChanged(nameof(HasErrors));
                RaiseAllCommands();
                var prop = new[] { columnName };
                var context = new ValidationContext<T>(GetInstance(), new PropertyChain(), new MemberNameValidatorSelector(prop));
                var validationResult = _validator.Validate(context);
                return validationResult.Errors.Any() ? validationResult.Errors.First().ErrorMessage : string.Empty;
            }
        }

        public string Error
        {
            get
            {
                if (_validator != null)
                {
                    var results = _validator.Validate(GetInstance());
                    if (results != null && results.Errors.Any())
                    {
                        var errors = string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage).ToArray());
                        return errors;
                    }
                }
                return string.Empty;
            }
        }

        public bool HasErrors => !string.IsNullOrEmpty(Error);




        #endregion IDataErrorInfo

        #region IRaiseCanExecuteChanged 

        public IList<DelegateCommand> Commands { get; set; }

        public void RaiseAllCommands()
        {
            if (Commands is null)
                return;

            foreach (var command in Commands)
                command.RaiseCanExecuteChanged();
        }

        #endregion
    }
}