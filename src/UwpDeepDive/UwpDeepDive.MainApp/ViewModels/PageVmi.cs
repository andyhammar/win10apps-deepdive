using System;

namespace UwpDeepDive.MainApp.ViewModels
{
    public class PageVmi
    {
        public PageVmi(string name, Type type, object param = null)
        {
            Name = name;
            Type = type;
            Param = param;
        }

        public string Name { get; set; }
        public Type Type { get; set; }
        public object Param { get; }
    }
}