using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class FilterRules : XmlObjectBase
    {
        internal FilterRules(XmlNode node)
            : base(node)
        {
        }


        public string Type => this.GetValue<string>("@type");

        public bool HasError => this.GetValue<bool>("@has-error");

        public FilterSetResult FilterSet => this.GetObject<FilterSetResult>("filter-set");

        public Error Error => this.GetObject<Error>("error");
    }
}
/*
      <stay-disconnector type="export" has-error="true">
 <filter-set></filter-set>
   <error type="connector-filter-rule-violation"></error>
 */
