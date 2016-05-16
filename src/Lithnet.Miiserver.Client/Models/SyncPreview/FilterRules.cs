using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
namespace Lithnet.Miiserver.Client
{
    public class FilterRules : NodeCache
    {
        internal FilterRules(XmlNode node)
            : base(node)
        {
        }


        public string Type
        {
            get
            {
                return this.GetValue<string>("@type");
            }
        }

        public bool HasError
        {
            get
            {
                return this.GetValue<bool>("@has-error");
            }
        }

        public FilterSetResult FilterSet
        {
            get
            {
                return this.GetObject<FilterSetResult>("filter-set");
            }
        }

        public Error Error
        {
            get
            {
                return this.GetObject<Error>("error");
            }
        }
    }
}
/*
      <stay-disconnector type="export" has-error="true">
 <filter-set></filter-set>
   <error type="connector-filter-rule-violation"></error>
 */
