using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddSiteToTrustedSites
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //現在登録されているサイト(IP)を取得
                var regCtrl = new RegController();
                List<SiteItem> rSiteItems = regCtrl.GetSites();

                //引数から追加するサイト情報を取得
                var converter = new SiteItemConverter();
                var siteItems = new List<SiteItem>();
                foreach (var arg in args)
                {
                    var siteItem = converter.ConvertFromString(arg);
                    if (siteItem != null)
                    {
                        //同じサイトが登録済みかどうか
                        var syn = false;
                        foreach (var rSite in rSiteItems)
                        {
                            if (rSite.Protocol == siteItem.Protocol && rSite.Range == siteItem.Range)
                            {
                                //サイトに変換できなかった場合
                                Console.WriteLine(@"'" + arg + "' は既に登録されています。");
                                syn = true;
                                break;
                            }
                        }
                        if (!syn)
                        {
                            //重複がなければ登録対象として追加
                            siteItems.Add(siteItem);
                        }
                    }
                    else
                    {
                        //サイトに変換できなかった場合
                        Console.WriteLine(@"'" + arg + "' は無効な値です。");
                    }
                }

                //レジストリにキーを追加
                var idx = rSiteItems.Count() + 1;
                foreach (var siteItem in siteItems)
                {
                    try
                    {
                        regCtrl.AddSite(siteItem, idx);
                        Console.WriteLine(@"'" + siteItem.SourceText + "' を信頼済みサイトに追加しました。");
                        idx = idx + 1;
                    }
                    catch (Exception e)
                    {
                        //書き込みエラー
                        Console.WriteLine(@"'" + siteItem.SourceText + "' の書き込みに失敗しました。");
                        Console.WriteLine(e.Message);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
        }
    }
}
