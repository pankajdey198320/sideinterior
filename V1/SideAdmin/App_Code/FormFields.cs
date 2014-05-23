using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class FormFields
{
    public FormFields()
    {
        this.FieldFormatter = new BaseScriptFormatter();
    }
    public string FieldId { get; set; }
    public string Caption { get; set; }
    public FieldType Type { get; set; }
    public string PlaceholderText { get; set; }
    public IFormFieldFormatter  FieldFormatter { get; set; }
    public enum FieldType
    {
        Text, TextArea, Checkbox, Radio, DropdownList, file
    }
}

public interface IFormFieldFormatter {
     string GetSelector(FormFields obj);
     string GetValueRetrivalString(FormFields obj);
}

public class BaseScriptFormatter:IFormFieldFormatter
{
    public virtual string GetSelector(FormFields obj)
    {
        return "#" + obj.FieldId;
    }

    public virtual string GetValueRetrivalString(FormFields obj)
    {
        return "$('" + this.GetSelector(obj) + "').val()";
    }
}

public class CheckboxFieldFormatter : BaseScriptFormatter
{
    public override string GetValueRetrivalString(FormFields obj)
    {
        return "$('" + this.GetSelector(obj) + "').prop('checked')"; 
    }
}