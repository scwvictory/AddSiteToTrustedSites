using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace AddSiteToTrustedSites
{
    class RegController
    {
        private const string _rangesKey = @"Software\Microsoft\Windows\CurrentVersion\Internet Settings\ZoneMap\Ranges";
        private const string _rangeValue = @":Range";

        public List<SiteItem> GetSites()
        {
            try
            {
                var results = new List<SiteItem>();

                //レジストリから、既に登録されているRange(IP)を取得
                RegistryKey regKey = Registry.CurrentUser.OpenSubKey(_rangesKey, false);
                foreach (var keyName in regKey.GetSubKeyNames())
                {
                    var siteItem = new SiteItem();
                    var keyPath = string.Format(_rangesKey + @"\{0}", keyName);
                    var subKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(keyPath, false);

                    foreach (var valName in subKey.GetValueNames())
                    {
                        switch (valName)
                        {
                            case "*":
                                siteItem.Protocol = "*";
                                break;
                            case "http":
                                siteItem.Protocol = "http";
                                break;
                            case "https":
                                siteItem.Protocol = "https";
                                break;
                            case _rangeValue:
                                siteItem.Range = subKey.GetValue(valName).ToString();
                                break;
                        }
                    }
                    results.Add(siteItem);
                }
                return results;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void AddSite(SiteItem siteItem, int idx)
        {
            try
            {
                //Range* というキーに、プロトコルを示す値と、:Range 値を書き込む
                var subKey = Registry.CurrentUser.CreateSubKey(string.Format(_rangesKey + @"\Range{0}", idx));
                subKey.SetValue(siteItem.Protocol, 2, RegistryValueKind.DWord);
                subKey.SetValue(_rangeValue, siteItem.Range, RegistryValueKind.String);
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
