using System;
using System.Collections.Generic;
using System.Text;
[assembly: CLSCompliant(true)]
namespace SOAFramework.Library.Validator
{
    [Serializable]
    public class ValidatorGroup
    {
        private List<IValidatorBase> listValidator = new List<IValidatorBase>();

        public List<IValidatorBase> ValidatorList
        {
            get { return listValidator; }
            set { listValidator = value; }
        }

        /// <summary>
        /// 分组名称，一般为控件ID
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 验证顺序
        /// </summary>
        public int ValidateIndex { get; set; }

        public ValidatorGroup(string name = "", int validateIndex = 0)
        {
            Name = name;
            ValidateIndex = validateIndex;
        }

        public void AddValidator(IValidatorBase validator)
        {
            if(validator!=null)
            listValidator.Add(validator);
        }

        public void ClearValidator()
        {
            listValidator.Clear();
        }

        public List<IValidatorBase> GetValidators()
        {
            return listValidator;
        }

        public void SetValidatorValue(object value)
        {
            foreach (var validator in listValidator)
            {
                validator.Value = value;
            }
        }

        public IValidatorBase FindValidator(string ID)
        {
            IValidatorBase validatorBase = listValidator.Find(p => p.ID == ID);
            return validatorBase;
        }

        public void SortValidators()
        {
            listValidator.Sort(delegate(IValidatorBase left, IValidatorBase right)
            {
                return left.ValidateIndex - right.ValidateIndex;
            });
        }
    }
}
