using System;

namespace UwpDeepDive.MainApp.ViewModels
{
    public class PageVmi
    {
        public PageVmi(string name, Type type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; set; } 
        public Type Type { get; set; } 
    }
}