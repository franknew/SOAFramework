using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SOAFramework.Library.Validator
{
    [Serializable]
    public class ValidatorManager
    {
        private List<ValidatorGroup> listValidatorGroup = new List<ValidatorGroup>();

        public List<ValidatorGroup> ValidatorGroup
        {
            get { return listValidatorGroup; }
            set { listValidatorGroup = value; }
        }

        public ValidateResult ValidateGroup(ValidatorGroup group)
        {
            ValidateResult result = new ValidateResult();
            result.IsValid = true;
            group.SortValidators();
            List<IValidatorBase> listValidators =  group.GetValidators();
            foreach (var validator in listValidators)
            {
                if (!validator.Validate())
                {
                    result.ControlID = group.Name;
                    result.ErrorMessage = validator.ErrorMessage;
                    result.IsValid = false;
                    return result;
                }
            }
            return result;
        }

        public ValidateResult Validate()
        {
            ValidateResult result = new ValidateResult();
            result.IsValid = true;
            //分组根据验证顺序排序
            SortGroup();
            foreach (var group in listValidatorGroup)
            {
                result = ValidateGroup(group);
                if (!result.IsValid)
                {
                    return result;
                }
            }
            return result;
        }

        public ValidateResult ValidateInForm(Form form)
        {
            ValidateResult result = new ValidateResult();
            result.IsValid = true;
            SortGroup();
            foreach (var group in listValidatorGroup)
            {

                if (form==null ||group == null || string.IsNullOrEmpty(group.Name)) continue;
                Control[] controls = form.Controls.Find(group.Name, true);
                if (controls.Length == 0)
                {
                    throw new Exception("在指定窗体中找不到ID为" + group.Name + "的控件，可能ValidatorGroup.Name设置错误。");
                }
                string value = controls[0].Text;
                group.SortValidators();
                List<IValidatorBase> listValidators = group.GetValidators();
                foreach (var validator in listValidators)
                {
                    //如果是自定义函数类验证，从控件中获得参数
                    if (validator is FunctionHandlerValidator)
                    {
                        FunctionHandlerValidator validatorTemp = validator as FunctionHandlerValidator;
                        Dictionary<string, object> dicCopy = new Dictionary<string, object>();
                        if (validatorTemp.Args != null)
                        {
                            foreach (var arg in validatorTemp.Args.Keys)
                            {
                                if (arg == "_value")
                                {
                                    continue;
                                }
                                Control[] controlsArg = form.Controls.Find(arg, true);
                                if (controlsArg.Length == 0)
                                {
                                    throw new Exception("在指定窗体中找不到ID为" + arg + "的控件，可能Validator.Args设置错误");
                                }
                                dicCopy[arg] = controlsArg[0].Text;
                            }
                            validatorTemp.Args = dicCopy;
                        }
                    }
                    validator.Value = value;
                    if (!validator.Validate())
                    {
                        result.ControlID = group.Name;
                        result.ErrorMessage = validator.ErrorMessage;
                        result.IsValid = false;
                        return result;
                    }
                }
            }
            return result;
        }

        public void AddValidatorGroup(ValidatorGroup group)
        {
            listValidatorGroup.Add(group);
        }
        
        public void ClearValidatorGroup()
        {
            listValidatorGroup.Clear();
        }

        public void RemoveValidatorGroup(ValidatorGroup group)
        {
            listValidatorGroup.Remove(group);
        }

        public List<ValidatorGroup> GetValidatorGroups()
        {
            return listValidatorGroup;
        }

        public ValidatorGroup FindGroup(string groupName)
        {
            return listValidatorGroup.Find(t => t.Name == groupName);
        }

        public void SortGroup()
        {
            listValidatorGroup.Sort(delegate(ValidatorGroup left, ValidatorGroup right)
            {
                return left.ValidateIndex - right.ValidateIndex;
            });
        }
    }
}
