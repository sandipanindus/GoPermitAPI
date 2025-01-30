using System;
using System.Collections.Generic;
using System.Text;

namespace LabelPad.Domain.ApplicationClasses
{
   public class GetModulesAc
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleIcon { get; set; }
        public List<ScreensModel> ScreensModel { get; set; }
    }
    public class ScreensModel
    {
        public int ScreenId { get; set; }
        public string Label { get; set; }

        public string ScreenName { get; set; }
        public string ScreenIcon { get; set; }
        public int ModuleId { get; set; }
        public int RoleId { get; set; }
        public int ClientId { get; set; }
        public bool View { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
        public bool Approve { get; set; }
        public bool Reject { get; set; }
        public int LoginId { get; set; }
        public int RoleScreenId { get; set; }
    }
    public class MenuItem
    {
        public int ModuleId { get; set; }
        public string id { get; set; }
        public string icon { get; set; }
        public string label { get; set; }
        public string to { get; set; }
        public List<ScreenItems> Subs { get; set; }

    }
    public class ScreenItems
    {
        public int ModuleId { get; set; }
        public int ScreenId { get; set; }
        public string ScreenName { get; set; }
        public string icon { get; set; }
        public string label { get; set; }
        public string to { get; set; }
    }
}
