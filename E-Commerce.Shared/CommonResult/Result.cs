using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.CommonResult
{
    //for void
    public class Result
    {
        private List<Error> _errors = [];
        public bool IsSucceed { get; }
        public bool IsFail => !IsSucceed;
        public IReadOnlyList<Error> Errors => _errors;
        //in succeed
        protected Result() 
        {
            IsSucceed = true;
        }
        //in fail with one error
        protected Result(Error error)
        {
            _errors.Add(error);
        }
        //in fail with multiple errors
        protected Result(List<Error> errors)
        {
            _errors.AddRange(errors);
        }

        //Static Factory Methods
        public static Result Ok() => new Result();
        public static Result Fail(Error error) => new Result(error);
        public static Result Fail(List<Error> errors) => new Result(errors);

    }


    ////REsult with with Value
    public class Result<TValue>:Result
    {
        private TValue _value;
        public TValue Value => IsSucceed ? _value : throw new InvalidOperationException("Can Not Access The Value Of Failed Operation");
        //if is succeed
        private Result(TValue value) : base()
        {
            _value = value;
        }
        
        private Result(Error error) : base(error)
        {
            _value = default!;
        }
        private Result(List<Error> errors) : base(errors)
        {
            _value = default!;
        }
        //Static Factory Methods
        public static Result<TValue> Ok(TValue value) =>new Result<TValue>(value);
        public new static Result<TValue> Fail(Error error) =>new Result<TValue>(error);
        public new static Result<TValue> Fail(List<Error> errors) =>new Result<TValue>(errors);

        //Implicit Casting operator 
        public static implicit operator Result<TValue>(TValue value) => Ok(value); //Result<Tvalue>.Ok() 
        public static implicit operator Result<TValue>(Error error) => Fail(error);//Result<Tvalue>.Fail
        public static implicit operator Result<TValue>(List<Error> errors) => Fail(errors);
    }
}
