using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace AddSiteToTrustedSites
{
    class SiteItemConverter
    {
        public SiteItem ConvertFromString(string source)
        {
            try
            {
                if (!Regex.IsMatch(source, @"^(http|https|\*)\:\/\/([0-9]{1,3}|\*)\.([0-9]{1,3}|\*)\.([0-9]{1,3}|\*)\.([0-9]{1,3}|\*)$"))
                {
                    //書式不正
                    return null;
                }
                var items = source.Split(new string[] { ":", "/", "/" }, StringSplitOptions.RemoveEmptyEntries);
                var result = new SiteItem();
                result.SourceText = source;
                result.Protocol = items[0];
                result.Range = items[1];
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }



    }
}
