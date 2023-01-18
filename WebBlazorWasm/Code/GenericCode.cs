using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Template.ServerCode
{

     public class AutoCompleteModel
    {

        public AutoCompleteModel(string value, string text)
        {
            this.value = value;
            this.text = text;
        }

        public string value { get; set; }
        public string text { get; set; }

    }

    public class PostedFileImageReturn
    {
        public string PathFileName { get; set; }

        public string Width { get; set; }

        public string Height { get; set; }


    }


}